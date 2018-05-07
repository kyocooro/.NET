using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;



namespace ExcelAuction.WebbrowserHandler
{
    class PayKantanManager : Manager
    {
        int processingIndex;
        public void payKantan()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new PayKantanHandler();
                webForm.webHandler.defaultManager = this;
            }
            processNextItem();
        }

        public override void didPayKantan(string itemID, string paymentInfo)
        {
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
                ((PayKantanHandler)(webForm.webHandler)).price = itemsInfo[processingIndex].payPrice;
                ((PayKantanHandler)(webForm.webHandler)).isStore = itemsInfo[processingIndex].isStore;
                webForm.Show();
                webForm.showURL(new System.Uri("http://page.auctions.yahoo.co.jp/jp/auction/" + itemsInfo[processingIndex].ID));
            }
            else
            {
                webForm.Close();
                webForm = null;
                if (Globals.Ribbons.MainRibbon.cbxSentPaymentNotify.Checked)
                {
                    SendPaymentMsgManager manager = new SendPaymentMsgManager();
                    try
                    {
                        foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                        {
                            YahooItem item = new YahooItem();
                            Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());
                            Range idCell = Globals.ThisAddIn.Application.get_Range("A" + cell.Row.ToString());

                            if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                                item.isStore = true;
                            if (sellerCell.Font.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))
                                item.isNewSystem = true;

                            item.ID = idCell.Value2.ToString();
                            item.seller = sellerCell.Value2.ToString();

                            manager.itemsInfo.Insert(0, item);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    manager.SendPaymentMsg();
                }
            }
        }
    }
}
