using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.esriSystem;
using System.Diagnostics;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.DataSourcesFile;
using System.Runtime.InteropServices;
/**1.29 LILEI
 * 矢量转栅格功能完成
 * 1.31 LILEI
 * 信息提示窗完成**/
namespace MyDatapreMenu
{
    public partial class FeatureToRaster1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        public int layerIndex;
        private IFeatureLayer pLayer;
        private ITable pTable;
        int m_nFieldIndex;
        string pFilePath, pFileName;

        #region 禁止最大化窗体
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")] //导入API函数
        extern static System.IntPtr GetSystemMenu(System.IntPtr hWnd, System.IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);
        static int MF_BYPOSITION = 0x400;
        static int MF_REMOVE = 0x1000;
        #endregion

        public FeatureToRaster1()
        {
            InitializeComponent();
            RemoveMenu(GetSystemMenu(Handle, IntPtr.Zero), 0, MF_BYPOSITION | MF_REMOVE);
        }
        private void FeatureToRaster1_Load(object sender, EventArgs e)
        {
            //清空选项
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Items.Add("");
            comboBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";

            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;
        }

        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            //从地图上得到图层
            IEnumLayer layers = GetLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IFeatureLayer;
            }

            return null;
        }

        private IEnumLayer GetLayers()
        {
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";//得到矢量图层
            IEnumLayer layers = pMap.get_Layers(uid, true);

            return layers;
        }
        /// <summary>
        /// 输入要素事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                pLayer = (IFeatureLayer)pMap.get_Layer(layerIndex);
            }
            catch
            {
                MessageBox.Show("请选择矢量图像！");
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
        /// <summary>
        /// 输出栅格地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "TIFF tif|*.tif|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                int index = this.saveFileDialog1.FileName.LastIndexOf("\\");
                pFilePath = this.saveFileDialog1.FileName.Substring(0, index);
                pFileName = this.saveFileDialog1.FileName.Substring(index + 1);

                textBox1.Text = pFilePath + "\\" + pFileName;
            }


        }

        /// <summary>
        /// 像元大小的textbox只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '.') || e.KeyChar == (char)8)   //(char)8是退格键
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
        /// <summary>
        /// 确定BUTTON事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            IFeatureLayer layer = GetFeatureLayer((string)comboBox1.SelectedItem);
            string field = (string)comboBox2.SelectedItem;
            string out_raster = textBox1.Text;
            object cellSize = textBox2.Text;
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置矢量转栅格参数            
            FeatureToRaster featuretoraster = new FeatureToRaster();
            featuretoraster.in_features = layer;
            featuretoraster.field = field;
            featuretoraster.cell_size = cellSize;
            featuretoraster.out_raster = out_raster;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(featuretoraster, null);
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
            b.Show(button2, false);
            #endregion

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
