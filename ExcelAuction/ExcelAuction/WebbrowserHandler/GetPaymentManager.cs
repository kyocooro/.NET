using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Windows.Forms;

namespace ExcelAuction.WebbrowserHandler
{
    
    class GetPaymentManager : Manager
    {
        int processingIndex;
        public void getPayment()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new GetPaymentMethodHandler();
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
                webForm.showURL(new System.Uri("http://page.auctions.yahoo.co.jp/jp/auction/" + itemsInfo[processingIndex].ID));
            }
            else
            {
                webForm.Close();
                webForm = null;
            }
        }
        public override void didGetPaymentInfo(string itemID, string paymentInfo)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            try
            {
                foreach (Range cell in selection.Cells)
                {
                    if (itemID.Equals(cell.Value2.ToString()))
                    {

                        if (paymentInfo.Contains("取引ナビ（ベータ版）"))
                        {
                            cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        }
                        if (paymentInfo.Contains("ジャパンネット銀行支払い"))
                        {
                            cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                        }
                        else if (paymentInfo.Contains("ゆうちょ") || paymentInfo.Contains("郵貯"))
                        {
                            cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);
                        }
                        else if (paymentInfo.Contains("ジャパンネット銀行"))
                        {
                            cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }
                }


            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
            processNextItem();
        }
    }
}
