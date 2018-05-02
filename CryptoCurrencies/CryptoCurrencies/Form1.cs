using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using System.Net;
using Newtonsoft.Json.Linq;
using ChoETL;
using System.IO;

namespace CryptoCurrencies
{
    public partial class Form1 : Form
    {
        private string dataLocation = "Z:\\CoinData";
        private string backupLocation = "C:\\OneDrive - Ho Chi Minh City University of Technology\\CoinData";
        private string archiveLocation = "C:\\OneDrive - Ho Chi Minh City University of Technology\\CoinData\\archive";
        private string columnHeader = "ID,name,symbol,rank,price_usd,price_btc,last_24h_volume_usd,market_cap_usd,available_supply,total_supply,max_supply,percent_change_1h,percent_change_24h,percent_change_7d,last_updated";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
           // Auth.SetUserCredentials("WPqEWaD94JiMVcljPLXMC4FVb", "oDUoCWku01L6AeaixwRi3eLylwZDqQDyzjVA4o8l1LfMszVKW7", "929366754119131136-78uuxHTyeBYVqIJeh3wm9iputsY19Ip", "kGDX4ZqZdnGS1wJLbFgO3wzO05rStYsOuObnxqbE2r88h");
            coinmarketcapTimer.Start();
            backupTimer.Start();
            //coinmarketcapTimer_Tick(null, null);
        }

        private void coinmarketcapTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string jsonString = new WebClient().DownloadString("https://api.coinmarketcap.com/v1/ticker/?limit=0");
                JArray json = JArray.Parse(jsonString);
                List<Currency> currencyList = new List<Currency>();
                if (json != null)
                {
                    IEnumerable<JToken> jsonItems = json.Children();

                    foreach (JToken jsonItem in jsonItems)
                    {
                        Currency item = new Currency();
                        item.ID = jsonItem["id"].ToString();
                        item.rank = Convert.ToInt32(jsonItem["rank"].ToString());
                        item.symbol = jsonItem["symbol"].ToString();
                        item.name = jsonItem["name"].ToString();
                        try
                        {
                            item.price_usd = Convert.ToDouble(jsonItem["price_usd"].ToString());
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            item.price_btc = Convert.ToDouble(jsonItem["price_btc"].ToString());
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            item.last_24h_volume_usd = Convert.ToDouble(jsonItem["24h_volume_usd"].ToString());
                        }
                        catch (Exception)
                        { }
                        
                       
                        
                        try
                        {
                            item.market_cap_usd = Convert.ToDouble(jsonItem["market_cap_usd"].ToString());
                        }
                        catch (Exception)
                        {


                        }

                        try
                        {
                            item.total_supply = Convert.ToDouble(jsonItem["total_supply"].ToString());
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            item.percent_change_24h = Convert.ToDouble(jsonItem["percent_change_24h"].ToString());

                        }
                        catch (Exception)
                        { }

                        try
                        {
                            item.available_supply = Convert.ToDouble(jsonItem["available_supply"].ToString());

                        }
                        catch (Exception)
                        { }
                        

                        try
                        {
                            item.max_supply = Convert.ToDouble(jsonItem["max_supply"].ToString());

                        }
                        catch (Exception)
                        { }


                        try
                        {
                            item.percent_change_1h = Convert.ToDouble(jsonItem["percent_change_1h"].ToString());
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            item.percent_change_24h = Convert.ToDouble(jsonItem["percent_change_24h"].ToString());
                        }
                        catch (Exception)
                        { }


                        try
                        {
                            item.percent_change_7d = Convert.ToDouble(jsonItem["percent_change_7d"].ToString());
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            item.last_updated = Convert.ToInt32(jsonItem["last_updated"].ToString());
                        }
                        catch (Exception)
                        { }
                        

                        currencyList.Add(item);
                    }

                    writeCurrencyListToDatabase(currencyList);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines(Path.Combine(dataLocation, "error.txt"), new string[] { ex.ToString() });

            }
            


        }

        private List<Currency> tokenDataList(int numberOfLastLine,  string tokenID)
        {
            string tokenDataPath = Path.Combine(dataLocation, tokenID + ".txt");
            List<Currency> dataList = new List<Currency>();
            if (!File.Exists(tokenDataPath)) //if not exit create file wwith header
            {
                File.AppendAllText(tokenDataPath, columnHeader + "\r\n");
                return dataList;
            }

            string lastRecord = ReadEndTokens(tokenDataPath, 1, Encoding.ASCII, "\r\n");
            foreach (var rec in ChoCSVReader<Currency>.LoadText(columnHeader + "\r\n" + lastRecord).WithFirstLineHeader())
                dataList.Add(rec);
            
            return dataList;
        }
        private void writeCurrencyListToDatabase(List<Currency> currencyList)
        {
            
            foreach (Currency token in currencyList)
            {
                string tokenDataPath = Path.Combine(dataLocation, token.ID + ".txt");
                //load last line
                List<Currency> dataList = tokenDataList(1, token.ID);
                if (dataList.Count == 0)
                {
                    //dataList.Add(token);
                    File.AppendAllText(tokenDataPath, ChoCSVWriter<Currency>.ToText(token));
                    //string a = ChoCSVWriter<Currency>.ToTextAll(dataList);
                    //using (var parser = new ChoCSVWriter<Currency>(tokenDataPath).WithFirstLineHeader())
                    //{
                    //    parser.Write(dataList);
                    //}
                }
                else if ((dataList.Last<Currency>()).last_updated < token.last_updated)
                {
                    File.AppendAllText(tokenDataPath, "\r\n" + ChoCSVWriter<Currency>.ToText(token));
                }
                

                
            }
        }

        private void loadSettings()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadSettings();
        }


        public static string ReadEndTokens(string path, Int64 numberOfTokens, Encoding encoding, string tokenSeparator)
        {

            int sizeOfChar = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(tokenSeparator);


            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Int64 tokenCount = 0;
                Int64 endPosition = fs.Length / sizeOfChar;

                for (Int64 position = sizeOfChar; position < endPosition; position += sizeOfChar)
                {
                    fs.Seek(-position, SeekOrigin.End);
                    fs.Read(buffer, 0, buffer.Length);

                    if (encoding.GetString(buffer) == tokenSeparator)
                    {
                        tokenCount++;
                        if (tokenCount == numberOfTokens)
                        {
                            byte[] returnBuffer = new byte[fs.Length - fs.Position];
                            fs.Read(returnBuffer, 0, returnBuffer.Length);
                            return encoding.GetString(returnBuffer);
                        }
                    }
                }

                // handle case where number of tokens in file is less than numberOfTokens
                fs.Seek(0, SeekOrigin.Begin);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return encoding.GetString(buffer);
            }
        }

        private void backupTimer_Tick(object sender, EventArgs e)
        {
                foreach (string newPath in Directory.GetFiles(dataLocation, "*.*",
        SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(dataLocation, backupLocation), true);
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            foreach (string filePath in Directory.GetFiles(dataLocation))
            {
                string[] contents = File.ReadAllLines(filePath);
                if (contents.Count() > 3 )
                {
                    int midIndex = contents.Count() / 2;
                    var firstHalf = contents.Take(midIndex);
                    var secondHalf = contents.Skip(midIndex);

                    if (columnHeader.Equals(firstHalf.First()))
                        firstHalf = firstHalf.Skip(1);

                    string archiveFileLocation = Path.Combine(archiveLocation, Path.GetFileName(filePath));
                    if (!File.Exists(archiveFileLocation))
                        File.AppendAllText(archiveFileLocation, columnHeader + "\r\n");

                    File.AppendAllLines(archiveFileLocation, firstHalf);
                    File.WriteAllText(filePath, columnHeader + "\r\n");
                    File.AppendAllLines(filePath, secondHalf);
                }
            }
        }
    }

}
