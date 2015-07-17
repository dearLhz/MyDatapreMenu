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
    /// 重采样
    /// </summary>
    class cResample: MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        Resample1 resample;

        public cResample()
        {
            string str = @"..\Data\Image\DatapreMenu\resample.png";
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
            get { return "重采样"; }
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
            get { return "重采样"; }
        }

        public string Name
        {
            get { return "Resample"; }
        }

        public void OnClick()
        {
            resample.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                resample = new Resample1();
                resample.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "重采样"; }
        }
        #endregion
    }
}
