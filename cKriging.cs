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
    /// 克里金插值
    /// </summary>
    class cKriging : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        Kriging1 kriging;

        public cKriging()
        {
            string str = @"..\Data\Image\DatapreMenu\kriging.png";
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
            get { return "克里金插值"; }
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
            get { return "克里金插值"; }
        }

        public string Name
        {
            get { return "Kriging"; }
        }

        public void OnClick()
        {
            kriging.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                kriging = new Kriging1();
                kriging.pMap = hk.MapControl.Map;
                kriging.Visible = false;       
            }
        }

        public string Tooltip
        {
            get { return "克里金插值"; }
        }
        #endregion
    }
}
