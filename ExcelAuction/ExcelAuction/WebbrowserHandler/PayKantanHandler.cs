using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Web;

namespace ExcelAuction.WebbrowserHandler
{
    class PayKantanHandler : WebbrowserHandler
    {
        public string price = "0";
        public bool isStore = false;
        public bool isPaymentMethodRepeatFlag = false;
        public bool isPaymentConfirmRepeatFlag = false;
        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            if (e.Url.OriginalString.Split('/').Last().Equals(itemID))
            {

                //check new system
                foreach (HtmlElement ele in ElementsByClass(browser.Document.GetElementById("modTradingNaviStep").All, "libBtnBlueL"))
                {
                    if (ele.InnerText.Equals("取引ナビ"))
                    {
                        ele.InvokeMember("click");
                    }

                }


                //click kantan button
                foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "libBtnGrayM"))
                {
                    if (ele.InnerText.Equals("支払う"))
                    {
                        ele.InvokeMember("click");
                    }
                    
                }
            }
            else if (e.Url.OriginalString.Contains("https://auc.payment.yahoo.co.jp/Payment"))
            {
                if (browser.Document.GetElementById("fnv") != null && browser.Document.GetElementById("fnv").InnerText.Equals("取引ナビ"))
                {
                    defaultManager.didPayKantan(itemID, null);
                }
                else if (browser.Document.GetElementById("paydtl") != null && "支払い明細".Equals(browser.Document.GetElementById("paydtl").InnerText))
                {
                    defaultManager.didPayKantan(itemID, null);
                }
                else if (browser.Document.GetElementById("paybtn") == null)
                {
                    try
                    {
                        //if (price != null)
                        //{
                        //    //browser.Document.GetElementById("price").SetAttribute("value", price);
                        //    //browser.Document.GetElementById("shipping").SetAttribute("value", "0");
                        //}
                        //else
                        //{
                        //    if (!isStore)
                        //        MessageBox.Show("Nhập giá");
                        //}

                        if (isStore)
                        {
                            //browser.Document.GetElementById("bank").SetAttribute("checked", "checked");
                            //HtmlElement ele = browser.Document.GetElementById("creditcard");
                            payByCreditCard(browser);
                            
                        }
                            
                        else
                        {
                            //browser.Document.GetElementById("bank").SetAttribute("checked", "checked");
                            payByCreditCard(browser);
                            //HtmlElement ele = browser.Document.GetElementById("jnb").Parent;
                            //if (ele.InnerText.Contains("Yahoo!ウォレットの支払口座に登録する"))
                            //{
                            //    payByCreditCard(browser);
                            //}
                                
                            //else
                            //    browser.Document.GetElementById("jnb").InvokeMember("click");
                        }

                        if (!isPaymentMethodRepeatFlag)
                            browser.Document.GetElementById("nextbtn").FirstChild.InvokeMember("click");
                        isPaymentMethodRepeatFlag = true;

                    }
                    catch (Exception)
                    {

                    }
                    
                }
                else
                {
                    if (!isPaymentConfirmRepeatFlag)
                    {
                        browser.Document.GetElementById("paybtn").ScrollIntoView(true);
                        browser.Document.GetElementById("paybtn").FirstChild.InvokeMember("click");
                    }
                    isPaymentConfirmRepeatFlag = true;
                }

            }
            else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp"))
            {
                //click kantan
                foreach (HtmlElement ele in ElementsByClass(browser.Document.GetElementById("yjMain").All, "libBtnRedL"))
                {
                    if (ele.InnerText.Equals("Yahoo!かんたん決済で支払う"))
                    {
                        ele.InvokeMember("click");
                    }

                }
            }

        }

        private void payByCreditCard(WebBrowser browser)
        {
            browser.Document.GetElementById("wcc").InvokeMember("click");
           
            foreach (HtmlElement wscodeEle in ElementsByName(browser.Document.All, "wscode"))
            {
                if ("input".Equals(wscodeEle.TagName, StringComparison.OrdinalIgnoreCase))
                {
                    wscodeEle.InnerText = "227";
                    isPaymentMethodRepeatFlag = false;

                }

            }
        }

        public virtual void WhenNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }
    }
}
