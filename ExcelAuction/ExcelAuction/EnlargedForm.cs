using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExcelAuction
{
    public partial class EnlargedForm : Form
    {
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool Repaint);
        private int maxWidth = 1000;
        private int maxHeight = 780;
        private void EnlargedForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(maxWidth, maxHeight);
            bool Result = MoveWindow(this.Handle, 0, 0, maxWidth, maxHeight, true);
        }

        public EnlargedForm()
        {
            Load += EnlargedForm_Load;

        }
    }
}
