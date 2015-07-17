using ESRI.ArcGIS.Carto;
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
    public partial class RasterCalculator1 : DevComponents.DotNetBar.Office2007Form
    {
        public IMap pMap;
        string s;//表达式参数

        public RasterCalculator1()
        {
            InitializeComponent();
        }

        #region 表达式按钮设置
        private void RasterCalculator1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            int i, layerCount;
            layerCount = pMap.LayerCount;
            for (i = 0; i < layerCount; i++)
                listBox1.Items.Add(pMap.get_Layer(i).Name);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += "\"" + listBox1.SelectedItem + "\"";
            s += "\"" + listBox1.SelectedItem + "\"";


        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += listBox2.SelectedItem;
            s += listBox2.SelectedItem;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "0";
            s += "0";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += ".";
            s += ".";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "1";
            s += "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "2";
            s += "2";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "3";
            s += "3";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "4";
            s += "4";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "5";
            s += "5";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "6";
            s += "6";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "7";
            s += "7";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "8";
            s += "8";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "9";
            s += "9";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "+" + " ";
            s += " " + "+" + " ";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "-" + " ";
            s += " " + "-" + " ";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "*" + " ";
            s += " " + "*" + " ";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "/" + " ";
            s += " " + "/" + " ";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "==" + " ";
            s += " " + "==" + " ";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + ">" + " ";
            s += " " + ">" + " ";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "<" + " ";
            s += " " + "<" + " ";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "(";
            s += "(";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += ")";
            s += ")";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "<=" + " ";
            s += " " + "<=" + " ";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + ">=" + " ";
            s += " " + ">=" + " ";
        }

        private void button23_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "!=" + " ";
            s += " " + "!=" + " ";
        }

        private void button27_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "&" + " ";
            s += " " + "&" + " ";
        }

        private void button26_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "|" + " ";
            s += " " + "|" + " ";
        }

        private void button25_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "^" + " ";
            s += " " + "^" + " ";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + "~";
            s += " " + "~";
        }
        #endregion

        private void button28_Click(object sender, EventArgs e)
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

        private void button29_Click(object sender, EventArgs e)
        {
            #region GP工具的使用
            //得到参数
            object expression = s;
            object outputRaster = textBox1.Text;
            //实例化GP工具
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置RasterCalculator参数
            RasterCalculator rastercalculator = new RasterCalculator();
            rastercalculator.expression = expression;
            rastercalculator.output_raster = outputRaster;
            //执行GP工具
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(rastercalculator, null);
            #endregion
        }

        private void button30_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
