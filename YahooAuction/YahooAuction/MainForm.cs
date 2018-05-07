using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace YahooAuction
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void downloadAuctionItemInfo(string id)
        {
            string applicationPath = Application.StartupPath;
            //DateTime endtime = Convert.ToDateTime(jsonInfo["EndTime"]);
            //String endDate = endtime.ToShortDateString().Replace('/', '_');
            string itemFolder = Path.Combine(applicationPath, id);

            if (!Directory.Exists(itemFolder))
            {
                JToken jsonInfo = Global.client.GetJsonItemInfo(id);
                if (jsonInfo == null)
                    return;
                Directory.CreateDirectory(itemFolder);

                File.WriteAllText(Path.Combine(itemFolder, "info.txt"), jsonInfo.ToString());

                WebClient webClient = new WebClient();
                try
                {
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image1"], Path.Combine(itemFolder, "1.jpg"));
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image2"], Path.Combine(itemFolder, "2.jpg"));
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image3"], Path.Combine(itemFolder, "3.jpg"));
                }
                catch (System.ArgumentNullException ex)
                {
                	
                }
                
            }
            
        }

        private void downloadInBackground()
        {
            int total = 1, end = 1;
            int page = 0;
            while (total >= end)
            {
                page++;
                List<string> itemsID = Global.client.GetSuccessfulItemsAtPage(page, ref total, ref end);
                foreach (string id in itemsID)
                {
                    downloadAuctionItemInfo(id);
                }
            }

        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            Thread backgroundThread = new Thread(downloadInBackground);
            backgroundThread.Start();
        }
    }
}
