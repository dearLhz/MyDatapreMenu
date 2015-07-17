using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDatapreMenu
{
    class DatapreMenuBar : MyPluginEngine.IMenuDef
    {
        #region IMenuDef 成员
        public string Caption
        {
            get { return "数据预处理"; }
        }

        public string Name
        {
            get { return "DatapreMenu"; }
        }

        public long ItemCount
        {
            get { return 9; }
        }

        public void GetItemInfo(int pos, MyPluginEngine.ItemDef itemDef)
        {
            switch (pos)
            {
                case 0:
                    itemDef.ID = "MyDatapreMenu.cFeatureToRaster";
                    itemDef.Group = false;
                    break;
                case 1:
                    itemDef.ID = "MyDatapreMenu.cRasterToPolygon";
                    itemDef.Group = false;
                    break;
                case 2:
                    itemDef.ID = "MyDatapreMenu.cProject";
                    itemDef.Group = false;
                    break;
                case 3:
                    itemDef.ID = "MyDatapreMenu.cResample";
                    itemDef.Group = false;
                    break;
                case 4:
                    itemDef.ID = "MyDatapreMenu.cKriging";
                    itemDef.Group = false;
                    break;
                case 5:
                    itemDef.ID = "MyDatapreMenu.cSpline";
                    itemDef.Group = false;
                    break;
                case 6:
                    itemDef.ID = "MyDatapreMenu.cIdw";
                    itemDef.Group = false;
                    break;
                case 7:
                    itemDef.ID = "MyDatapreMenu.cRasterCalculator";
                    itemDef.Group = false;
                    break;
                case 8:
                    itemDef.ID = "MyDatapreMenu.cDBConnection";
                    itemDef.Group = true;
                    break;
                default:
                    break;

            }
        }
        #endregion
    }
}
