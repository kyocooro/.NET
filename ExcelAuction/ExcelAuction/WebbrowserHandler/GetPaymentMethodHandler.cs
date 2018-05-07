using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelAuction.WebbrowserHandler
{
    public static class StringExtensions
    {
        public static string Right(this string str, int length)
        {
            if (str.Length < length) return "";
            return str.Substring(str.Length - length, length);
        }
    }
    class GetPaymentMethodHandler : WebbrowserHandler
    {
        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.OriginalString.Split('/').Last().Equals(itemID))
            {
                WebBrowser browser = (WebBrowser)sender;
                browser.Stop();
                //MessageBox.Show(browser.Document.GetElementById("itempayment").InnerText);
                string paymentInfo = browser.Document.GetElementById("itempayment").InnerText;
                try
                {
                    paymentInfo += ElementsByClass(browser.Document.GetElementById("modTradingNaviStep").All, "libBtnBlueL")[0].InnerText;
                }
                catch (Exception)
                {
                    MessageBox.Show(e.ToString());
                }
                
                defaultManager.didGetPaymentInfo(itemID, paymentInfo);
            }
        }
    }
}
