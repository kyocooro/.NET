using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelAuction
{
    public partial class AuctionInfoForm : Form
    {
        public string itemID;
        public AuctionInfoForm()
        {
            InitializeComponent();
        }

        private void AuctionInfoForm_Load(object sender, EventArgs e)
        {
            string applicationPath = "C:\\Auction\\Info\\";
            //DateTime endtime = Convert.ToDateTime(jsonInfo["EndTime"]);
            //String endDate = endtime.ToShortDateString().Replace('/', '_');
            string itemFolder = Path.Combine(applicationPath, itemID);

            //picture
            try
            {
                pictureBox1.Load(Path.Combine(itemFolder, "1.jpg"));
                pictureBox4.Load(Path.Combine(itemFolder, "1.jpg"));
                pictureBox2.Load(Path.Combine(itemFolder, "2.jpg"));
                pictureBox3.Load(Path.Combine(itemFolder, "3.jpg"));
            }
            catch (Exception)
            {

            }


            //info
            try
            {
                JObject infoJson = JObject.Parse(File.ReadAllText(Path.Combine(itemFolder, "info.txt")));
                lblName.Text = infoJson.GetValue("Title").ToString();
            }
            catch (Exception)
            {
                lblName.Text = "Chưa tải thông tin";
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = pictureBox1.Image;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = pictureBox3.Image;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = pictureBox2.Image;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
