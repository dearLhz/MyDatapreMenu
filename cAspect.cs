using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyPluginEngine;

namespace MyDatapreMenu
{
    
    /// <summary>
    /// 坡向分析
    /// </summary>
    class cAspect:MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        Aspect aspect;


        public cAspect()
        {
            string str = @"..\Data\Image\DatapreMenu\Aspect.png";
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
            get { return "坡向分析"; }
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
            get { return "坡向分析"; }
        }

        public string Name
        {
            get { return "Aspect"; }
        }

        public void OnClick()
        {
            aspect.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                aspect = new Aspect(hook.MapControl.Map);
                aspect.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "坡向分析"; }
        }
        #endregion

    }
}
