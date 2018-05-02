using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Binance.Net;
using Binance.Net.Objects;
using Trady.Core;
using Trady.Analysis;
using Trady.Analysis.Indicator;
namespace BinanceMonitor
{
    public partial class Form1 : Form
    {

        string api = "oYa8mcIv2eZhbV6aGBBYG5VBUrSKN8G2MG0dEDnpoFo+Ln5kapHKvhTYsFI6mHJr";
        string apiKey = "srm8ouLJP/0ym2eg8xKNHd9yYO/P8vxtv7oQbFDvP2Jh5s/Ul/YGRP6Phclb5plz";
        BinanceClient client = new BinanceClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            //oneHourTimer.Start();
            oneHourTimer_Tick(null, null);
        }

        private void oneHourTimer_Tick(object sender, EventArgs e)
        {
            //get all pair
            var result = client.Get24HPricesList();
            Binance24HPrice[] pairs = null;
            if (result.Success)
            {
                pairs = result.Data;
                //Thread.Sleep(60000);
            }
            else
                Console.WriteLine($"Error: {result.Error.Message}");

            if (pairs != null)
            {
                foreach (Binance24HPrice pair in pairs)
                {
                    if (pair.Symbol.Contains("BTC") && pair.Volume > 2000)
                    {
                        var candleResult = client.GetKlines(pair.Symbol, KlineInterval.OneHour, null, null, 30);
                        if (candleResult.Success)
                        {
                            List<decimal> closes = new List<decimal>();

                            foreach (BinanceKline candle in candleResult.Data)
                            {
                                closes.Add(candle.Close);
                            }

                            var bollingerBands = new BollingerBandsByTuple(closes, 20, 2);
                            var  b = bollingerBands[29].UpperBand;
                            int a = 0;
                        }
                        else
                            Console.WriteLine($"Error: {result.Error.Message}");
                    }
                }
            }
        }
    }


}
