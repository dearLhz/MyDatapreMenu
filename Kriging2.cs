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
    public partial class Kriging2 : DevComponents.DotNetBar.Office2007RibbonForm
    {
        #region 定义四个变量
        object lagSize;
        public object LagSize
        {
            get { return lagSize; }
            set { lagSize = value; }
        }
        object majorRange;
        public object MajorRange
        {
            get { return majorRange; }
            set { majorRange = value; }
        }
        object partialSill;
        public object PartialSill
        {
            get { return partialSill; }
            set { partialSill = value; }
        }
        object nugget;
        public object Nugget
        {
            get { return nugget; }
            set { nugget = value; }
        }
        #endregion

        public Kriging2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lagSize = textBox1.Text;//步长大小
            majorRange = textBox2.Text;//主要范围
            partialSill = textBox3.Text;//偏基台值
            nugget = textBox4.Text;//块金值
            this.Close();
        }
    }
}
