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
    /// 栅格计算器
    /// </summary>
    class cRasterCalculator : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        RasterCalculator1 rastercalculator;

        public cRasterCalculator()
        {
            string str = @"..\Data\Image\DatapreMenu\rastercal.ico";
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
            get { return "栅格计算器"; }
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
            get { return "栅格计算器"; }
        }

        public string Name
        {
            get { return "RasterCalculator"; }
        }

        public void OnClick()
        {
            rastercalculator.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                rastercalculator = new RasterCalculator1();
                rastercalculator.pMap = hk.MapControl.Map;
                rastercalculator.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "栅格计算器"; }
        }
        #endregion
    }
}
