using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.esriSystem;



namespace MyDatapreMenu
{
    public partial class Buffer : DevComponents.DotNetBar.OfficeForm
    {
        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr wnd,
                                              uint Msg,
                                              IntPtr wParam,
                                              IntPtr lParam);

        private IMap pMap = null;
        private const uint WM_VSCROLL = 0x0115;
        private const uint SB_BOTTOM = 7;
        public IEnumLayer layers;
        public Buffer(IMap _pMap)
        {
            InitializeComponent();

            //禁用Glass主题
            this.EnableGlass = false;
            //不显示最大化最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //去除图标
            this.ShowIcon = false;

            //load all the feature layers in the map to the layers combo
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            pMap = _pMap;
            layers = pMap.get_Layers(uid, true);
            layers.Reset();
            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                cboLayers.Items.Add(layer.Name);
            }
            //select the first layer
            if (cboLayers.Items.Count > 0)
                cboLayers.SelectedIndex = 0;

            string tempDir = System.IO.Path.GetTempPath();
            txtOutputPath.Text = System.IO.Path.Combine(tempDir, ((string)cboLayers.SelectedItem + "_buffer.shp"));

            //set the default units of the buffer
            int units = Convert.ToInt32(pMap.MapUnits);
            cboUnits.SelectedIndex = units;
        }

        private void btnOutputLayer_Click(object sender, EventArgs e)
        {
            //set the output layer
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = (string)cboLayers.SelectedItem + "_buffer.shp";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                txtOutputPath.Text = saveDlg.FileName;
        }

        private void btnBuffer_Click(object sender, EventArgs e)
        {
            //timer 控件可用
            timerPro.Enabled = true;
            //scroll the textbox to the bottom
            
            txtMessages.Text = "\r\n分析开始,这可能需要几分钟时间,请稍候..\r\n";
            txtMessages.Update();
            ScrollToBottom();

            //make sure that all parameters are okay
            double bufferDistance;
            //转换distance为double类型
            double.TryParse(txtBufferDistance.Text, out bufferDistance);
            if (0.0 == bufferDistance)
            {
                MessageBox.Show("Bad buffer distance!");
                return;
            }
            //判断输出路径是否合法
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtOutputPath.Text)) ||
              ".shp" != System.IO.Path.GetExtension(txtOutputPath.Text))
            {
                MessageBox.Show("Bad output filename!");
                return;
            }
            //判断图层个数
            if (pMap.LayerCount == 0)
                return;

            //get the layer from the map
            IFeatureLayer layer = GetFeatureLayer((string)cboLayers.SelectedItem);
            if (null == layer)
            {
                txtMessages.Text += "Layer " + (string)cboLayers.SelectedItem + "cannot be found!\r\n";
                return;
            }


            //get an instance of the geoprocessor
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            //create a new instance of a buffer tool
            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(layer, txtOutputPath.Text, Convert.ToString(bufferDistance) + " " + (string)cboUnits.SelectedItem);
            buffer.dissolve_option = "ALL";    //这个要设成ALL,否则相交部分不会融合
            //buffer.line_side = "FULL";       //默认是"FULL",最好不要改否则出错
            //buffer.line_end_type = "ROUND";  //默认是"ROUND",最好不要改否则出错

            //execute the buffer tool (very easy :-))
            IGeoProcessorResult results = null;

            try
            {
                results = (IGeoProcessorResult)gp.Execute(buffer, null);
            }
            catch
            {
                txtMessages.Text += "Failed to buffer layer: " + layer.Name + "\r\n";
            }
            if (results.Status != esriJobStatus.esriJobSucceeded)
            {
                txtMessages.Text += "Failed to buffer layer: " + layer.Name + "\r\n";
            }

            //scroll the textbox to the bottom
            ScrollToBottom();
            txtMessages.Text += "\r\n分析完成.\r\n";
            timerPro.Enabled = false;
            gp = null;

        }
        private void ScrollToBottom()
        {
            PostMessage((IntPtr)txtMessages.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)IntPtr.Zero);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            //get the layers from the maps
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            layers = pMap.get_Layers(uid, true);
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IFeatureLayer;
            }

            return null;
        }

        private void timerPro_Tick(object sender, EventArgs e)
        {
            ScrollToBottom();
            txtMessages.Text += " * ";
            txtMessages.Update();
        }
   
    }
}