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
    /// 栅格转矢量
    /// </summary>
    class cRasterToPolygon : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        RasterToPolygon1 rastertopolygon;

        public cRasterToPolygon()
        {
            string str = @"..\Data\Image\DatapreMenu\rst2shp.ico";
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
            get { return "栅格转矢量"; }
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
            get { return "栅格转矢量"; }
        }

        public string Name
        {
            get { return "RasterToPolygon"; }
        }

        public void OnClick()
        {
            rastertopolygon.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                rastertopolygon = new RasterToPolygon1();
                rastertopolygon.pMap = hk.MapControl.Map;
                rastertopolygon.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "栅格转矢量"; }
        }
        #endregion
    }
}
