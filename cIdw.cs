using ESRI.ArcGIS.Controls;
using MyPluginEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/**1.27  IDW功能基本完成，没有测试数据运行 
 * 遗留问题：将IDW运行结果同步到主程序TOC控件上没有写。
 * LILEI
 * **/
namespace MyDatapreMenu
{
    /// <summary>
    /// 反距离加权
    /// </summary>
    class cIdw : MyPluginEngine.ICommand
    {private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        IDW1 idw;
        
        public cIdw()
        {
            string str = @"..\Data\Image\DatapreMenu\idw.png";
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
            get { return "反距离加权"; }
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
            get { return "反距离加权"; }
        }

        public string Name
        {
            get { return "Idw"; }
        }
        
        public void OnClick()
        {
            idw.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                idw = new IDW1();
                idw.pMap = hk.MapControl.Map;
                idw.Visible = false;           


            }
        }

        public string Tooltip
        {
            get { return "反距离加权"; }
        }
        #endregion
    
    }
}
