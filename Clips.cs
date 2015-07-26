using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;



namespace MyDatapreMenu
{
    public partial class Clips : DevComponents.DotNetBar.OfficeForm
    {
        private IMap pMap;
        public Clips(IMap _pMap)
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

            this.pMap = _pMap;
        }
        //确定  按钮
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {


                IFeatureClass featureclassInput = null;
                IFeatureClass featureclassClip = null;

                IFields outfields = null;

                // Create a new ShapefileWorkspaceFactory CoClass to create a new workspace
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();

                // System.IO.Path.GetDirectoryName(shapefileLocation) returns the directory part of the string. Example: "C:\test\"
                // System.IO.Path.GetFileNameWithoutExtension(shapefileLocation) returns the base filename (without extension). Example: "cities"
                //IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(shapefileLocation));

                IFeatureWorkspace featureWorkspaceInput = (IFeatureWorkspace)workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(TxtInputFile.Text), 0); // Explicit Cast
                IFeatureWorkspace featureWorkspaceOut= (IFeatureWorkspace)workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(txtOutputPath.Text), 0);
                IFeatureWorkspace featureWorkspaceClip = (IFeatureWorkspace)workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(txtClipsFile.Text), 0);
                
                //timer 控件可用
                timerPro.Enabled = true;
                //scroll the textbox to the bottom

                txtMessages.Text = "\r\n分析开始,这可能需要几分钟时间,请稍候..\r\n";
                txtMessages.Update();

                //inputfeatureclass = featureWorkspace.OpenFeatureClass("land00_ws");
                //clipfeatureclass = featureWorkspace.OpenFeatureClass("drain_ws_buffer");

                featureclassInput = featureWorkspaceInput.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(TxtInputFile.Text));
                featureclassClip = featureWorkspaceClip.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(txtClipsFile.Text));
                outfields = featureclassInput.Fields;

                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                //IFeatureClass outfeatureclass = featureWorkspace.CreateFeatureClass("Clip_result", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                IFeatureClass featureclassOut = null;
               
                //文件存在的处理
                if (File.Exists(txtOutputPath.Text.Trim()) == true)
                {
                    featureclassOut = featureWorkspaceOut.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(txtOutputPath.Text));
                    DelFeatureFile(featureclassOut, txtOutputPath.Text.Trim());
                }

                featureclassOut = featureWorkspaceOut.CreateFeatureClass(System.IO.Path.GetFileNameWithoutExtension(txtOutputPath.Text), outfields,null,null,esriFeatureType.esriFTSimple, "Shape", "");

                ESRI.ArcGIS.AnalysisTools.Clip clipTool =
                    new ESRI.ArcGIS.AnalysisTools.Clip(featureclassInput, featureclassClip, featureclassOut);
                
                gp.Execute(clipTool,null);
                workspaceFactory = null;

                //复制feature层
                //IDataset pDataset = outfeatureclass as IDataset;
                //pDataset.Copy("Clip_result1", featureWorkspace as IWorkspace);   

                //添加图层到当前地图
                IFeatureLayer outlayer = new FeatureLayerClass();
                outlayer.FeatureClass = featureclassOut;
                outlayer.Name = featureclassOut.AliasName;
                pMap.AddLayer((ILayer)outlayer);
                //
                txtMessages.Text += "\r\n分析完成.\r\n";
                timerPro.Enabled = false;
            }
            catch(Exception ex)
            {
                //
                txtMessages.Text += "\r\n分析失败.\r\n";
                timerPro.Enabled = false;
                MessageBox.Show(ex.Message.ToString());
            }

        }
        //输出图层要素   打开  按钮
        private void button3_Click(object sender, EventArgs e)
        {
            //set the output layer
           
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = "clips.shp";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                txtOutputPath.Text = saveDlg.FileName;
        }
        //输入图层要素   打开  按钮
        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.CheckPathExists = true;
            openDlg.Filter = "Shapefile (*.shp)|*.shp";
            openDlg.Title = "open input Layer";
            openDlg.RestoreDirectory = true;

            DialogResult dr = openDlg.ShowDialog();
            if (dr == DialogResult.OK)
                TxtInputFile.Text = openDlg.FileName;
        }
        //裁剪图层要素   打开  按钮
        private void button2_Click(object sender, EventArgs e)
        {
            // Use the OpenFileDialog Class to choose which shapefile to load.
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Shapefiles (*.shp)|*.shp";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;


            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // The user chose a particular shapefile.

                // The returned string will be the full path, filename and file-extension for the chosen shapefile. Example: "C:\test\cities.shp"
                string shapefileLocation = openFileDialog.FileName;
                txtClipsFile.Text = shapefileLocation;

                if (shapefileLocation != "")
                {
                    // Ensure the user chooses a shapefile

                    // Create a new ShapefileWorkspaceFactory CoClass to create a new workspace
                    ESRI.ArcGIS.Geodatabase.IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();

                    // System.IO.Path.GetDirectoryName(shapefileLocation) returns the directory part of the string. Example: "C:\test\"
                    ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(shapefileLocation), 0); // Explicit Cast

                    // System.IO.Path.GetFileNameWithoutExtension(shapefileLocation) returns the base filename (without extension). Example: "cities"
                    ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(shapefileLocation));

                    ESRI.ArcGIS.Carto.IFeatureLayer featureLayer = new ESRI.ArcGIS.Carto.FeatureLayerClass();
                    featureLayer.FeatureClass = featureClass;
                    featureLayer.Name = featureClass.AliasName;
                    featureLayer.Visible = true;
                    //activeView.FocusMap.AddLayer(featureLayer);

                    // Zoom the display to the full extent of all layers in the map
                    //activeView.Extent = activeView.FullExtent;
                   // activeView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, null, null);
                }
                else
                {
                    // The user did not choose a shapefile.
                    // Do whatever remedial actions as necessary
                    System.Windows.Forms.MessageBox.Show("No shapefile chosen", "No Choice #1",
                                                        System.Windows.Forms.MessageBoxButtons.OK,
                                                        System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                // The user did not choose a shapefile. They clicked Cancel or closed the dialog by the "X" button.
                // Do whatever remedial actions as necessary.
                System.Windows.Forms.MessageBox.Show("No shapefile chosen", "No Choice #2",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Exclamation);
            }

        }

        //保存一个FeatureClass为文件 by gisoracle   
        //public void saveFeatureClass(IFeatureClass pFeatureClass, string fileName)
        //{
        //    try
        //    {
        //        string sFileName = System.IO.Path.GetFileName(fileName);
        //        string sFilePath = System.IO.Path.GetDirectoryName(fileName);

        //        IDataset pDataset = pFeatureClass as IDataset;

        //        IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
        //        IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(sFilePath, 0) as IFeatureWorkspace;

        //        IWorkspace pWorkspace = pFeatureWorkspace as IWorkspace;
        //        if (pWorkspace.Exists() == true)
        //        {
        //            //DelFeatureFile(sFilePath, sFileName);
        //        }
        //        //pDataset.Copy(sFileName, pFeatureWorkspace as IWorkspace);

        //    }
        //    catch { MessageBox.Show("错误"); }
        //} 
 
        //by gisoracle   
        public static void DelFeatureFile(IFeatureClass pFeatureClass, string sName)   
        {   
            if (pFeatureClass!= null)   
            {   
                IDataset dataset = pFeatureClass as IDataset;   
                dataset.Delete();   
            }   
  
        }
        //获得Shape文件 by gisoracle   
        public static IFeatureClass GetFeatureClassByFileName(string fileName)
        {
           /* IWorkspace shapeWorkspace = GetWorkSpaceByPath(fileName);
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)shapeWorkspace;


            string extfileName = System.IO.Path.GetFileNameWithoutExtension(fileName); //文件名   
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(extfileName);

            return featureClass;*/
            return null;

        }
        //取消  按钮
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmClips_Load(object sender, EventArgs e)
        {

        }

        private void timerPro_Tick(object sender, EventArgs e)
        {
            txtMessages.Text += " * ";
            txtMessages.Update();
        }
    }
}