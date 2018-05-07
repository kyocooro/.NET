using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;


namespace ExcelAuction.WebbrowserHandler
{
    class SendPaymentMsgHandler : WebbrowserHandler
    {
        public string message;
        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            if (itemID.Equals(HttpUtility.ParseQueryString(e.Url.Query).Get("aid")))
            {
                if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/submit"))
                {
                    defaultManager.didSendPaymentMsg(itemID, null);
                }
                else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/buyer/"))
                {
                    try
                    {
                        browser.Document.GetElementById("textarea").InnerText = message;
                        browser.Document.GetElementById("submitButton").InvokeMember("click");
                        defaultManager.didSendPaymentMsg(itemID, null);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                else if (e.Url.OriginalString.Contains("auctions.yahoo.co.jp/jp/show/discussion"))
                {
                    foreach (HtmlElement ele in browser.Document.GetElementsByTagName("textarea")) //store/input message
                    {
                        
                        ele.InnerText = message;
                        foreach (HtmlElement btnEle in browser.Document.GetElementById("modBlbdForm").GetElementsByTagName("input"))
                        {
                            if ("投稿する".Equals(btnEle.GetAttribute("value")))
                            {
                                btnEle.InvokeMember("click");
                            }
                        }

                    }

                }
                else if (browser.Document.GetElementById("mBoxPayBt") != null)  //induvidual/finish
                { 
                    defaultManager.didSendPaymentMsg(itemID, null);
                    //FirstElementByClass(browser.Document.All, "libBtnRedL").InvokeMember("click");
                }
                else
                    foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "libJsCountTextInput")) //individual/input message
                    {
                        ele.InnerText = message;
                        browser.Document.GetElementById("generated.input.id$2").SetAttribute("checked", "checked");
                        ElementsByClass(browser.Document.All, "libBtnBlueL mB10").First<HtmlElement>().InvokeMember("click");
                    }
            }
            else if (e.Url.OriginalString.Equals("https://contact.auctions.yahoo.co.jp/preview")) //induvidual/preview message
            {
                browser.Document.GetElementById("mBoxPayBt").InvokeMember("click");
            }
            else if (e.Url.OriginalString.Contains("auctions.yahoo.co.jp/jp/show/discuss_preview"))  //store/preview message
            {
                foreach (HtmlElement btnEle in browser.Document.GetElementsByTagName("input"))
                {
                    if ("　送信　".Equals(btnEle.GetAttribute("value")))
                    {
                        btnEle.InvokeMember("click");
                        break;
                    }
                }
            }
            else if (e.Url.OriginalString.Contains("auctions.yahoo.co.jp/jp/config/discussion_submit"))//store / finish
            {
                defaultManager.didSendPaymentMsg(itemID, null);
            }
            
        }

        public virtual void WhenNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }
    }
}
