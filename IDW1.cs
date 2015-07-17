using DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.SpatialAnalystTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
/**2.05 IDW功能完成，但是相同的参数，输出结果跟ARCGIS上面的不同
 * LILEI**/
namespace MyDatapreMenu
{
    public partial class IDW1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        public int layerIndex;
        private IFeatureLayer pLayer;
        private ITable pTable;

        #region 禁止最大化窗体
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")] //导入API函数
        extern static System.IntPtr GetSystemMenu(System.IntPtr hWnd, System.IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);
        static int MF_BYPOSITION = 0x400;
        static int MF_REMOVE = 0x1000;
        #endregion

        public IDW1()
        {
            InitializeComponent();
            RemoveMenu(GetSystemMenu(Handle, IntPtr.Zero), 0, MF_BYPOSITION | MF_REMOVE);
        }

        private void IDW1_Load(object sender, EventArgs e)
        {
            //清空选项
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Items.Add("");
            comboBox2.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

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
            //Z值字段的选择并更新下拉菜单控件2的下拉选项
            layerIndex = comboBox1.SelectedIndex;
            string text = comboBox1.SelectedText;
            if (layerIndex == -1)
            {
                MessageBox.Show("必须选择一个图层");
                return;
            }
            comboBox2.Enabled = true;

            pLayer = (IFeatureLayer)pMap.get_Layer(layerIndex);
            pTable = (ITable)pLayer;

            int fieldCount, i;
            fieldCount = pTable.Fields.FieldCount;
            comboBox2.Items.Clear();

            for (i = 0; i < fieldCount; i++)
            {
                comboBox2.Items.Add(pTable.Fields.get_Field(i).Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "TIFF tif|*.tif|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pFilePath, pFileName;
                int index = this.saveFileDialog1.FileName.LastIndexOf("\\");
                pFilePath = this.saveFileDialog1.FileName.Substring(0, index);
                pFileName = this.saveFileDialog1.FileName.Substring(index + 1);

                textBox2.Text = pFilePath + "\\" + pFileName;
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            label7.Text = "点数：";
            label8.Text = "最大距离：";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            label7.Text = "距离：";
            label8.Text = "最小点数：";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            IFeatureLayer layer = GetFeatureLayer((string)comboBox1.SelectedItem);//输入点要素
            string Zfield = (string)comboBox2.SelectedItem;//Z字段
            string outRaster = textBox2.Text;//输出栅格
            object cellSize = textBox3.Text;//输出像元大小
            double power = double.Parse(textBox4.Text);//幂
            string searchRadius;//搜索半径
            if (radioButton1.Checked)
            {
                searchRadius = "RadiusVariable(" + textBox5.Text + "," + textBox6.Text + ")";
            }
            else
            {
                searchRadius = "RadiusFixed(" + textBox5.Text + "," + textBox6.Text + ")";
            }
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置IDW参数
            Idw idw = new Idw();
            idw.in_point_features = layer;
            idw.z_field = Zfield;
            idw.out_raster = outRaster;
            idw.cell_size = cellSize;
            idw.power = power;
            idw.search_radius = searchRadius;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(idw, null);
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
            b.Show(button3, false);
            #endregion
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
