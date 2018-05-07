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
using System.IO;

namespace ExcelAuction
{
    public partial class LoginForm : Form
    {
        private Dictionary<string, string> accountInfo = null;
        
        public LoginForm()
        {
            InitializeComponent();
        }
        private void showAccountButton()
        {
            loadAccountInfo();
            foreach (KeyValuePair<string, string> entry in accountInfo)
            {
                foreach (Control c in this.Controls)
                    if ("1".Equals(c.Tag))
                    {
                        c.Text = entry.Key;
                        c.Tag = null;
                        break;
                    }
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Global.returnURI = new Uri("http://developer.yahoo.co.jp/start/");
            Global.client = new YahooClient("dj0zaiZpPWhzQVdWRFpuYXV0WiZzPWNvbnN1bWVyc2VjcmV0Jng9Yzk-", "");
            showAccountButton();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void webAuth_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.AbsoluteUri.Contains("https://developer.yahoo.co.jp/start/")) 
            {
                Global.authenticationCode = e.Url.AbsoluteUri.Substring(42, 8);
                Global.accessCode = Global.client.QueryAccessToken(Global.returnURI, Global.authenticationCode);
                this.Close();
            }

            if (e.Url.AbsoluteUri.Contains("http://auctions.yahoo.co.jp/")) 
            {
                //this.Close();
                LoginForm_Shown(this, null);
            }
        }
        private void loadAccountInfo()
        {
            StreamReader reader = new StreamReader(ExcelAuction.Global.storeLocation + "Account.txt");

            accountInfo = new Dictionary<string, string>();

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine();
                accountInfo.Add(line.Split(';')[0], line.Split(';')[1]);
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

        private void loginButton_Click(object sender, EventArgs e)
        {
            webAuth.Document.GetElementById("username").SetAttribute("value", ((Button)sender).Text);
            webAuth.Document.GetElementById("passwd").SetAttribute("value", accountInfo[((Button)sender).Text]);
            webAuth.Document.GetElementById(".save").InvokeMember("click");
            webAuth.Document.GetElementById("btnNext").InvokeMember("click");
        }

       
    }
}
