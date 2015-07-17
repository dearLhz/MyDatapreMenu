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
    /// 样条插值法
    /// </summary>
    class cSpline : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        Spline1 spline;

        public cSpline()
        {
            string str = @"..\Data\Image\DatapreMenu\spline.png";
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
            get { return "样条插值法"; }
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
            get { return "样条插值法"; }
        }

        public string Name
        {
            get { return "Spline"; }
        }

        public void OnClick()
        {
            spline.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                spline = new Spline1();
                spline.pMap = hk.MapControl.Map;
                spline.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "样条插值法"; }
        }
        #endregion
    }
}
