using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Policy;
using Microsoft.Office.Interop.Excel;

namespace ExcelAuction
{
    public partial class WebForm : Form
    {
        public bool isClosed = false;
        public WebForm()
        {
            InitializeComponent();
        }

        private void WebForm_Load(object sender, EventArgs e)
        {

        }

        public void showURL(Uri docURI)
        {
            webBrowser.Navigate(docURI);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void btnFillAccount_Click(object sender, EventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            webBrowser.Document.ExecCommand("Copy", false, null);
            Globals.ThisAddIn.Application.get_Range("C" + Convert.ToString(selectedCell.Row)).Value2 = Clipboard.GetText();
        }

        private void btnFillMoney_Click(object sender, EventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            webBrowser.Document.ExecCommand("Copy", false, null);
            Globals.ThisAddIn.Application.get_Range("D" + Convert.ToString(selectedCell.Row)).Value2 = Clipboard.GetText();
        }

        private void WebForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }



        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {

            switch (keys)
            {

                case Keys.F3 :

                    btnFillAccount_Click(null, null);

                    return true;
                case Keys.F4 :
                    btnFillMoney_Click(null, null);
                    return true;

            }



            return false;

        }

        private void WebForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
        }


    }
}
