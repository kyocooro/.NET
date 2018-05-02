namespace BumpAnalyzer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnStartMonitor = new System.Windows.Forms.Button();
            this.timerIndexUpdator = new System.Windows.Forms.Timer(this.components);
            this.timerAnalyzer = new System.Windows.Forms.Timer(this.components);
            this.timerHourlyAnalyzer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnStartMonitor
            // 
            this.btnStartMonitor.Location = new System.Drawing.Point(54, 42);
            this.btnStartMonitor.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(67, 34);
            this.btnStartMonitor.TabIndex = 0;
            this.btnStartMonitor.Text = "Monitor";
            this.btnStartMonitor.UseVisualStyleBackColor = true;
            this.btnStartMonitor.Click += new System.EventHandler(this.btnStartMonitor_Click);
            // 
            // timerIndexUpdator
            // 
            this.timerIndexUpdator.Interval = 3600000;
            this.timerIndexUpdator.Tick += new System.EventHandler(this.timerIndexUpdator_Tick);
            // 
            // timerAnalyzer
            // 
            this.timerAnalyzer.Interval = 3600000;
            this.timerAnalyzer.Tick += new System.EventHandler(this.timerAnalyzer_Tick);
            // 
            // timerHourlyAnalyzer
            // 
            this.timerHourlyAnalyzer.Interval = 1800000;
            this.timerHourlyAnalyzer.Tick += new System.EventHandler(this.timerHourlyAnalyzer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 159);
            this.Controls.Add(this.btnStartMonitor);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "BumpAnalyzer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartMonitor;
        private System.Windows.Forms.Timer timerIndexUpdator;
        private System.Windows.Forms.Timer timerAnalyzer;
        private System.Windows.Forms.Timer timerHourlyAnalyzer;
    }
}

