using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChoETL;

namespace CryptoCurrencies
{
    
    public class Currency
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int rank { get; set; }
        public double price_usd { get; set; }
        public double price_btc { get; set; }
        public double last_24h_volume_usd { get; set; }
        public double market_cap_usd { get; set; }
        public double available_supply { get; set; }
        public double total_supply { get; set; }
        public double max_supply { get; set; }
        public double percent_change_1h { get; set; }
        public double percent_change_24h { get; set; }
        public double percent_change_7d { get; set; }
        public int last_updated { get; set; }
    }
}
