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
    public partial class Kriging2 : DevComponents.DotNetBar.OfficeForm
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
            //禁用Glass主题
            this.EnableGlass = false;
            //不显示最大化最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //去除图标
            this.ShowIcon = false;
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
