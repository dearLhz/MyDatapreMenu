using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataManagementTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geoprocessing;
using DevComponents.DotNetBar;
using System.Runtime.InteropServices;

namespace MyDatapreMenu
{
    public partial class Resample1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        public int layerIndex;
        private IFeatureLayer pLayer;
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

        public Resample1()
        {
            InitializeComponent();
            RemoveMenu(GetSystemMenu(Handle, IntPtr.Zero), 0, MF_BYPOSITION | MF_REMOVE);
        }

        private void Resample1_Load(object sender, EventArgs e)
        {

            //清空选项
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            radioButton1.Checked = true;
            textBox3.ReadOnly = true;
            

            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TIFF tif|*.tif|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pFilePath, pFileName;
                int index = this.openFileDialog1.FileName.LastIndexOf("\\");
                pFilePath = this.openFileDialog1.FileName.Substring(0, index);
                pFileName = this.openFileDialog1.FileName.Substring(index + 1);

                textBox4.Text = pFilePath + "\\" + pFileName;
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

        private void radioButton1_Click(object sender, EventArgs e)
        {
            textBox3.ReadOnly = true;
            textBox3.Text = "";
            textBox2.ReadOnly = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
            textBox2.Text = "";
            textBox3.ReadOnly = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ToggleEndlessProgress.Execute();
            circularProgress1.IsRunning = true;
            #region GP工具的使用
            //得到参数
            string inRaster = textBox4.Text;//输入栅格
            string outRaster = textBox1.Text;//输出栅格
            string cellSize;//输出栅格的像元大小
            if (radioButton1.Checked)
                cellSize = textBox2.Text;
            else
                cellSize = textBox3.Text;
            string resamplingType = (string)comboBox2.SelectedItem;//重采样技术

            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置重采样参数        
            Resample resample = new Resample();
            resample.in_raster = inRaster;
            resample.out_raster = outRaster;
            resample.cell_size = cellSize;
            resample.resampling_type = resamplingType;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(resample, null);
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
