using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;

using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;

namespace MyDatapreMenu
{
    public partial class Aspect : DevComponents.DotNetBar.OfficeForm
    {
        private IMap pMap = null;
        private bool bDataPath = false;
        private IRasterLayer m_pRasterLyr = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ppAxMapControl">地图控件</param>
        public Aspect(IMap _pMap)
        {
            pMap = _pMap;
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
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = this.txtOutPath.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.comboBoxInData.Text = openFileDialog1.FileName;
                string filePath = openFileDialog1.FileName;
                m_pRasterLyr = new RasterLayerClass();
                m_pRasterLyr.CreateFromFilePath(filePath);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = this.txtOutPath.Text;
            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOutPath.Text = saveFileDialog1.FileName;
            }
            bDataPath = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            string strInFileName;
            string strOutFileName;
            int startX, endX;
            string strOutDir;
            try
            {
                if (bDataPath == true)
                {
                    strInFileName = txtOutPath.Text;
                    strOutDir = strInFileName.Substring(0, strInFileName.LastIndexOf("\\"));
                    startX = strInFileName.LastIndexOf("\\");
                    endX = strInFileName.Length;
                    strOutFileName = strInFileName.Substring(startX + 1, endX - startX - 1);
                }
                else
                {
                    strOutDir = txtOutPath.Text;
                    strOutFileName = "aspect";
                }
                if (File.Exists(strOutDir + "\\" + strOutFileName + ".aux") == true)
                {
                    MessageBox.Show("文件" + strOutFileName + "已经存在，请重新命名！");
                    return;
                }
                if (m_pRasterLyr != null)
                {
                    double dCellSize = Convert.ToDouble(txtCellSize.Text);
                    ISurfaceOp pRasterSurfaceOp = new RasterSurfaceOpClass();
                    IRaster pInRaster = m_pRasterLyr.Raster;
                    IRaster pOutRaster = null;
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    IGeoDataset pGeoDs = pRasterSurfaceOp.Aspect(pInRaster as IGeoDataset);
                    pOutRaster = pGeoDs as IRaster;
                    IRasterLayer pOutRasterLayer = new RasterLayerClass();
                    pOutRasterLayer.CreateFromRaster(pOutRaster);
                    pOutRasterLayer.Name = strOutFileName;
                    IWorkspaceFactory pWSF = new RasterWorkspaceFactoryClass();
                    IWorkspace pRWS = pWSF.OpenFromFile(strOutDir, 0);
                    IRasterBandCollection pRasBandCol = (IRasterBandCollection)pGeoDs;
                    pRasBandCol.SaveAs(strOutFileName, pRWS, "GRID");//"IMAGINE Image"
                    pOutRasterLayer = UtilityFunction.SetStretchRenderer(pOutRasterLayer.Raster);
                    pOutRasterLayer.Name = strOutFileName;
                    pMap.AddLayer(pOutRasterLayer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void frmAspect_Load(object sender, EventArgs e)
        {
            this.txtOutPath.Text = Application.StartupPath.Replace("\\bin\\Debug", "\\Data\\data");
            PopulateComboWithMapLayers(comboBoxInData, true);
        }
        private void PopulateComboWithMapLayers(ComboBox Layers, bool bLayer)
        {
            Layers.Items.Clear();
            ILayer aLayer;
            for (int i = 0; i < pMap.LayerCount ; i++)
            {
                // Get the layer name and add to combo
                aLayer = pMap.get_Layer(i);
                if (aLayer.Valid == true)
                {
                    if (bLayer == true)
                    {
                        if (aLayer is IRasterLayer)
                        {
                            Layers.Items.Add(aLayer.Name);
                        }
                    }

                }
            }
            if (Layers.Items.Count > 0)
                Layers.SelectedIndex = 0;
        }

        private void comboBoxInData_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sLayerName = comboBoxInData.Text;
            try
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ILayer pLyr = pMap.get_Layer(i);
                    if (pLyr.Name == sLayerName)
                    {
                        if (pLyr is IRasterLayer)
                        {
                            m_pRasterLyr = pLyr as IRasterLayer;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

    }
}