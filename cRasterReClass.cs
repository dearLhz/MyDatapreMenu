using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyPluginEngine;

namespace MyDatapreMenu
{
    /// <summary>
    /// 影像重分类
    /// </summary>
    class cRasterReClass: MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        RasterReClass rasterReClass1;

        public cRasterReClass()
        {
            string str = @"..\Data\Image\DatapreMenu\ReClass.png";
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
            get { return "影像重分类"; }
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
            get { return "影像重分类"; }
        }

        public string Name
        {
            get { return "ReClass"; }
        }

        public void OnClick()
        {
            rasterReClass1.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                rasterReClass1 = new RasterReClass(null);
                rasterReClass1.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "影像重分类"; }
        }
        #endregion
    }
}
