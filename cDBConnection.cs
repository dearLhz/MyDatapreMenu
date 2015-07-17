using MyPluginEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyDatapreMenu
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    class cDBConnection : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        DBConnection1 dbconnection;

        public cDBConnection()
        {
            string str = @"..\Data\Image\DatapreMenu\dbconnection.ico";
            if (System.IO.File.Exists(str))
                m_hBitmap = new Bitmap(str);
            else
                m_hBitmap = null;
        }

        #region ICommand 成员
        public System.Drawing.Bitmap Bitmap
        {
            get { return m_hBitmap; }
        }

        public string Caption
        {
            get { return "数据库连接"; }
        }

        public string Category
        {
            get { return "DatapreMenu"; }
        }

        public bool Checked
        {
            get { return false; }
        }

        public bool Enabled
        {
            get { return true; }
        }

        public int HelpContextId
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public string Message
        {
            get { return "数据库连接"; }
        }

        public string Name
        {
            get { return "DBConnection"; }
        }

        public void OnClick()
        {
            dbconnection.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                dbconnection = new DBConnection1();
                dbconnection.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "数据库连接"; }
        }
        #endregion
    }
}
