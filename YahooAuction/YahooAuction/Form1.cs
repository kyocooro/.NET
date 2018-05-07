using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;

namespace YahooAuction
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //webAuth.Navigate(Global.client.GetServiceLoginUrl(Global.returnURI));
            //WebBrowser browser = new WebBrowser();
            //browser.Navigated +=new WebBrowserNavigatedEventHandler(webAuth_Navigated);
            
        }

        private void webAuth_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.AbsoluteUri.Contains("http://developer.yahoo.co.jp/start/")) 
            {
                Global.authenticationCode = e.Url.AbsoluteUri.Substring(41, 8);
                Global.accessCode = Global.client.QueryAccessToken(Global.returnURI, Global.authenticationCode);
                MainForm form = new MainForm();
                form.Show();
                form.Owner = this;
                this.Hide();
            }
        }

        private void webAuth_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "hau1772014");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "vinhhien");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            webAuth.Navigate(@"http://login.yahoo.co.jp/config/login?.lg=jp&.intl=jp&logout=1&.src=auc&.done=http%3a//auctions.yahoo.co.jp/");
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            Global.returnURI = new Uri("http://developer.yahoo.co.jp/start/");
            Global.client = new YahooClient("dj0zaiZpPWhzQVdWRFpuYXV0WiZzPWNvbnN1bWVyc2VjcmV0Jng9Yzk-", "");
            webAuth.Navigate(Global.client.GetServiceLoginUrl(Global.returnURI));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "hau1772014");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "vinhhien");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }
    }
}
