using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace BarChart
{
	public partial class MainForm : Form
	{
        private string workingLocation = "C:\\OneDrive - Ho Chi Minh City University of Technology\\BumpTime";
        public MainForm()
		{
			InitializeComponent();
            chartMain.Series[0].XValueType = ChartValueType.DateTime;
            chartMain.ChartAreas[0].AxisX.LabelStyle.Format = "M:d HH";
            this.WindowState = FormWindowState.Maximized;
            //load data
            foreach (var files in Directory.GetFiles(workingLocation, "*.txt"))
            {
                
                lsbFileName.Items.Add(Path.GetFileName(files));

            }


		}

        public DateTime RoundTo6Hours(DateTime input)
        {
            //input = input.AddHours(7);
            //int roundTime = input.Hour / 6 * 6 ; //round by 6hour per
            //DateTime dt = new DateTime(input.Year, input.Month, input.Day, roundTime, 0, 0);
            //return dt;
            DateTime dt = new DateTime(input.Year, input.Month, input.Day, input.Hour, 0, 0);

            if (input.Minute > 29)
                return dt.AddHours(1);
            else
                return dt;
            
        }

        private void lsbFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Visible = false;
                List<double> eventList = new List<double>();
                foreach (string line in File.ReadAllLines(Path.Combine(workingLocation, lsbFileName.SelectedItem.ToString())))
                {
                    DateTime eventTime = RoundTo6Hours(DateTime.Parse(line.Split(';').Last()));
                    eventList.Add(eventTime.ToOADate());
                    
                }


                chartMain.Series[0].Points.Clear();
                var groups = eventList.GroupBy(v => v);
                
                foreach (var group in groups)
                {
                    chartMain.Series[0].Points.AddXY(group.Key, group.Count());
                }

                // create sample data series

            }
            catch (Exception ex)
            {

                
            }
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnShowWeb_Click(object sender, EventArgs e)
        {
            
            webBrowser1.Visible = !webBrowser1.Visible;
            if (webBrowser1.Visible)
            {
                try
                {
                    webBrowser1.Navigate(@"https://coinmarketcap.com/currencies/" + Path.GetFileNameWithoutExtension(lsbFileName.SelectedItem.ToString()));
                }
                catch (Exception)
                {

                    
                }
                
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //chartMain.Size = new Size(this.Size.Width - 250, this.Size.Height);
                webBrowser1.Size = new Size(this.Size.Width - 250, this.Size.Height);
            }
            catch (Exception)
            {

                
            }
            
        }

        private void btnAddtoWatchlist_Click(object sender, EventArgs e)
        {
            try
            {
                File.AppendAllLines(Path.Combine(workingLocation, "watchlist.ini"), new string[] { Path.GetFileNameWithoutExtension(lsbFileName.SelectedItem.ToString()) });
            }
            catch (Exception)
            {

                
            }
        }
    }
}