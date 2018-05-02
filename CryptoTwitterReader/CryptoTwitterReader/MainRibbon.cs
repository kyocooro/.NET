using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CryptoTwitterReader
{
    public partial class Crypto
    {
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private string fomattedTweetFromJson(JObject jsonObj)
        {
            return String.Format("{0}\r\n{1}\r\n{2}", jsonObj["text"], jsonObj["created_at"], jsonObj["retweet_count"]);
        }
        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            for (int i = 1; i <= 300; i++)
            {
                Range twitterURL = Globals.ThisAddIn.Application.get_Range("C" + i.ToString());
                if (twitterURL.Value2 == null || "".Equals(twitterURL.Value2.ToString())) continue;
                string twitterID = new Uri(twitterURL.Value2.ToString()).Segments.Last();

                string tweetsFilePath = Path.Combine(Globals.ThisAddIn.Application.ActiveWorkbook.Path, twitterID + ".txt");
                if (File.Exists(tweetsFilePath))
                {
                    List<JObject> tweetsJson = new List<JObject>();

                    //create list of json from file
                    foreach (string json in File.ReadLines(tweetsFilePath))
                    {
                        tweetsJson.Add(JObject.Parse(json));
                    }

                    //newest tweet go first
                    tweetsJson.Reverse();

                    //print last 5 tweet
                    if (tweetsJson.Count >= 1)
                        Globals.ThisAddIn.Application.get_Range("F" + i.ToString()).Value2 = fomattedTweetFromJson(tweetsJson[0]);
                    if (tweetsJson.Count >= 2)
                        Globals.ThisAddIn.Application.get_Range("G" + i.ToString()).Value2 = fomattedTweetFromJson(tweetsJson[1]);
                    if (tweetsJson.Count >= 3)
                        Globals.ThisAddIn.Application.get_Range("H" + i.ToString()).Value2 = fomattedTweetFromJson(tweetsJson[2]);
                    if (tweetsJson.Count >= 4)
                        Globals.ThisAddIn.Application.get_Range("I" + i.ToString()).Value2 = fomattedTweetFromJson(tweetsJson[3]);
                    if (tweetsJson.Count >= 5)
                        Globals.ThisAddIn.Application.get_Range("G" + i.ToString()).Value2 = fomattedTweetFromJson(tweetsJson[4]);
                }
            }
        }
    }
}
