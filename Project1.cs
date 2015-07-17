using DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
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
    public partial class Project1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;

        #region 禁止最大化窗体
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")] //导入API函数
        extern static System.IntPtr GetSystemMenu(System.IntPtr hWnd, System.IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);
        static int MF_BYPOSITION = 0x400;
        static int MF_REMOVE = 0x1000;
        #endregion

        public Project1()
        {
            InitializeComponent();
            RemoveMenu(GetSystemMenu(Handle, IntPtr.Zero), 0, MF_BYPOSITION | MF_REMOVE);
        }

        private void Project1_Load(object sender, EventArgs e)
        {
            //清空选项
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Items.Add("");
            comboBox2.Text = "";
            comboBox3.Items.Clear();
            comboBox3.Items.Add("");
            comboBox3.Text = "";
            comboBox4.Items.Clear();
            comboBox4.Items.Add("");
            comboBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";

            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;

        }

        #region 得到栅格图层
        private IRasterLayer GetRasterLayer(string layerName)
        {
            //从地图上得到图层
            IEnumLayer layers = GetRasterLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IRasterLayer;
            }

            return null;
        }

        private IEnumLayer GetRasterLayers()
        {
            UID uid = new UIDClass();
            uid.Value = "{D02371C7-35F7-11D2-B1F2-00C04F8EDEFF}";//得到栅格图层
            IEnumLayer layers = pMap.get_Layers(uid, true);

            return layers;
        }
        #endregion
        #region 得到矢量图层
        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            //从地图上得到图层
            IEnumLayer layers = GetFeatureLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IFeatureLayer;
            }

            return null;
        }

        private IEnumLayer GetFeatureLayers()
        {
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";//得到矢量图层
            IEnumLayer layers = pMap.get_Layers(uid, true);

            return layers;
        }
        #endregion

        #region 栅格投影转换
        private void comboBox1_Click(object sender, EventArgs e)
        {
            //加载主窗体已经打开的所有图层名称
            comboBox1.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                comboBox1.Items.Add(pMap.get_Layer(i).Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "TIF tif|*.tif|All files (*.*)|*.*";
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

        private void comboBox2_Click(object sender, EventArgs e)
        {
            //加载主窗体已经打开的所有图层名称
            comboBox2.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                comboBox2.Items.Add(pMap.get_Layer(i).Name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            IRasterLayer layer = GetRasterLayer((string)comboBox1.SelectedItem);
            string out_raster = textBox1.Text;
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置栅格投影转换参数
            ProjectRaster PR = new ProjectRaster();
            PR.in_raster = layer;
            PR.out_raster = out_raster;
            
            IEnvelope tempRaster = GetRasterLayer((string)comboBox2.SelectedItem).VisibleExtent;
            PR.out_coor_system = tempRaster.SpatialReference;
            
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(PR, null);
            #endregion
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
        #endregion
        #region 矢量投影转换

        private void comboBox3_Click(object sender, EventArgs e)
        {
            //加载主窗体已经打开的所有图层名称
            comboBox3.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                comboBox3.Items.Add(pMap.get_Layer(i).Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "SHP shp|*.shp|All files (*.*)|*.*";
            saveFileDialog2.RestoreDirectory = true;

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string pFilePath, pFileName;
                int index = this.saveFileDialog2.FileName.LastIndexOf("\\");
                pFilePath = this.saveFileDialog2.FileName.Substring(0, index);
                pFileName = this.saveFileDialog2.FileName.Substring(index + 1);

                textBox2.Text = pFilePath + "\\" + pFileName;
            }
        }

        private void comboBox4_Click(object sender, EventArgs e)
        {
            //加载主窗体已经打开的所有图层名称
            comboBox4.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                comboBox4.Items.Add(pMap.get_Layer(i).Name);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            IFeatureLayer layer = GetFeatureLayer((string)comboBox3.SelectedItem);
            string out_feature = textBox2.Text;
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置矢量投影转换参数
            Project pro = new Project();
            pro.in_dataset = layer;
            pro.out_dataset = out_feature;
            IEnvelope tempfeature = GetFeatureLayer((string)comboBox4.SelectedItem).AreaOfInterest;
            pro.out_coor_system = tempfeature.SpatialReference;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(pro, null);
            #endregion
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
            b.Show(button5, false);
            #endregion
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        


    }
}
