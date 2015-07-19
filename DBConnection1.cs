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
    public partial class DBConnection1 : DevComponents.DotNetBar.OfficeForm
    {
        public DBConnection1()
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
        }
    }
}
