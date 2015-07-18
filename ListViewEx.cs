using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MyDatapreMenu
{
    //msg=0x115   (WM_VSCROLL)     
    //msg=0x114   (WM_HSCROLL)   
    ///   <summary>   
    ///   CListView   的摘要说明。   
    ///   </summary>   
    class ListViewEx : ListView
    {
        private TextBox m_tb;

        public ListViewEx()
        {
            m_tb = new TextBox();
            m_tb.Multiline = true;
            m_tb.Visible = false;
            this.GridLines = true;
            this.FullRowSelect = true;
            this.Controls.Add(m_tb);
        }
        private void EditItem(ListViewItem.ListViewSubItem subItem)
        {
            if (this.SelectedItems.Count <= 0)
            {
                return;
            }

            Rectangle _rect = subItem.Bounds;
            m_tb.Bounds = _rect;
            m_tb.BringToFront();
            m_tb.Text = subItem.Text;
            m_tb.Leave += new EventHandler(tb_Leave);
            m_tb.TextChanged += new EventHandler(m_tb_TextChanged);
            m_tb.Visible = true;
            m_tb.Tag = subItem;
            m_tb.Select();
        }
        private void EditItem(ListViewItem.ListViewSubItem subItem, Rectangle rt)
        {
            if (this.SelectedItems.Count <= 0)
            {
                return;
            }

            Rectangle _rect = rt;
            m_tb.Bounds = _rect;
            m_tb.BringToFront();
            m_tb.Text = subItem.Text;
            m_tb.Leave += new EventHandler(tb_Leave);
            m_tb.TextChanged += new EventHandler(m_tb_TextChanged);
            m_tb.Visible = true;
            m_tb.Tag = subItem;
            m_tb.Select();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (this.SelectedItems.Count > 0)
                {
                    ListViewItem lvi = this.SelectedItems[0];
                    EditItem(lvi.SubItems[0]);
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.m_tb.Visible = false;
            base.OnSelectedIndexChanged(e);
        }
        protected override void OnClick(EventArgs e)
        //protected override void OnDoubleClick(EventArgs e)
        {
            Point tmpPoint = this.PointToClient(Cursor.Position);
            ListViewItem.ListViewSubItem subitem = this.HitTest(tmpPoint).SubItem;
            ListViewItem item = this.HitTest(tmpPoint).Item;
            if (subitem != null)
            {
                if (item.SubItems[0].Equals(subitem))
                {
                    //EditItem(subitem);
                    EditItem(subitem, new Rectangle(item.Bounds.Left, item.Bounds.Top, this.Columns[0].Width, item.Bounds.Height));
                }
                else
                {
                    EditItem(subitem);
                }
            }
            base.OnClick(e);
            //base.OnDoubleClick(e);
        }

        protected override void WndProc(ref   Message m)
        {
            if (m.Msg == 0x115 || m.Msg == 0x114)
            {
                this.m_tb.Visible = false;
            }
            base.WndProc(ref m);
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            m_tb.TextChanged -= new EventHandler(m_tb_TextChanged);
            (sender as TextBox).Visible = false;
        }

        private void m_tb_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Tag is ListViewItem.ListViewSubItem)
            {
                (this.m_tb.Tag as ListViewItem.ListViewSubItem).Text = this.m_tb.Text;
            }

        }
    }
}
