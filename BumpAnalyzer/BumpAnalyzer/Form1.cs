using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Threading;
using LumenWorks.Framework.IO.Csv;

namespace BumpAnalyzer
{
    public partial class Form1 : Form
    {
        private string workingLocation = "C:\\OneDrive - Ho Chi Minh City University of Technology\\BumpTime";
        private string dataLocation = "Z:\\CoinData";
        string watchListString = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartMonitor_Click(object sender, EventArgs e)
        {
            //foreach (var files in Directory.GetFiles(dataLocation))
            //{
            //    string allLine = File.ReadAllText(files);
            //    if (!allLine.Contains("ID,name,symbol"))
            //    {
            //        allLine = allLine.Replace("\0\r\n", "");
            //        allLine = allLine.Replace("\0", "");
            //        File.WriteAllText(files, "ID,name,symbol,rank,price_usd,price_btc,last_24h_volume_usd,market_cap_usd,available_supply,total_supply,max_supply,percent_change_1h,percent_change_24h,percent_change_7d,last_updated\r\n" + allLine);

            //    }

            //}

            timerIndexUpdator.Start();
            timerIndexUpdator_Tick(null, null);
            Thread.Sleep(5000);
            timerAnalyzer.Start();
            timerAnalyzer_Tick(null, null);

            Thread.Sleep(5000);
            timerHourlyAnalyzer.Start();
            timerHourlyAnalyzer_Tick(null, null);
        }

        private void timerIndexUpdator_Tick(object sender, EventArgs e)
        {
            try
            {
                watchListString = File.ReadAllText(Path.Combine(workingLocation, "watchlist.ini"));
                string jsonString = new WebClient().DownloadString("https://api.coinmarketcap.com/v1/ticker/?limit=0");
                
                string watchListPrice = "Watchlist Price \r\n";
                JArray json = JArray.Parse(jsonString);
                List<string> currencyList = new List<string>();
                if (json != null)
                {
                    IEnumerable<JToken> jsonItems = json.Children();

                    foreach (JToken jsonItem in jsonItems)
                    {
                        
                        if (jsonItem["24h_volume_usd"] != null)
                        {
                            double volume = 0;
                            double.TryParse(jsonItem["24h_volume_usd"].ToString() , out volume);
                            if (volume > 500000)
                                currencyList.Add(jsonItem["id"].ToString());
                        }

                        if (jsonItem["price_usd"] != null)
                        {
                            double price = 0;
                            double.TryParse(jsonItem["price_usd"].ToString(), out price);
                            if (watchListString.Contains(jsonItem["id"].ToString()))
                                watchListPrice += jsonItem["id"].ToString() + ": " + jsonItem["price_usd"].ToString() + "\r\n";


                        }
                    }

                    triggerAlarm(watchListPrice, 0, 0);
                    File.WriteAllLines(Path.Combine(workingLocation, "index.ini"), currencyList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void timerAnalyzer_Tick(object sender, EventArgs e)
        {
            string[] currencyList = null;
            try
            {
                currencyList = File.ReadAllLines(Path.Combine(workingLocation, "index.ini"));
                watchListString = File.ReadAllText(Path.Combine(workingLocation, "watchlist.ini"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            if (currencyList != null)
                {
                    string alarmMessage = "";
                foreach (string item in currencyList)
                {
                    //load data
                    // open the file "data.csv" which is a CSV file with headers
                    try
                    {
                        using (CsvReader csv =
                           new CsvReader(new StreamReader(Path.Combine(dataLocation, item + ".txt")), true))
                        {



                            //string[] headers = csv.GetFieldHeaders();
                            int rowCount = 0;
                            int[] timestampArray = new int[10000];
                            double[] volumesArray = new double[10000];
                            long twoDayAgo = (new DateTimeOffset(DateTime.Now.AddDays(-2))).ToUnixTimeSeconds();

                            while (csv.ReadNextRecord())
                            {
                                int recordTime = Convert.ToInt32(csv[14]);

                                if (recordTime > twoDayAgo)
                                {
                                    volumesArray[rowCount] = Convert.ToDouble(csv[6]);
                                    timestampArray[rowCount] = Convert.ToInt32(csv[14]);
                                    rowCount++;
                                }

                            }


                            //analyzing
                            double increasingRatio = volumesArray[rowCount - 1] / volumesArray.Take(rowCount).Min();
                            if (increasingRatio >= 8 && volumesArray[rowCount - 1] > 2000000) // volume incresse 8 times in last 2 days
                            {
                                logCoinEvent(item, "Bump Volume p: " + Math.Round(increasingRatio, 2));
                                alarmMessage += item + " " + volumesArray[rowCount - 1].ToString("N") + " p: " + Math.Round(increasingRatio, 2) + "\r\n";

                                if (watchListString.Contains(item))
                                    alarmMessage += "******* \r\n";
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllLines(Path.Combine(workingLocation, "error.txt"), new string[] { ex.ToString() + item });
                        continue;
                    }
                    

                }
                    triggerAlarm(alarmMessage, 0, 0);
                

            }
            
        }

        private void triggerAlarm(string coinID, double compareVolume, long compareDayTimestamp)
        {
            //MessageBox.Show(coinID + compareVolume.ToString());
            string message = coinID + compareVolume.ToString();
            //message = Encoding.UTF8.GetString(Encoding.Default.GetBytes(message)); //convert to utf8
            new WebClient().DownloadStringAsync(new Uri("https://api.telegram.org/bot494896945:AAHzFs5cguPBWGl2Q1Qlg4pp7-acSJj3490/sendMessage?chat_id=421489390&text=" + message));
        }

        private void logCoinEvent(string coinID, string content)
        {
            File.AppendAllText(Path.Combine(workingLocation, coinID + ".txt"), content + ";" + DateTime.Now.ToString() + "\r\n");
        }

        private void timerHourlyAnalyzer_Tick(object sender, EventArgs e)
        {

            string[] currencyList = null;
            try
            {
                currencyList = File.ReadAllLines(Path.Combine(workingLocation, "index.ini"));
                watchListString = File.ReadAllText(Path.Combine(workingLocation, "watchlist.ini"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            if (currencyList != null)
            {
                string alarmMessage = "Hourly + \r\n";
                string priceAlarmMessage = "Price + \r\n";
                foreach (string item in currencyList)
                {
                    //load data
                    // open the file "data.csv" which is a CSV file with headers
                    try
                    {
                        using (CsvReader csv =
                           new CsvReader(new StreamReader(Path.Combine(dataLocation, item + ".txt")), true))
                        {



                            //string[] headers = csv.GetFieldHeaders();
                            int rowCount = 0;
                            int[] timestampArray = new int[10000];
                            double[] volumesArray = new double[10000];
                            double[] priceArray = new double[10000];
                            long twoDayAgo = (new DateTimeOffset(DateTime.Now.AddHours(-5))).ToUnixTimeSeconds();
                            long oneHourAgo = (new DateTimeOffset(DateTime.Now.AddHours(-1))).ToUnixTimeSeconds();

                            while (csv.ReadNextRecord())
                            {
                                int recordTime = Convert.ToInt32(csv[14]);

                                if (recordTime > twoDayAgo)
                                {
                                    if (recordTime > oneHourAgo)
                                        priceArray[rowCount] = Convert.ToDouble(csv[4]);
                                    volumesArray[rowCount] = Convert.ToDouble(csv[6]);
                                    timestampArray[rowCount] = Convert.ToInt32(csv[14]);
                                    rowCount++;
                                }

                            }


                            //analyzing volume
                            bool isVolumeSupport = false;
                            double increasingRatio = volumesArray[rowCount - 1] / volumesArray.Take(rowCount).Min();
                            if (increasingRatio >= 2 || (volumesArray[rowCount - 1] - volumesArray.Take(rowCount).Min()) > 30000000) // volume incresse 8 times in last 2 days
                            {
                                logCoinEvent(item, "Hourly Volume p: " + Math.Round(increasingRatio, 2) + " u: " + Math.Round(priceArray[rowCount - 1], 3));
                                alarmMessage += item + " v: " + Math.Round(volumesArray[rowCount - 1]).ToString("N") + "  p: " + Math.Round(increasingRatio, 2) + " u: " + Math.Round(priceArray[rowCount - 1], 3) +
                                                                                                                    "\r\n";
                                isVolumeSupport = true;

                                if (watchListString.Contains(item))
                                    alarmMessage += "******* \r\n";

                            }

                            //analyzing price
                            double priceRatio = priceArray[rowCount - 1] / priceArray.Take(rowCount).Where(f => f > 0).Min();
                            if (priceRatio >= 1.05 && volumesArray[rowCount - 1]  > 1500000) // volume incresse 8 times in last 2 days
                            {
                                //logCoinEvent(item, "Price: p: " + Math.Round(priceRatio, 2) + " u: " + Math.Round(priceArray[rowCount - 1], 3));
                                priceAlarmMessage += item + " v: " + Math.Round(volumesArray[rowCount - 1]).ToString("N") + "  p: " + Math.Round(priceRatio, 2) +  " u: " + Math.Round(priceArray[rowCount - 1], 3) +
                                                                                                                        "\r\n";
                                if (isVolumeSupport)
                                    priceAlarmMessage += "**** \r\n";
                                if (watchListString.Contains(item))
                                    priceAlarmMessage += "******* \r\n";
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        File.AppendAllLines(Path.Combine(workingLocation, "error.txt"), new string[] { ex.ToString() + item });
                        continue;
                    }


                }
                triggerAlarm(alarmMessage, 0, 0);
                triggerAlarm(priceAlarmMessage, 0, 0);


                //try
                //{
                //    string eventCount = "";
                //    foreach (var filePath in Directory.GetFiles(workingLocation))
                //    {
                //        eventCount += Path.GetFileNameWithoutExtension(filePath) + ": " + File.ReadLines(filePath).Count() + "\r\n";
                //    }
                //    triggerAlarm(eventCount, 0, 0);
                //}
                //catch (Exception)
                //{

                    
                //}
            }
        }
    }
}
