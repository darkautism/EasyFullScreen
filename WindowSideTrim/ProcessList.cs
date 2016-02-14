using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyFullScreen {
    public partial class ProcessList : Form {
        public Process selectProcess;

        public ProcessList() {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            Process[] processlist = Process.GetProcesses();
            foreach (Process p in processlist) {
                ListViewItem i3 = new ListViewItem(p.ProcessName);
                i3.Tag = p;
                ListViewItem.ListViewSubItem sub_i3 = new ListViewItem.ListViewSubItem();
                sub_i3.Text = p.Id + "";
                i3.SubItems.Add(sub_i3);
                sub_i3 = new ListViewItem.ListViewSubItem();
                sub_i3.Text = p.MainWindowTitle + "";
                i3.SubItems.Add(sub_i3);
                listView1.Items.Add(i3);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e) {
            selectProcess = (Process)listView1.SelectedItems[0].Tag;
            this.Hide();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn) {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending) {
                    lvwColumnSorter.Order = SortOrder.Descending;
                } else {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            } else {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }
        private ListViewColumnSorter lvwColumnSorter;
    }
}
