using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Web;
using System.Threading;

namespace ExcelAuction.WebbrowserHandler
{
    class GetSellerInfoHandler : WebbrowserHandler
    {
        public string crumID = null;
        public string seller = null;

        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            if (e.Url.OriginalString.Split('/').Last().Equals(itemID))
            {
                //check is store
                foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "decTx02"))
                {
                    if (ele.InnerText !=null && ele.InnerText.Equals("オークションストア"))
                    {
                        foreach (HtmlElement formBtn in ElementsByClass(browser.Document.GetElementById("modTradingNaviStep").All, "libBtnBlueL"))
                        {
                            if (formBtn.InnerText.Equals("オーダーフォーム"))
                            {
                                //if （外部サイト）
                                if (formBtn.Parent.InnerText.Contains("（外部サイト）") == false)
                                {
                                    formBtn.InvokeMember("click");
                                    return;
                                }
                                
                            }

                        }
                        //is store and dont have input form button
                        browser.Navigate("http://auctions.yahoo.co.jp/jp/show/discussion?aID=" + itemID);
                        return;
                    }

                }
                
                //check new system
                foreach (HtmlElement ele in ElementsByClass(browser.Document.GetElementById("modTradingNaviStep").All, "libBtnBlueL"))
                {
                    if (ele.InnerText.Equals("取引ナビ"))
                    {
                        ele.InvokeMember("click");
                    }

                }


                //old system click kantan button
                foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "ptsTnaviBtnWr"))
                {
                    if (ele.InnerText.Equals("取引ナビ"))
                    {
                        ele.InvokeMember("click");
                    }

                }

                
            }
            else if (e.Url.OriginalString.Contains("https://auctions.yahoo.co.jp/jp/show/discussion?aID="))
            {
                defaultManager.didGetSellerInfo(itemID, browser.DocumentText);
            }
            else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/buyer/top"))
            {
                if (HttpUtility.ParseQueryString(e.Url.Query).Get(".crumb") == null)
                {
                    if (crumID != null && !crumID.Equals(""))
                    {
                        UriBuilder uriBuilder = new UriBuilder(e.Url);
                        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                        query[".crumb"] = crumID;
                        uriBuilder.Query = query.ToString();
                        browser.Navigate(uriBuilder.Uri);
                    }
                    
                }
                else
                {
                     if (crumID == null || crumID.Equals(""))
                     {
                         crumID = HttpUtility.ParseQueryString(e.Url.Query).Get(".crumb");
                         Properties.Settings.Default["Scrumb"] = crumID;
                         Properties.Settings.Default.Save();
                     }
                     HtmlElement mainEle = browser.Document.Body;

                    
                    
                    //remove message
                    //foreach (HtmlElement messageForm in ElementsByClass(mainEle.Children, "acMdStatusImage"))
                    //{
                    //    messageForm.OuterHtml = "";
                    //}
                    //foreach (HtmlElement messageForm in ElementsByClass(mainEle.Children, "acMdStatusCmt"))
                    //{
                    //    messageForm.OuterHtml = "";
                    //}
                    //foreach (HtmlElement messageForm in ElementsByClass(mainEle.Children, "acMdMsgForm"))
                    //{
                    //    messageForm.OuterHtml = "";
                    //}

                    //expand info
                     string sellerInfo = browser.DocumentText.Replace("DISPLAY: none", "display: block");

                    defaultManager.didGetSellerInfo(itemID, sellerInfo);
                }

            }
            else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/top"))
            {
                foreach (HtmlElement ele in base.ElementsByClass(browser.Document.All, "libJsExpandToggleBtn"))
                {
                    ele.InvokeMember("click");

                }

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Thread.Sleep(4000);
                    browser.Parent.Invoke((MethodInvoker)delegate
                    {
                        List<HtmlElement> qaEles = ElementsByClass(browser.Document.All, "untmainMessageList");
                        HtmlElement mainEle = browser.Document.Body;

                        string sellerInfo = mainEle.OuterHtml.Replace("DISPLAY: none", "display: block");


                        defaultManager.didGetSellerInfo(itemID, string.Format(Properties.Resources.htmltemplate, sellerInfo));
                        //メッセージの取得ができませんでした。再度お試しください。
                        //メッセージを取得中です。


                    });


                }).Start();

            }
            else if (e.Url.OriginalString.Contains("https://order.auctions.yahoo.co.jp/jp/show/orderform"))
            {
                Encoding eucjp = Encoding.GetEncoding("euc-jp");
                Encoding utf8 = Encoding.UTF8;
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = utf8;
                string sellerInfo = client.DownloadString("http://auctions.yahoo.co.jp/html/profile/" + seller + ".html");
                string coding = browser.Document.Encoding;
                byte[] eucjpBytes = eucjp.GetBytes(browser.Document.Body.OuterHtml);
                byte[] utf8Bytes = Encoding.Convert(eucjp, utf8, eucjpBytes);
                sellerInfo += Encoding.UTF8.GetString(utf8Bytes);
                defaultManager.didGetSellerInfo(itemID, sellerInfo);
            }
        }



        public virtual void WhenNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }
    }
}
