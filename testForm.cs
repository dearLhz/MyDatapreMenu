using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace MyDatapreMenu
{
    public partial class testForm : DevComponents.DotNetBar.OfficeForm
    {
        public testForm()
        {
            InitializeComponent();
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
    }
}