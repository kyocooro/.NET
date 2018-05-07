using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ExcelAuction.WebbrowserHandler
{
    class GetSellerInfoManager : Manager
    {
        int processingIndex;
        public void getSellerInfo()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new GetSellerInfoHandler();
                object crumbID = Microsoft.VisualBasic.Interaction.InputBox("Nhap ma", "Title", Properties.Settings.Default["Scrumb"].ToString());
                ((GetSellerInfoHandler)(webForm.webHandler)).crumID = crumbID.ToString();
                Properties.Settings.Default["Scrumb"] = crumbID.ToString();;
                Properties.Settings.Default.Save();
                webForm.webHandler.defaultManager = this;
            }
            processNextItem();
        }

        private void processNextItem()
        {
            processingIndex++;
            if (processingIndex < itemsInfo.Count)
            {

                webForm.Text = itemsInfo[processingIndex].ID;
                //MessageBox.Show(processingIndex.ToString());
                webForm.webHandler.itemID = itemsInfo[processingIndex].ID;
                ((GetSellerInfoHandler)(webForm.webHandler)).seller = itemsInfo[processingIndex].seller;
                webForm.Show();
                webForm.showURL(new System.Uri("http://page.auctions.yahoo.co.jp/jp/auction/" + itemsInfo[processingIndex].ID));
            }
            else
            {
                webForm.Close();
                webForm = null;
            }
        }
        public override void didGetSellerInfo(string itemID, string info)
        {
            //remove trunk data
            info = info.Replace("yads.c.yimg.jp/js/yads.js", "");
            System.IO.File.WriteAllText("D:\\Auction\\Seller\\" + itemsInfo[processingIndex].endDate + " " + (itemsInfo.Count - processingIndex).ToString() + " "+ itemID + " " + itemsInfo[processingIndex].endPrice.ToString() 
            + "jpy" + ".html", info, Encoding.Unicode);

            processNextItem();
        }
    }
}
