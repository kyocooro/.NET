namespace ExcelAuction
{
    partial class MainRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MainRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnLogin = this.Factory.CreateRibbonButton();
            this.btnGetLast = this.Factory.CreateRibbonButton();
            this.btnLogout = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btnOpenBrowser = this.Factory.CreateRibbonButton();
            this.btnLoadImage = this.Factory.CreateRibbonButton();
            this.btnShowQA = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Yahoo Auction";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnLogin);
            this.group1.Items.Add(this.btnGetLast);
            this.group1.Items.Add(this.btnLogout);
            this.group1.Label = "Login";
            this.group1.Name = "group1";
            // 
            // btnLogin
            // 
            this.btnLogin.Label = "Login";
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnLogin_Click);
            // 
            // btnGetLast
            // 
            this.btnGetLast.Label = "Get Last ID";
            this.btnGetLast.Name = "btnGetLast";
            this.btnGetLast.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGetLast_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Label = "Logout";
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnLogout_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnOpenBrowser);
            this.group2.Items.Add(this.btnLoadImage);
            this.group2.Items.Add(this.btnShowQA);
            this.group2.Label = "Tools";
            this.group2.Name = "group2";
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Label = "Open in Firefox";
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOpenBrowser_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Label = "Load Image";
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnLoadImage_Click);
            // 
            // btnShowQA
            // 
            this.btnShowQA.Label = "Show QA";
            this.btnShowQA.Name = "btnShowQA";
            this.btnShowQA.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowQA_Click);
            // 
            // MainRibbon
            // 
            this.Name = "MainRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MainRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLogin;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGetLast;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOpenBrowser;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLogout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLoadImage;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnShowQA;
    }

    partial class ThisRibbonCollection
    {
        internal MainRibbon MainRibbon
        {
            get { return this.GetRibbon<MainRibbon>(); }
        }
    }
}
