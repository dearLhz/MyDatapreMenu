using DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MyDatapreMenu
{
    public partial class RasterToPolygon1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        public int layerIndex;
        private IRasterLayer pLayer;
        private ITable pTable;
        private int m_nFieldIndex;

        #region 禁止最大化窗体
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")] //导入API函数
        extern static System.IntPtr GetSystemMenu(System.IntPtr hWnd, System.IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);
        static int MF_BYPOSITION = 0x400;
        static int MF_REMOVE = 0x1000;
        #endregion

        public RasterToPolygon1()
        {
            InitializeComponent();
            RemoveMenu(GetSystemMenu(Handle, IntPtr.Zero), 0, MF_BYPOSITION | MF_REMOVE);
        }

        private void RasterToPolygon1_Load(object sender, EventArgs e)
        {
            //清空选项
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Items.Add("");
            comboBox2.Text = "";
            textBox1.Text = "";

            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;
        }

        private IRasterLayer GetRasterLayer(string layerName)
        {
            //从地图上得到图层
            IEnumLayer layers = GetLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IRasterLayer;
            }

            return null;
        }

        private IEnumLayer GetLayers()
        {
            UID uid = new UIDClass();
            uid.Value = "{D02371C7-35F7-11D2-B1F2-00C04F8EDEFF}";//得到栅格图层
            IEnumLayer layers = pMap.get_Layers(uid, true);
            
            return layers;
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            //加载主窗体已经打开的所有图层名称
            comboBox1.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                comboBox1.Items.Add(pMap.get_Layer(i).Name);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //待转换数据的选择并更新下拉菜单控件2的下拉选项
            layerIndex = comboBox1.SelectedIndex;
            string text = comboBox1.SelectedText;
            if (layerIndex == -1)
            {
                MessageBox.Show("必须选择一个图层");
                return;
            }
            comboBox2.Enabled = true;
            try
            {
                pLayer = (IRasterLayer)pMap.get_Layer(layerIndex);
            }
            catch
            {
                MessageBox.Show("请选择栅格影像！");
                comboBox1.SelectedText = "";
                return;
            }
            pTable = (ITable)pLayer;

            int fieldCount, i;
            fieldCount = pTable.Fields.FieldCount;
            comboBox2.Items.Clear();

            for (i = 0; i < fieldCount; i++)
            {
                comboBox2.Items.Add(pTable.Fields.get_Field(i).Name);
            }
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            m_nFieldIndex = comboBox2.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "shp|*.shp|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pFilePath, pFileName;
                int index = this.saveFileDialog1.FileName.LastIndexOf("\\");
                pFilePath = this.saveFileDialog1.FileName.Substring(0, index);
                pFileName = this.saveFileDialog1.FileName.Substring(index + 1);

                textBox1.Text = pFilePath + "\\" + pFileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            IRasterLayer layer = GetRasterLayer((string)comboBox1.SelectedItem);
            string field = (string)comboBox2.SelectedItem;
            string out_polygon = textBox1.Text;
            string simplify;
            if (radioButton1.Checked)
            {
                simplify = radioButton1.Text;
            }
            else
            {
                simplify = radioButton2.Text;
            }
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置矢量转栅格参数            
            RasterToPolygon rastertopolygon = new RasterToPolygon();
            rastertopolygon.in_raster = layer;
            rastertopolygon.out_polygon_features = out_polygon;
            rastertopolygon.raster_field = field;
            rastertopolygon.simplify = simplify;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(rastertopolygon, null);
            #endregion
            circularProgress1.IsRunning = false;
            #region 运行完成之后的信息提示窗口
            balloonTipFocus.Enabled = true;

            DevComponents.DotNetBar.Balloon b = new DevComponents.DotNetBar.Balloon();
            b.Style = eBallonStyle.Alert;
            //b.CaptionImage = balloonTipFocus.CaptionImage.Clone() as Image;
            b.CaptionText = "信息提示";
            b.Text = "运行成功！";
            b.AlertAnimation = eAlertAnimation.TopToBottom;
            b.AutoResize();
            b.AutoClose = true;
            b.AutoCloseTimeOut = 4;
            b.Owner = this;
            b.Show(button4, false);
            #endregion

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
