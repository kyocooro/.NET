using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using ExcelAuction.WebbrowserHandler;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace ExcelAuction.Tools
{
    public class ExtractTwiiterHandler : WebbrowserHandler.WebbrowserHandler
    {
        public bool firstTime = true;

        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            if (firstTime)
            {
                firstTime = false;
                new Thread(() =>
                {

                    Thread.Sleep(2000);
                    browser.Parent.Invoke((MethodInvoker)delegate
                    {
                        bool hasTwitter = false;
                        foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "twitter-timeline"))
                        {
                            ((ExtractTwiiterManager)defaultManager).didGetTwitterInfo(itemID, ele.GetAttribute("href"));
                            hasTwitter = true;
                        }
                        if (!hasTwitter)
                            ((ExtractTwiiterManager)defaultManager).failGetTwitterInfo(itemID, null);

                    });


                }).Start();
            }
            
            
            

            
        }


    }


    class ExtractTwiiterManager : Manager
    {
        int processingIndex;
        public void begin()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new ExtractTwiiterHandler();
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
                ((ExtractTwiiterHandler)(webForm.webHandler)).firstTime = true;
                webForm.Show();
                webForm.showURL(new System.Uri(itemsInfo[processingIndex].seller));
            }
            else
            {
                webForm.Close();
                webForm = null;
            }
        }
        public void didGetTwitterInfo(string itemID, string info)
        {

            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            try
            {
                foreach (Range cell in selection.Cells)
                {
                    if (itemID.Equals(cell.Value2.ToString()))
                    {

                        Range twitterCell = Globals.ThisAddIn.Application.get_Range("P" + cell.Row.ToString());
                        twitterCell.Value2 = info;
                    }
                }


            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            processNextItem();
        }

        public void failGetTwitterInfo(string itemID, string info)
        {
            

            processNextItem();
        }
    }
}
