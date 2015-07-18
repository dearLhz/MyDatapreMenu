using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace MyDatapreMenu
{
    public partial class RasterReClass : DevComponents.DotNetBar.OfficeForm
    {
        private IMap pMap = null;

        private static int iReClassCount = 5;
        private IRasterLayer pRLayer = null;

        private string strOutDir="";
        public RasterReClass(IMap _pMap)
        {
            InitializeComponent();
            pMap = _pMap;
            //����Glass����
            this.EnableGlass = false;
            //����ʾ�����С����ť
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //ȥ��ͼ��
            this.ShowIcon = false;
        }

        private void frmRasterReClass_Load(object sender, EventArgs e)
        {
            try
            {
                //��ʼ��pMap����
                strOutDir=Application.StartupPath.Replace("\\bin\\Debug", "\\Data\\data");
                this.txtPathSave.Text = strOutDir;
                //���դ��ͼ�����Ƶ�cmbLayer
                if (pMap != null)
                {
                    for (int i = 0; i < pMap.LayerCount; i++)
                    {
                        if (pMap.get_Layer(i) is IRasterLayer)
                            this.cmbLayer.Items.Add(pMap.get_Layer(i).Name);
                    }
                    if (this.cmbLayer.Items.Count > 0)
                        this.cmbLayer.SelectedIndex = 0;
                }
                //��ʼ��lsvView
                this.lsvValue.GridLines = true;     //��ʾ������¼�ķָ���
                lsvValue.View = View.Details;       //�����б���ʾ�ķ�ʽ
                lsvValue.Scrollable = true;         //��Ҫʱ����ʾ������
                lsvValue.MultiSelect = false;             // �����Զ���ѡ��
                lsvValue.HeaderStyle = ColumnHeaderStyle.Nonclickable;// ��ִ�в���
                lsvValue.Columns.Add("ԭֵ", 100, HorizontalAlignment.Center);
                lsvValue.Columns.Add("��ֵ", 100, HorizontalAlignment.Center);
                lsvValue.LabelEdit = true;
                //�����и�
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 20);
                lsvValue.SmallImageList = imgList;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private IRasterLayer SetViewShedRenderer(IRaster pInRaster, string sField, string sPath,double[,] dValue)
        {
            try
            {
                if (dValue == null)
                    return null;
                if (sField.Trim() == "")
                    return null;
                string path="";
                string fileName="";
                int iPath=sPath.LastIndexOf('\\');
                path=sPath.Substring(0,iPath);
                fileName=sPath.Substring(iPath+1,sPath.Length-iPath-1);

                IRasterDescriptor pRD = new RasterDescriptorClass();
                pRD.Create(pInRaster, null, sField);
                IReclassOp pReclassOp = new RasterReclassOpClass();
                
                IGeoDataset pGeodataset = pInRaster as IGeoDataset;
                IRasterAnalysisEnvironment pEnv = pReclassOp as IRasterAnalysisEnvironment;
                IWorkspaceFactory pWSF = new RasterWorkspaceFactoryClass();
                IWorkspace pWS = pWSF.OpenFromFile(path, 0);
                //pEnv.OutWorkspace = pWS;
                //object objSnap = null;
                //object objExtent = pGeodataset.Extent;
                //pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref objExtent, ref objSnap);
                //pEnv.OutSpatialReference = pGeodataset.SpatialReference;

                IRasterBandCollection pRsBandCol = pGeodataset as IRasterBandCollection;
                IRasterBand pRasterBand = pRsBandCol.Item(0);
                pRasterBand.ComputeStatsAndHist();
                IRasterStatistics pRasterStatistic = pRasterBand.Statistics;
                double dMaxValue = pRasterStatistic.Maximum;
                double dMinValue = pRasterStatistic.Minimum;

                INumberRemap pNumRemap = new NumberRemapClass();
                for (int i = 0; i < (dValue.Length/3); i++)
                {
                    pNumRemap.MapRange(dValue[i, 0], dValue[i, 1], (Int32)dValue[i, 2]);
                }
                IRemap pRemap = pNumRemap as IRemap;

                IGeoDataset pGeoDs = new RasterDatasetClass();
                pGeoDs=pReclassOp.ReclassByRemap(pGeodataset, pRemap, false);
                IRasterBandCollection pRasBandCol = (IRasterBandCollection)pGeoDs;
                pRasBandCol.SaveAs(fileName, pWS, "GRID");//"IMAGINE Image"
                IRaster pOutRaster =pGeoDs as IRaster;
                IRasterLayer pRLayer = new RasterLayerClass();
                
                pRLayer.CreateFromRaster(pOutRaster);
                pRLayer.Name = fileName;
                return pRLayer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
        //ȷ����ť
        private void btnOK_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(lsvValue.Items.Count.ToString());
            try
            {
                if (pRLayer == null)
                    return;
                pMap.AddLayer(SetViewShedRenderer(pRLayer.Raster, this.cmbFieldsName.Text, this.txtPathSave.Text, getReClassValue()) as ILayer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //ȡ����ť
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //���������Ŀ��ť
        private void btnReClassify_Click(object sender, EventArgs e)
        {

        }
        //Ψһֵ���ఴť
        private void btnUnique_Click(object sender, EventArgs e)
        {

        }
        //��ӷ��ఴť
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem li = new ListViewItem();
            li.SubItems.Clear();
            li.SubItems[0].Text = "";
            li.SubItems.Add("");
            this.lsvValue.Items.Add(li);
            iReClassCount++;
        }
        //ɾ�����ఴť
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lsvValue.SelectedItems.Count > 0)
            {
                iReClassCount--;
                this.lsvValue.Items.RemoveAt(this.lsvValue.SelectedItems[0].Index);
            }
        }
        //�����ť
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "դ������|*.*";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "����դ���ļ���...";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = "reclass1";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                this.txtPathSave.Text = saveDlg.FileName;
        }

        private void lsvValue_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        //ͼ��ѡ��� ѡ��ͬ��ͼ��
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                string name = pMap.get_Layer(i).Name;
                if (pMap.get_Layer(i).Name == this.cmbLayer.Text)
                {
                    pRLayer = pMap.get_Layer(i) as IRasterLayer;
                    setRasterLayerFiledName(pRLayer, this.cmbFieldsName);
                    break;
                }
            }
        }

        /// <summary>
        /// ����ֶ��б�ָ����Combobox
        /// </summary>
        /// <param name="pRlyr">դ��ͼ��</param>
        /// <param name="cmb">Ҫ��ʾ�ֶε�Combobox</param>
        private void setRasterLayerFiledName(IRasterLayer pRlyr, ComboBox cmb)
        {
            try
            {
                //����ֶ��б�
                cmb.Items.Clear();

                IRaster pRaster = pRlyr.Raster;
                IRasterProps pProp = pRaster as IRasterProps;
                pProp.PixelType = rstPixelType.PT_LONG;
                if (pProp.PixelType == rstPixelType.PT_LONG)
                {
                    IRasterBandCollection pBcol = pRaster as IRasterBandCollection;
                    IRasterBand pBand = pBcol.Item(0);
                    ITable pRTable = pBand.AttributeTable;

                    string strFieldName = "";
                    for (int i = 0; i < pRTable.Fields.FieldCount; i++)
                    {
                        strFieldName = pRTable.Fields.get_Field(i).Name;
                        if (strFieldName.ToUpper() != "ROWID" && strFieldName.ToUpper() != "COUNT")
                            cmb.Items.Add(strFieldName);
                    }
                    if (cmbFieldsName.Items.Count > 0)
                        cmbFieldsName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// ������ʾ��ԭֵ--��ֵ
        /// </summary>
        /// <param name="pInRaster"></param>
        /// <param name="sField"></param>
        /// <returns></returns>
        public void setViewValue(IRaster pInRaster, string sField)
        {
            try
            {
                IRasterDescriptor pRD = new RasterDescriptorClass();
                pRD.Create(pInRaster, new QueryFilterClass(), sField);
                IGeoDataset pGeodataset = pInRaster as IGeoDataset;
                IRasterLayer pRLayer = new RasterLayerClass();
                IRasterBandCollection pRsBandCol = pGeodataset as IRasterBandCollection;
                IRasterBand pRasterBand = pRsBandCol.Item(0);
                pRasterBand.ComputeStatsAndHist();
                IRasterStatistics pRasterStatistic = pRasterBand.Statistics;

                double dMaxValue = pRasterStatistic.Maximum;
                double dMinValue = pRasterStatistic.Minimum;

                if ((dMaxValue - dMinValue) / iReClassCount >= 1)
                {
                    double dItemInterval = (dMaxValue - dMinValue) / iReClassCount;
                    double dMinTemp = dMinValue;
                    if (this.cmbFieldsName.Items.Count > 0)
                        this.lsvValue.Items.Clear();
                    for (int i = 0; i < iReClassCount; i++)
                    {
                        ListViewItem li = new ListViewItem();
                        li.SubItems.Clear();
                        li.SubItems[0].Text = dMinTemp.ToString() + " - " + (dMinTemp + dItemInterval).ToString("N4");
                        li.SubItems.Add(i.ToString());
                        this.lsvValue.Items.Add(li);
                        dMinTemp = dMinTemp + dItemInterval;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //ѡ��ͬ���ֶ�ʱ ��Ӧ�ĸı�lstview��ʾ��ֵ
        private void cmbFieldsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pRLayer == null)
                return;
            setViewValue(pRLayer.Raster, this.cmbFieldsName.Text);
        }

        /// <summary>
        /// ���ء���Сֵ�����ֵ����ֵ��������,�쳣����null
        /// </summary>
        /// <returns>���ء���Сֵ�����ֵ����ֵ�������飨ireclass-1,3��</returns>
        private double[,] getReClassValue()
        {
            try
            {
                int n = this.lsvValue.Items.Count;
                iReClassCount = n;
                ListViewItem lvt = null;
                for (int i = n-1; i >=0; i--)
                {
                    lvt = this.lsvValue.Items[i];
                    if (lvt.SubItems[0].Text.Trim() == "" && lvt.SubItems[1].Text.Trim() == "")
                    {
                        iReClassCount--;
                    }
                    else if (lvt.SubItems[0].Text.Trim() == "" || lvt.SubItems[1].Text.Trim() == "")
                    {
                        MessageBox.Show("��" + i.ToString() + "�����������������ɾ����");
                        return null;
                    }
                }

                double[,] dValue = new double[iReClassCount, 3];
                //ע�����һ����nodata(������)
                for (int i = 0; i < iReClassCount; i++)
                {
                    string[] temp = this.lsvValue.Items[i].SubItems[0].Text.Split('-');
                    dValue[i, 0] = double.Parse(temp[0].Trim());
                    dValue[i, 1] = double.Parse(temp[1].Trim());
                    dValue[i, 2] = double.Parse(this.lsvValue.Items[i].SubItems[1].Text.Trim());
                }
                return dValue;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }

        }
    }
}