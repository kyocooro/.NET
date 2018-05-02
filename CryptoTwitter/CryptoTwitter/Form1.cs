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
using System.IO;
using System.Net;

namespace CryptoTwitter
{
    public partial class Form1 : Form
    {
        private string dataLocation = "C:\\OneDrive - Ho Chi Minh City University of Technology\\TwitterNews";
        private string watchList = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Auth.SetUserCredentials("WPqEWaD94JiMVcljPLXMC4FVb", "oDUoCWku01L6AeaixwRi3eLylwZDqQDyzjVA4o8l1LfMszVKW7", "929366754119131136-78uuxHTyeBYVqIJeh3wm9iputsY19Ip", "kGDX4ZqZdnGS1wJLbFgO3wzO05rStYsOuObnxqbE2r88h");
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
        
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //foreach (var tweet in Timeline.GetHomeTimeline(1000))
            //{
            //    File.AppendAllText(Path.Combine(dataLocation, tweet.CreatedBy.UserIdentifier.ScreenName + ".txt"), tweet.ToJson() + "\r\n");
            //}
            watchListUpdater.Start();

            var stream = Tweetinvi.Stream.CreateUserStream();

            stream.TweetCreatedByFriend += (tweetSender, args) =>
            {
                var createdTweet = args.Tweet;
                File.AppendAllText(Path.Combine(dataLocation, createdTweet.CreatedBy.UserIdentifier.ScreenName + ".txt"), createdTweet.ToJson() + "\r\n");

                new WebClient().DownloadStringAsync(new Uri("https://api.telegram.org/bot494896945:AAHzFs5cguPBWGl2Q1Qlg4pp7-acSJj3490/sendMessage?chat_id=421489390&text=" + createdTweet.Text + @" https://twitter.com/statuses/" + createdTweet.IdStr));
                //try
                //{
                //    if (watchList.Contains(createdTweet.CreatedBy.UserIdentifier.ScreenName))
                //    {
                //        new WebClient().DownloadStringAsync(new Uri("https://api.telegram.org/bot494896945:AAHzFs5cguPBWGl2Q1Qlg4pp7-acSJj3490/sendMessage?chat_id=421489390&text=" + createdTweet.Text));
                //    }
                //}
                //catch (Exception)
                //{

                  
                //}
            };
            stream.StartStream();
            
        }

        private void watchListUpdater_Tick(object sender, EventArgs e)
        {
            try
            {
                watchList = File.ReadAllText(Path.Combine(dataLocation, "watchlist.txt"));
            }
            catch (Exception)
            {

               
            }
            
        }
    }
}
