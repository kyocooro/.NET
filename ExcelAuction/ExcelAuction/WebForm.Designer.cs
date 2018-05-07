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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(692, 733);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser_ProgressChanged);
            // 
            // btnFillAccount
            // 
            this.btnFillAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFillAccount.Location = new System.Drawing.Point(698, 27);
            this.btnFillAccount.Name = "btnFillAccount";
            this.btnFillAccount.Size = new System.Drawing.Size(75, 23);
            this.btnFillAccount.TabIndex = 1;
            this.btnFillAccount.Text = "Fill Account";
            this.btnFillAccount.UseVisualStyleBackColor = true;
            this.btnFillAccount.Click += new System.EventHandler(this.btnFillAccount_Click);
            // 
            // btnFillMoney
            // 
            this.btnFillMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFillMoney.Location = new System.Drawing.Point(698, 66);
            this.btnFillMoney.Name = "btnFillMoney";
            this.btnFillMoney.Size = new System.Drawing.Size(75, 23);
            this.btnFillMoney.TabIndex = 2;
            this.btnFillMoney.Text = "Fill Money";
            this.btnFillMoney.UseVisualStyleBackColor = true;
            this.btnFillMoney.Click += new System.EventHandler(this.btnFillMoney_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(697, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 733);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFillMoney);
            this.Controls.Add(this.btnFillAccount);
            this.Controls.Add(this.webBrowser);
            this.Location = new System.Drawing.Point(600, 0);
            this.MaximumSize = new System.Drawing.Size(800, 4000);
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
        private System.Windows.Forms.Button button1;
    }
}