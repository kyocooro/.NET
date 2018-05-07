using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Threading;

namespace ExcelAuction.WebbrowserHandler
{
    class GetQAManager : Manager
    {
        int processingIndex;
        private List<HtmlElement> ElementsByClass(HtmlElementCollection elements, string className)
        {
            List<HtmlElement> results = new List<HtmlElement>();
            foreach (HtmlElement e in elements)
                if (e.GetAttribute("className") == className)
                    results.Add(e);
            return results;
        }

        public void getQA()
        {
            processingIndex = -1;
            if (webForm == null || webForm.isClosed)
            {
                webForm = new WebForm();
                webForm.webHandler = new QAHandler();
                webForm.webHandler.defaultManager = this;
            }
            processNextItem();
        }

        private void processNextItem()
        {
            processingIndex++;
            Thread.Sleep(200);
            if (processingIndex < itemsInfo.Count)
            {

                webForm.Text = itemsInfo[processingIndex].ID;
                //MessageBox.Show(processingIndex.ToString());
                webForm.webHandler.itemID = itemsInfo[processingIndex].ID;
                webForm.Show();
                if (itemsInfo[processingIndex].isStore)
                {
                    webForm.showURL(new System.Uri("https://order.auctions.yahoo.co.jp/jp/show/orderform?seller=" + itemsInfo[processingIndex].seller +
                                                        "&aid=" + itemsInfo[processingIndex].ID
                                                        + "&yahooID=" + System.IO.Path.GetFileNameWithoutExtension(Globals.ThisAddIn.Application.ActiveWorkbook.Name)));
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
        public override void didGetQA(string itemID, HtmlElement qaInfo)
        {
            if (qaInfo != null)
            {
                System.IO.Directory.CreateDirectory(ExcelAuction.Global.storeLocation + "QA\\");
                System.IO.File.WriteAllText(ExcelAuction.Global.storeLocation + "QA\\" + itemID + ".html", qaInfo.OuterHtml.Normalize(NormalizationForm.FormKC), Encoding.Unicode);


                if (itemsInfo[processingIndex].isStore) //is store
                {
                    try
                    {

                        foreach (Range cell in Global.GetVisibleSelectionCells())
                        {
                            if (itemID.Equals(cell.Value2.ToString()))
                            {
                                foreach (HtmlElement ele in qaInfo.GetElementsByTagName("td"))
                                {
                                    if (ele.InnerText != null && ele.InnerText.Equals("合計金額（税込）"))
                                    {
                                        Range priceCell = Globals.ThisAddIn.Application.get_Range("D" + cell.Row.ToString());
                                        priceCell.Value2 = ele.NextSibling.InnerText.Replace("円", "");
                                        break;
                                    }
                                }

                                //foreach (HtmlElement ele in qaInfo.GetElementsByTagName("td"))
                                //{
                                //    if (ele.InnerText != null && ele.InnerText.Equals("お支払い方法"))
                                //    {
                                //        Range accountCell = Globals.ThisAddIn.Application.get_Range("C" + cell.Row.ToString());
                                //        accountCell.Value2 = ExtractAccountNumber(ele.NextSibling.InnerText);
                                //        break;
                                //    }
                                //}

                                cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                break;
                            }
                        }

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    try
                    {

                        foreach (Range cell in Global.GetVisibleSelectionCells())
                        {
                            if (itemID.Equals(cell.Value2.ToString()))
                            {
                                foreach (HtmlElement ele in qaInfo.All)
                                {
                                    if (ele.GetAttribute("className") == "decCnfWr")
                                    {
                                        if (ele.InnerText != null && ele.InnerText.Contains("落札価格"))
                                        {
                                            Range priceCell = Globals.ThisAddIn.Application.get_Range("D" + cell.Row.ToString());
                                            string cleanedPrice = ele.InnerText.Remove(ele.InnerText.IndexOf("（"));
                                            priceCell.Value2 = cleanedPrice.Replace("円", "");
                                            break;
                                        }
                                    }

                                }

                                //foreach (HtmlElement ele in qaInfo.GetElementsByTagName("td"))
                                //{
                                //    if (ele.InnerText != null && ele.InnerText.Equals("お支払い方法"))
                                //    {
                                //        Range accountCell = Globals.ThisAddIn.Application.get_Range("C" + cell.Row.ToString());
                                //        accountCell.Value2 = ExtractAccountNumber(ele.NextSibling.InnerText);
                                //        break;
                                //    }
                                //}

                                cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                break;
                            }
                        }

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }

            processNextItem();
        }

        public override void StoreFormInputingStatus(string itemID, string info)
        {

            try
            {
                foreach (Range cell in Global.GetVisibleSelectionCells())
                {
                    if (itemID.Equals(cell.Value2.ToString()))
                    {
                        if (info == null)
                        {
                            cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);
                            processNextItem();
                        }
                        else
                        {
                            cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        }

                    }
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }
        private string ExtractAccountNumber(string accountInfo)
        {
            if (accountInfo.Contains("Yahoo!かんたん決済"))
                return "kantan";
            else if (accountInfo.Contains("銀行"))
                try
                {
                    return accountInfo.Substring(0, 50);
                }
                catch (ArgumentOutOfRangeException)
                {

                }

            return accountInfo;
        }
    }


}
