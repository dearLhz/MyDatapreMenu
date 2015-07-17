using ESRI.ArcGIS.Controls;
using MyPluginEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyDatapreMenu
{
    /// <summary>
    /// 投影转换
    /// </summary>
    class cProject : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        Project1 prj;

        public cProject()
        {
            string str = @"..\Data\Image\DatapreMenu\project.ico";
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
            get { return "投影转换"; }
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
            get { return "投影转换"; }
        }

        public string Name
        {
            get { return "Project"; }
        }

        public void OnClick()
        {
            prj.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                prj = new Project1();
                prj.pMap = hk.MapControl.Map;
                prj.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "投影转换"; }
        }
        #endregion
    }
}
