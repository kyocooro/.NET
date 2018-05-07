namespace ExcelAuction
{
    partial class WebForm
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.btnFillAccount = new System.Windows.Forms.Button();
            this.btnFillMoney = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(641, 804);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // btnFillAccount
            // 
            this.btnFillAccount.Location = new System.Drawing.Point(647, 38);
            this.btnFillAccount.Name = "btnFillAccount";
            this.btnFillAccount.Size = new System.Drawing.Size(75, 23);
            this.btnFillAccount.TabIndex = 1;
            this.btnFillAccount.Text = "Fill Account";
            this.btnFillAccount.UseVisualStyleBackColor = true;
            this.btnFillAccount.Click += new System.EventHandler(this.btnFillAccount_Click);
            // 
            // btnFillMoney
            // 
            this.btnFillMoney.Location = new System.Drawing.Point(647, 80);
            this.btnFillMoney.Name = "btnFillMoney";
            this.btnFillMoney.Size = new System.Drawing.Size(75, 23);
            this.btnFillMoney.TabIndex = 2;
            this.btnFillMoney.Text = "Fill Money";
            this.btnFillMoney.UseVisualStyleBackColor = true;
            this.btnFillMoney.Click += new System.EventHandler(this.btnFillMoney_Click);
            // 
            // WebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 804);
            this.Controls.Add(this.btnFillMoney);
            this.Controls.Add(this.btnFillAccount);
            this.Controls.Add(this.webBrowser);
            this.Location = new System.Drawing.Point(700, 0);
            this.Name = "WebForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "abc";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WebForm_FormClosed);
            this.Load += new System.EventHandler(this.WebForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WebForm_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Button btnFillAccount;
        private System.Windows.Forms.Button btnFillMoney;
    }
}