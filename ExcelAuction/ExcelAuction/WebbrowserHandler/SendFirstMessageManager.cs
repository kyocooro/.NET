using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelAuction.WebbrowserHandler
{
    class SendFirstMessageManager : Manager
    {
        int processingIndex;
        public void SendFirstMsg()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new SendFirstMsgHandler();
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
                    webForm.showURL(new System.Uri("https://order.auctions.yahoo.co.jp/jp/show/orderform?yahooID=" + System.IO.Path.GetFileNameWithoutExtension(Globals.ThisAddIn.Application.ActiveWorkbook.Name) +
                                                        "&aid=" + itemsInfo[processingIndex].ID
                                                        + "&seller=" + itemsInfo[processingIndex].seller));
                }
                else
                    webForm.showURL(new System.Uri("https://contact.auctions.yahoo.co.jp/top?syid=" + itemsInfo[processingIndex].seller +
                                                        "&aid=" + itemsInfo[processingIndex].ID));
            }
            else
            {
                webForm.Close();
                webForm = null;
            }
        }
        public override void didSendFirstMsg(string itemID, string info)
        {

            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            try
            {
                foreach (Range cell in selection.Cells)
                {
                    if (itemID.Equals(cell.Value2.ToString()))
                    {

                        cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                    }
                }


            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            processNextItem();
        }

        public override void failToSendFirstMsg(string itemID, string info)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            try
            {
                foreach (Range cell in selection.Cells)
                {
                    if (itemID.Equals(cell.Value2.ToString()))
                    {

                        cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                }


            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

            processNextItem();
        }
    }
}
