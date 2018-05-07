using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace ExcelAuction.WebbrowserHandler
{
    public class SendFirstMsgHandler : WebbrowserHandler
    {
        public DateTime paymentDay;

        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            if (itemID.Equals(HttpUtility.ParseQueryString(e.Url.Query).Get("aid")))
            {
                if (isStoreForm(e.Url))
                {
                    //is  inputed
                    //bool isInputed = false;
                    //if (browser.Document.Body.InnerText.Contains("に確定しています。"))
                    //{
                    //    isInputed = true;
                    //}

                    if (!isFormInputed(browser))
                    {
                        //bool isDeliveryOK = false, isPaymentOK = false;
                        ////set delivery
                        //if (ElementsByName(browser.Document.GetElementsByTagName("input"), "delivery_pattern").Count == 1)
                        //{
                        //    ElementsByName(browser.Document.GetElementsByTagName("input"), "delivery_pattern")[0].SetAttribute("checked", "checked");
                        //    isDeliveryOK = true;
                        //}

                        ////set juucho method
                        ////foreach (HtmlElement radio in ElementsByName(browser.Document.GetElementsByTagName("input"), "payment_type"))
                        ////{
                        ////    if (radio.GetAttribute("value").Equals("502"))
                        ////    {
                        ////        radio.SetAttribute("checked", "checked");
                        ////        isPaymentOK = true;
                        ////        break;
                        ////    }
                        ////}

                        ////if failed, set japanet
                        ////if (!isPaymentOK)
                        ////{
                        ////    foreach (HtmlElement radio in ElementsByName(browser.Document.GetElementsByTagName("input"), "payment_type"))
                        ////    {
                        ////        if (radio.Parent.Parent.InnerText.Contains("ジャパンネット"))
                        ////        {
                        ////            radio.SetAttribute("checked", "checked");
                        ////            isPaymentOK = true;
                        ////            break;
                        ////        }
                        ////    }
                        ////}

                        ////if failed, set kantan
                        //if (!isPaymentOK)
                        //{
                        //    foreach (HtmlElement radio in ElementsByName(browser.Document.GetElementsByTagName("input"), "payment_type"))
                        //    {
                        //        if (radio.GetAttribute("value").Equals("100"))
                        //        {
                        //            radio.SetAttribute("checked", "checked");
                        //            isPaymentOK = true;
                        //            break;
                        //        }
                        //    }
                        //}

                        //if (isDeliveryOK && isPaymentOK)
                        //{
                        //    //press confirm
                        //    foreach (HtmlElement button in browser.Document.GetElementsByTagName("input"))
                        //    {
                        //        if (button.GetAttribute("value").Equals("確認"))
                        //        {
                        //            button.InvokeMember("click");
                        //            break;
                        //        }
                        //    }
                        //}
                        if (ElementsByClass(browser.Document.All, "Button Button--proceed Button--large").Count > 0
                             && ElementsByClass(browser.Document.All, "Button Button--proceed Button--large").First<HtmlElement>() != null)
                        {
                            ElementsByClass(browser.Document.All, "ShipmentMethod__labelText").First<HtmlElement>().InvokeMember("click");
                            ElementsByClass(browser.Document.All, "Button Button--proceed Button--large").First<HtmlElement>().InvokeMember("click");
                        }
                        else
                        {
                            DialogResult dialogResult = MessageBoxEx.Show("Skip Input? If cancel, manual input", "", MessageBoxButtons.YesNoCancel, 30000);
                            if (dialogResult == DialogResult.Yes)
                            {
                                defaultManager.failToSendFirstMsg(itemID, null);

                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                browser.Navigate("http://auctions.yahoo.co.jp/jp/show/discussion?aID=" + itemID);
                            }
                            else if (dialogResult == DialogResult.Cancel)
                            {

                            }

                        }
                    }
                    else
                    {
                        defaultManager.didSendFirstMsg(itemID, null);
                    }



                }

                //end send individual msg
                else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/submit"))
                {
                    defaultManager.didSendFirstMsg(itemID, null);
                }
                //input info for new system
                else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/buyer/edit"))
                {
                    browser.Document.GetElementById("elAdressEdit").InvokeMember("click");

                    foreach (HtmlElement submitEle in browser.Document.GetElementsByTagName("input"))
                    {
                        if ("first_name".Equals(submitEle.GetAttribute("name")))
                        {
                            submitEle.SetAttribute("value", itemID);
                        }
                    }

                    ElementsByClass(browser.Document.All, "libBtnBlueL").First<HtmlElement>().InvokeMember("click");
                }
                else if (e.Url.OriginalString.Contains("https://contact.auctions.yahoo.co.jp/buyer/top"))
                {
                    //end send individual msg new system
                    if (isFormInputed(browser))
                    {
                        defaultManager.didSendFirstMsg(itemID, null);
                    }
                    else
                    {
                        //detect is new system
                        HtmlElement popupEle = null;
                        foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "libBtnGrayM close"))
                        {
                            if (ele.GetAttribute("value").Equals("閉じる"))
                            {
                                popupEle = ele;
                                break;
                            }
                        }

                        //process new system
                        if (popupEle != null)
                        {
                            popupEle.InvokeMember("click");
                            if (ElementsByClass(browser.Document.All, "libBtnBlueL mB10").Count > 0)
                            {
                                ElementsByClass(browser.Document.All, "libBtnBlueL mB10").First<HtmlElement>().InvokeMember("click");
                            }
                            else
                            {
                                defaultManager.failToSendFirstMsg(itemID, null);
                            }
                        }

                    }


                }
                else if (e.Url.OriginalString.Contains("auctions.yahoo.co.jp/jp/show/discussion"))
                {
                    foreach (HtmlElement textareaEle in browser.Document.GetElementsByTagName("textarea"))
                    {
                        if ("comment".Equals(textareaEle.GetAttribute("name")))
                        {
                            textareaEle.InnerText = getHelloMessage();
                            foreach (HtmlElement btnEle in browser.Document.GetElementById("modBlbdForm").GetElementsByTagName("input"))
                            {
                                if ("投稿する".Equals(btnEle.GetAttribute("value")))
                                {
                                    btnEle.InvokeMember("click");
                                }
                            }
                        }
                    }
                }
                else // process old system
                    foreach (HtmlElement ele in ElementsByClass(browser.Document.All, "libJsCountTextInput"))
                    {
                        //send introdution msg to individual
                        ele.InnerText = getHelloMessage();
                        //TOMONI(株) CAM HIEN
                        //320-0851 栃木県宇都宮市鶴田町1429番地 

                        //                    NGUYEN TU (HIEN)
                        //〒289-2177
                        //千葉県 匝瑳市 金原 243-1
                        //TEL:080-4144-5959

                        
                        
                        browser.Document.GetElementById("generated.input.id$1").SetAttribute("checked", "checked");
                        ElementsByClass(browser.Document.All, "libBtnBlueL mB10").First<HtmlElement>().InvokeMember("click");
                    }

            }
            //store without orderform--confirm hello message
            else if (e.Url.OriginalString.Contains("https://auctions.yahoo.co.jp/jp/show/discuss_preview"))
            {
                foreach (HtmlElement submitEle in browser.Document.GetElementsByTagName("input"))
                {
                    if ("　送信　".Equals(submitEle.GetAttribute("value")))
                    {
                        submitEle.InvokeMember("click");
                        break;
                    }
                }
            }
            else if (e.Url.OriginalString.Contains("auctions.yahoo.co.jp/jp/config/discussion_submit"))//store / finish
            {
                defaultManager.didSendFirstMsg(itemID, null);
            }
            //end send individual msg new system case 1
            else if (e.Url.OriginalString.Contains("https://auc.payment.yahoo.co.jp/Payment"))
            {
                defaultManager.didSendFirstMsg(itemID, null);
            }
            // confirm individual introdution msg
            else if (e.Url.OriginalString.Equals("https://contact.auctions.yahoo.co.jp/preview"))
            {
                browser.Document.GetElementById("mBoxPayBt").InvokeMember("click");
            }
            // confirm individual info new system
            else if (e.Url.OriginalString.Equals("https://contact.auctions.yahoo.co.jp/buyer/preview"))
            {
                ElementsByClass(browser.Document.All, "libBtnRedL").First<HtmlElement>().InvokeMember("click");
            }
            else if (e.Url.OriginalString.Equals("https://order.auctions.yahoo.co.jp/jp/confirm/orderform?preview=order_form"))
            {
                //confirm input form
                //foreach (HtmlElement button in browser.Document.GetElementsByTagName("input"))
                //{
                //    if (button.GetAttribute("value").Equals("免責事項に同意した上で送信"))
                //    {
                //        button.InvokeMember("click");
                //        break;
                //    }
                //}

                ElementsByClass(browser.Document.All, "Button Button--submit Button--large").First<HtmlElement>().InvokeMember("click");
            }
            //end input form
            else if (e.Url.OriginalString.Contains("https://order.auctions.yahoo.co.jp/jp/config/orderform"))
            {
                defaultManager.didSendFirstMsg(itemID, null);
            }



        }

        private static bool isFormInputed(WebBrowser browser)
        {
            return browser.Document.Body.OuterText.Contains("出品者からの送料の連絡をお待ちください。")
                                    || browser.Document.Body.OuterText.Contains("出品者に取引情報の連絡をしました")
                                    || browser.Document.Body.OuterText.Contains("出品者から送料の連絡がありました。")
                                    || browser.Document.Body.OuterText.Contains("取引情報が公開されました。")
                                    || browser.Document.Body.OuterText.Contains("出品者からの連絡をお待ちください。")
                                    || browser.Document.Body.OuterText.Contains("出品者から商品発送の連絡がありました。")
                                    || browser.Document.Body.OuterText.Contains("取引情報を入力しました。");
        }

        private static bool isStoreForm(Uri formURL)
        {
            return formURL.OriginalString.Contains("https://order.auctions.yahoo.co.jp/jp/show/orderform");
        }

        private string getHelloMessage()
        {
            return String.Format(@"お世話になっております。
    入金が3日間以内待っていただきませんか？（普通は1日間です）。
    送金したからすぐ連絡します。
    また、下記の住所に発送をお願いいたします。
    「送り先」
    {0} 


    大事なお連絡があったら引き取りの’その他’としてお連絡ください。
    よろしくお願いします。
    ", Globals.Ribbons.MainRibbon.getCurrentBuyerAddress(itemID));
        }
    }
}
