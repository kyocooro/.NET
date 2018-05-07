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

namespace ExcelAuction
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Global.returnURI = new Uri("http://developer.yahoo.co.jp/start/");
            Global.client = new YahooClient("dj0zaiZpPWhzQVdWRFpuYXV0WiZzPWNvbnN1bWVyc2VjcmV0Jng9Yzk-", "");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void webAuth_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.AbsoluteUri.Contains("http://developer.yahoo.co.jp/start/")) 
            {
                Global.authenticationCode = e.Url.AbsoluteUri.Substring(41, 8);
                Global.accessCode = Global.client.QueryAccessToken(Global.returnURI, Global.authenticationCode);
                this.Close();
            }

            if (e.Url.AbsoluteUri.Contains("http://auctions.yahoo.co.jp/")) 
            {
                this.Close();
            }
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            webAuth.Navigate(Global.client.GetServiceLoginUrl(Global.returnURI));
        }

        public void logout()
        {
            webAuth.Navigate(@"http://login.yahoo.co.jp/config/login?.lg=jp&.intl=jp&logout=1&.src=auc&.done=http%3a//auctions.yahoo.co.jp/");
        }

        private void webAuth_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "konoasa2014");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "hoanganh");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "asarinku2011");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "saigon2014");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "rinkuvn");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "rinkuttq90");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", "asarinku2012");
            webAuth.Document.GetElementById("passwd").SetAttribute("value", "saigon2014");
            webAuth.Document.GetElementById(".save").InvokeMember("click");
        }
    }
}
