using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MyPluginEngine;
using System.Drawing;
using ESRI.ArcGIS.Controls;

namespace MyDatapreMenu
{
    /// <summary>
    /// 矢量转栅格
    /// </summary>
    class cFeatureToRaster : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        FeatureToRaster1 F2Raster;

        public cFeatureToRaster()
        {
            string str = @"..\Data\Image\DatapreMenu\shp2rst.ico";
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
            get { return "矢量转栅格"; }
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
            get { return "矢量转栅格"; }
        }

        public string Name
        {
            get { return "FeatureToRaster"; }
        }

        public void OnClick()
        {
            F2Raster.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                F2Raster = new FeatureToRaster1();
                F2Raster.pMap = hk.MapControl.Map;
                F2Raster.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "矢量转栅格"; }
        }
        #endregion
    }
}
