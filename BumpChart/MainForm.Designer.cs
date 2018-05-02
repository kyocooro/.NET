namespace BarChart
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lsbFileName = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnShowWeb = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnAddtoWatchlist = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.SuspendLayout();
            // 
            // lsbFileName
            // 
            this.lsbFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbFileName.FormattingEnabled = true;
            this.lsbFileName.ItemHeight = 16;
            this.lsbFileName.Location = new System.Drawing.Point(839, 18);
            this.lsbFileName.Margin = new System.Windows.Forms.Padding(2);
            this.lsbFileName.Name = "lsbFileName";
            this.lsbFileName.Size = new System.Drawing.Size(134, 404);
            this.lsbFileName.TabIndex = 2;
            this.lsbFileName.SelectedIndexChanged += new System.EventHandler(this.lsbFileName_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chartMain);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(821, 597);
            this.panel2.TabIndex = 3;
            // 
            // chartMain
            // 
            this.chartMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartMain.ChartAreas.Add(chartArea1);
            this.chartMain.Location = new System.Drawing.Point(8, 10);
            this.chartMain.Margin = new System.Windows.Forms.Padding(2);
            this.chartMain.Name = "chartMain";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMain.Series.Add(series1);
            this.chartMain.Size = new System.Drawing.Size(811, 578);
            this.chartMain.TabIndex = 0;
            this.chartMain.Text = "chart1";
            this.chartMain.Click += new System.EventHandler(this.chart1_Click_1);
            // 
            // btnShowWeb
            // 
            this.btnShowWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowWeb.Location = new System.Drawing.Point(835, 458);
            this.btnShowWeb.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowWeb.Name = "btnShowWeb";
            this.btnShowWeb.Size = new System.Drawing.Size(137, 37);
            this.btnShowWeb.TabIndex = 4;
            this.btnShowWeb.Text = "Show Web";
            this.btnShowWeb.UseVisualStyleBackColor = true;
            this.btnShowWeb.Click += new System.EventHandler(this.btnShowWeb_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(13, 13);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(831, 664);
            this.webBrowser1.TabIndex = 5;
            this.webBrowser1.Visible = false;
            // 
            // btnAddtoWatchlist
            // 
            this.btnAddtoWatchlist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddtoWatchlist.Location = new System.Drawing.Point(839, 513);
            this.btnAddtoWatchlist.Name = "btnAddtoWatchlist";
            this.btnAddtoWatchlist.Size = new System.Drawing.Size(129, 29);
            this.btnAddtoWatchlist.TabIndex = 6;
            this.btnAddtoWatchlist.Text = "Add to Watchlist";
            this.btnAddtoWatchlist.UseVisualStyleBackColor = true;
            this.btnAddtoWatchlist.Click += new System.EventHandler(this.btnAddtoWatchlist_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 679);
            this.Controls.Add(this.btnAddtoWatchlist);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnShowWeb);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lsbFileName);
            this.Name = "MainForm";
            this.Text = "MindFusion.Charting Sample: Bar Chart";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
        private System.Windows.Forms.ListBox lsbFileName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
        private System.Windows.Forms.Button btnShowWeb;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnAddtoWatchlist;
    }
}

