using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace ExcelAuction.WebbrowserHandler
{
    class QAHandler : WebbrowserHandler
    {
        public override void WhenDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (itemID.Equals(HttpUtility.ParseQueryString(e.Url.Query).Get("aid")))
            {
                WebBrowser browser = (WebBrowser)sender;
                List<HtmlElement> expandBtns = ElementsByClass(browser.Document.All, "libJsExpandToggleBtn");
                if (expandBtns.Count == 0) //is store
                {
                    bool isInputed = true;
                    foreach (HtmlElement button in browser.Document.GetElementsByTagName("input"))
                    {
                        if (button.GetAttribute("value").Equals("確認"))
                        {
                            isInputed = false;
                            defaultManager.StoreFormInputingStatus(itemID, null);

                            break;
                        }
                    }

                    if (isInputed)
                    {
                        saveWebToImage(browser, itemID);
                        WriteQAToFile(browser.Document.Body);
                        defaultManager.StoreFormInputingStatus(itemID, "inputed");
                    }
                }
                else // is indiviual
                {
                    foreach (HtmlElement ele in base.ElementsByClass(browser.Document.All, "libJsExpandToggleBtn"))
                    {
                        ele.InvokeMember("click");

                    }

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(2000);
                        browser.Parent.Invoke((MethodInvoker)delegate
                        {
                            List<HtmlElement> qaEles = ElementsByClass(browser.Document.All, "untmainMessageList");
                            if (qaEles.Count > 0)
                            {
                                saveWebToImage(browser, itemID);
                                WriteQAToFile(qaEles[0]);
                            }
                            else
                                try
                                {
                                    saveWebToImage(browser, itemID);
                                    WriteQAToFile(ElementsByClass(browser.Document.All, "acMdPaymentInfo")[0]);
                                }
                                catch (Exception)
                                {
                                    WriteQAToFile(null);
                                    //MessageBox.Show("error WriteQAToFile");
                                }


                        });


                    }).Start();
                }




            }

        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void saveWebToImage(WebBrowser browser, string fileName)
        {
            return;
            try
            {
                Size originalSize = new Size(browser.Width, browser.Height);

                // Change to full scroll size
                int scrollHeight = browser.Document.Body.ScrollRectangle.Height;
                int scrollWidth = browser.Document.Body.ScrollRectangle.Width;

                Bitmap image = new Bitmap(scrollWidth, scrollHeight);



                // Draw to image
                browser.Parent.DrawToBitmap(image, new Rectangle(0, 0, scrollWidth, scrollHeight));

                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                image.Save(ExcelAuction.Global.storeLocation + fileName + ".jpg", jpgEncoder, myEncoderParameters);

            }
            catch (Exception)
            {

            }
        }
        private void WriteQAToFile(HtmlElement ele)
        {

            defaultManager.didGetQA(itemID, ele);
        }
    }
}
