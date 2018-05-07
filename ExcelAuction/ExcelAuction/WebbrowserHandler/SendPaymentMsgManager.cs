using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAuction.WebbrowserHandler
{
    class SendPaymentMsgManager : Manager
    {
        int processingIndex;
        public void SendPaymentMsg()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new SendPaymentMsgHandler();

                string message = @"遅れて申し訳ありませんでした。
    支払いました。

お手数ですが、よろしくお願いします。";
                //無理のお願いでがこの商品を早目に発送していただきませんか？
                //尚、宛名は「tomoniロジスティクス株式会社」
                //但し書は「お品代として」でお願いいたします。
                //message = Microsoft.VisualBasic.Interaction.InputBox("", "", message);
                ((SendPaymentMsgHandler)(webForm.webHandler)).message = message;
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
                webForm.Show();
                if (itemsInfo[processingIndex].isStore)
                {
                    webForm.showURL(new System.Uri("http://page.auctions.yahoo.co.jp/jp/show/discussion?aID=" + itemsInfo[processingIndex].ID));
                }
                else
                {
                    webForm.showURL(new System.Uri("https://contact.auctions.yahoo.co.jp/top?syid=" + itemsInfo[processingIndex].seller +
                                                            "&aid=" + itemsInfo[processingIndex].ID));
                }
            }
            else
            {
                webForm.Close();
                webForm = null;
            }
        }
        public override void didSendPaymentMsg(string itemID, string info)
        {

            //System.IO.File.WriteAllText("D:\\Auction\\QA\\" + itemID + ".html", qaInfo, Encoding.Unicode);
            foreach (Range cell in Global.GetVisibleSelectionCells())
            {
                if (itemID.Equals(cell.Value2.ToString()))
                {
                    Range priceCell = Globals.ThisAddIn.Application.get_Range("F" + cell.Row.ToString());
                    priceCell.Value2 = "Đã gửi tiền";

                    Range payDateRange = Globals.ThisAddIn.Application.get_Range("B" + cell.Row.ToString());
                    payDateRange.Value2 = DateTime.Today.ToString("dd_M_yyyy");
                    Globals.ThisAddIn.Application.ActiveWorkbook.Save();
                    break;
                }
            }
            processNextItem();
        }
    }
}
