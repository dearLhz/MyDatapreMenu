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
using System.Text;
using System.Windows.Forms;

namespace MyDatapreMenu
{
    public partial class Kriging1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        public int layerIndex;
        private IFeatureLayer pLayer;
        private ITable pTable;
        object lagsize, majorrange, partialsill, nugget;//高级参数里面的四个参数

        public Kriging1()
        {
            InitializeComponent();
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

                textBox1.Text = pFilePath + "\\" + pFileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kriging2 kriging = new Kriging2();
            kriging.ShowDialog();
            lagsize = kriging.LagSize;//步长大小
            majorrange = kriging.MajorRange;//主要范围
            partialsill = kriging.PartialSill;//偏基台值
            nugget = kriging.Nugget;//块金值
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "TIFF tif|*.tif|All files (*.*)|*.*";
            saveFileDialog2.RestoreDirectory = true;

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string pFilePath, pFileName;
                int index = this.saveFileDialog2.FileName.LastIndexOf("\\");
                pFilePath = this.saveFileDialog2.FileName.Substring(0, index);
                pFileName = this.saveFileDialog2.FileName.Substring(index + 1);

                textBox7.Text = pFilePath + "\\" + pFileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region GP工具的使用
            #region 得到参数
            IFeatureLayer layer = GetFeatureLayer((string)comboBox1.SelectedItem);//输入点要素
            string Zfield = (string)comboBox2.SelectedItem;//Z字段
            string outRaster = textBox1.Text;//输出栅格


            string semiVariogramprops = "";//半变异函数属性
            string semivariogramType;//克里金方法
            if (radioButton1.Checked)
            {
                semivariogramType = (string)comboBox3.SelectedItem;
                semiVariogramprops = "KrigingModelOrdinary(" + semivariogramType + "," + lagsize + "," + majorrange + "," + partialsill + "," + nugget + ")";
            }
            else if (radioButton2.Checked)
            {
                semivariogramType = (string)comboBox4.SelectedItem;
                semiVariogramprops = "KrigingModelUniversal(" + semivariogramType + "," + lagsize + "," + majorrange + "," + partialsill + "," + nugget + ")";
            }

            object cellSize = textBox2.Text;//输出像元大小

            string searchRadius = "";//搜索半径
            if (radioButton3.Checked)
            {
                searchRadius = "RadiusFixed(" + textBox3.Text + "," + textBox4.Text + ")";
            }
            else if (radioButton4.Checked)
            {
                searchRadius = "RadiusVariable(" + textBox5.Text + "," + textBox6.Text + ")";
            }

            string outvarianceprediction_raster = textBox7.Text;//输出预测栅格数据的方差
            #endregion
            #region 设置GP工具
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置Kriging参数
            Kriging kriging = new Kriging();
            kriging.in_point_features = layer;
            kriging.z_field = Zfield;
            kriging.out_surface_raster = outRaster;
            kriging.semiVariogram_props = semiVariogramprops;
            kriging.cell_size = cellSize;
            kriging.search_radius = searchRadius;
           // kriging.out_variance_prediction_raster = outvarianceprediction_raster;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(kriging, null);
            #endregion
            #endregion
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
