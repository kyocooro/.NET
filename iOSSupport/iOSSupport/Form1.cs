using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace iOSSupport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string[] rawLines = Regex.Split(txtRaw.Text, "\r\n");
            int eleSize;
            if (rdb4.Checked)
                eleSize = 4;
            else if (rdb3.Checked)
                eleSize = 3;
        }
    }
}
