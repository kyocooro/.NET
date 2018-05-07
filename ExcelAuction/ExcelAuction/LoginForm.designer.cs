namespace ExcelAuction
{
    partial class LoginForm
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
            this.webAuth = new System.Windows.Forms.WebBrowser();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webAuth
            // 
            this.webAuth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webAuth.Location = new System.Drawing.Point(12, 12);
            this.webAuth.MinimumSize = new System.Drawing.Size(20, 20);
            this.webAuth.Name = "webAuth";
            this.webAuth.Size = new System.Drawing.Size(705, 375);
            this.webAuth.TabIndex = 1;
            this.webAuth.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webAuth_DocumentCompleted);
            this.webAuth.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webAuth_Navigated);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 393);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 33);
            this.button1.TabIndex = 2;
            this.button1.Tag = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(96, 393);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 33);
            this.button2.TabIndex = 3;
            this.button2.Tag = "1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(180, 393);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 33);
            this.button3.TabIndex = 4;
            this.button3.Tag = "1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(264, 393);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 33);
            this.button4.TabIndex = 5;
            this.button4.Tag = "1";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(348, 393);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(78, 33);
            this.button5.TabIndex = 6;
            this.button5.Tag = "1";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(432, 393);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(78, 33);
            this.button6.TabIndex = 7;
            this.button6.Tag = "1";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(516, 393);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(78, 33);
            this.button7.TabIndex = 8;
            this.button7.Tag = "1";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(600, 393);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(78, 33);
            this.button8.TabIndex = 9;
            this.button8.Tag = "1";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 438);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.webAuth);
            this.Name = "LoginForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webAuth;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}

