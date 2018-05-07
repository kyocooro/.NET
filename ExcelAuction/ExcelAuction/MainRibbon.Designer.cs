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
            this.btnChrome = this.Factory.CreateRibbonButton();
            this.btnCheckInput = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.btnShowQA = this.Factory.CreateRibbonButton();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.btnDate = this.Factory.CreateRibbonButton();
            this.btnNextDate = this.Factory.CreateRibbonButton();
            this.btnPrevDate = this.Factory.CreateRibbonButton();
            this.group5 = this.Factory.CreateRibbonGroup();
            this.btnClear = this.Factory.CreateRibbonButton();
            this.btnPayKantan = this.Factory.CreateRibbonButton();
            this.btnSendFirstMessage = this.Factory.CreateRibbonButton();
            this.group6 = this.Factory.CreateRibbonGroup();
            this.btnExportQA = this.Factory.CreateRibbonButton();
            this.btnExportChaoHoi = this.Factory.CreateRibbonButton();
            this.btnExportCommon = this.Factory.CreateRibbonButton();
            this.group7 = this.Factory.CreateRibbonGroup();
            this.btnGetPaymentInfo = this.Factory.CreateRibbonButton();
            this.btnGetQA = this.Factory.CreateRibbonButton();
            this.btnSendPayment = this.Factory.CreateRibbonButton();
            this.group8 = this.Factory.CreateRibbonGroup();
            this.btnDownload = this.Factory.CreateRibbonButton();
            this.btnShowInfo = this.Factory.CreateRibbonButton();
            this.btnExport = this.Factory.CreateRibbonButton();
            this.Misc = this.Factory.CreateRibbonGroup();
            this.cbxSentPaymentNotify = this.Factory.CreateRibbonCheckBox();
            this.btnSellerInfo = this.Factory.CreateRibbonButton();
            this.btnGetCat = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            this.group5.SuspendLayout();
            this.group6.SuspendLayout();
            this.group7.SuspendLayout();
            this.group8.SuspendLayout();
            this.Misc.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Groups.Add(this.group4);
            this.tab1.Groups.Add(this.group5);
            this.tab1.Groups.Add(this.group6);
            this.tab1.Groups.Add(this.group7);
            this.tab1.Groups.Add(this.group8);
            this.tab1.Groups.Add(this.Misc);
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
            this.group2.Items.Add(this.btnChrome);
            this.group2.Items.Add(this.btnCheckInput);
            this.group2.Label = "Tools";
            this.group2.Name = "group2";
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Label = "Open in Firefox";
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOpenBrowser_Click);
            // 
            // btnChrome
            // 
            this.btnChrome.Label = "Open In Chrome";
            this.btnChrome.Name = "btnChrome";
            this.btnChrome.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnChrome_Click);
            // 
            // btnCheckInput
            // 
            this.btnCheckInput.Label = "Check Input";
            this.btnCheckInput.Name = "btnCheckInput";
            this.btnCheckInput.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCheckInput_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.btnShowQA);
            this.group3.Label = "group3";
            this.group3.Name = "group3";
            // 
            // btnShowQA
            // 
            this.btnShowQA.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnShowQA.Label = "Show QA";
            this.btnShowQA.Name = "btnShowQA";
            this.btnShowQA.ShowImage = true;
            this.btnShowQA.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowQA_Click);
            // 
            // group4
            // 
            this.group4.Items.Add(this.btnDate);
            this.group4.Items.Add(this.btnNextDate);
            this.group4.Items.Add(this.btnPrevDate);
            this.group4.Label = "group4";
            this.group4.Name = "group4";
            // 
            // btnDate
            // 
            this.btnDate.Label = "Insert Today";
            this.btnDate.Name = "btnDate";
            this.btnDate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDate_Click);
            // 
            // btnNextDate
            // 
            this.btnNextDate.Label = "Insert Tommorow";
            this.btnNextDate.Name = "btnNextDate";
            this.btnNextDate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnNextDate_Click);
            // 
            // btnPrevDate
            // 
            this.btnPrevDate.Label = "Insert Yesterday";
            this.btnPrevDate.Name = "btnPrevDate";
            this.btnPrevDate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPrevDate_Click);
            // 
            // group5
            // 
            this.group5.Items.Add(this.btnClear);
            this.group5.Items.Add(this.btnPayKantan);
            this.group5.Items.Add(this.btnSendFirstMessage);
            this.group5.Label = "group5";
            this.group5.Name = "group5";
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Label = "Clear";
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnClear_Click);
            // 
            // btnPayKantan
            // 
            this.btnPayKantan.Label = "Pay Kantan";
            this.btnPayKantan.Name = "btnPayKantan";
            this.btnPayKantan.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPayKantan_Click);
            // 
            // btnSendFirstMessage
            // 
            this.btnSendFirstMessage.Label = "Insert Form";
            this.btnSendFirstMessage.Name = "btnSendFirstMessage";
            this.btnSendFirstMessage.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // group6
            // 
            this.group6.Items.Add(this.btnExportQA);
            this.group6.Items.Add(this.btnExportChaoHoi);
            this.group6.Items.Add(this.btnExportCommon);
            this.group6.Label = "Export";
            this.group6.Name = "group6";
            // 
            // btnExportQA
            // 
            this.btnExportQA.Label = "Export To LayQA";
            this.btnExportQA.Name = "btnExportQA";
            this.btnExportQA.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExportQA_Click);
            // 
            // btnExportChaoHoi
            // 
            this.btnExportChaoHoi.Label = "Export To ChaoHoi";
            this.btnExportChaoHoi.Name = "btnExportChaoHoi";
            this.btnExportChaoHoi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExportChaoHoi_Click);
            // 
            // btnExportCommon
            // 
            this.btnExportCommon.Label = "Export To Chung";
            this.btnExportCommon.Name = "btnExportCommon";
            this.btnExportCommon.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExportCommon_Click);
            // 
            // group7
            // 
            this.group7.Items.Add(this.btnGetPaymentInfo);
            this.group7.Items.Add(this.btnGetQA);
            this.group7.Items.Add(this.btnSendPayment);
            this.group7.Label = "QA";
            this.group7.Name = "group7";
            // 
            // btnGetPaymentInfo
            // 
            this.btnGetPaymentInfo.Label = "Get Payment Info";
            this.btnGetPaymentInfo.Name = "btnGetPaymentInfo";
            this.btnGetPaymentInfo.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGetPaymentInfo_Click);
            // 
            // btnGetQA
            // 
            this.btnGetQA.Label = "Get QA";
            this.btnGetQA.Name = "btnGetQA";
            this.btnGetQA.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGetQA_Click);
            // 
            // btnSendPayment
            // 
            this.btnSendPayment.Label = "Sent Money";
            this.btnSendPayment.Name = "btnSendPayment";
            this.btnSendPayment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSendPayment_Click);
            // 
            // group8
            // 
            this.group8.Items.Add(this.btnDownload);
            this.group8.Items.Add(this.btnShowInfo);
            this.group8.Items.Add(this.btnExport);
            this.group8.Label = "HauHien";
            this.group8.Name = "group8";
            // 
            // btnDownload
            // 
            this.btnDownload.Label = "Tải Thông Tin";
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDownload_Click);
            // 
            // btnShowInfo
            // 
            this.btnShowInfo.Label = "Hiện Thông Tin";
            this.btnShowInfo.Name = "btnShowInfo";
            this.btnShowInfo.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowInfo_Click);
            // 
            // btnExport
            // 
            this.btnExport.Label = "Xuất Thông Tin";
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExport_Click);
            // 
            // Misc
            // 
            this.Misc.Items.Add(this.cbxSentPaymentNotify);
            this.Misc.Items.Add(this.btnSellerInfo);
            this.Misc.Items.Add(this.btnGetCat);
            this.Misc.Items.Add(this.button1);
            this.Misc.Label = "Misc";
            this.Misc.Name = "Misc";
            // 
            // cbxSentPaymentNotify
            // 
            this.cbxSentPaymentNotify.Checked = true;
            this.cbxSentPaymentNotify.Label = "Sent Payment Notify";
            this.cbxSentPaymentNotify.Name = "cbxSentPaymentNotify";
            this.cbxSentPaymentNotify.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.cbxSentPaymentNotify_Click);
            // 
            // btnSellerInfo
            // 
            this.btnSellerInfo.Label = "Get Seller Info";
            this.btnSellerInfo.Name = "btnSellerInfo";
            this.btnSellerInfo.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSellerInfo_Click);
            // 
            // btnGetCat
            // 
            this.btnGetCat.Label = "Get Category";
            this.btnGetCat.Name = "btnGetCat";
            this.btnGetCat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGetCat_Click);
            // 
            // button1
            // 
            this.button1.Label = "button1";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click_1);
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
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();
            this.group5.ResumeLayout(false);
            this.group5.PerformLayout();
            this.group6.ResumeLayout(false);
            this.group6.PerformLayout();
            this.group7.ResumeLayout(false);
            this.group7.PerformLayout();
            this.group8.ResumeLayout(false);
            this.group8.PerformLayout();
            this.Misc.ResumeLayout(false);
            this.Misc.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLogin;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGetLast;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOpenBrowser;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLogout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCheckInput;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnShowQA;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnChrome;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDate;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPrevDate;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnNextDate;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group5;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnClear;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPayKantan;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group6;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExportQA;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExportChaoHoi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExportCommon;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSendFirstMessage;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group7;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGetPaymentInfo;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGetQA;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSendPayment;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group8;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDownload;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnShowInfo;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExport;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Misc;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox cbxSentPaymentNotify;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSellerInfo;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGetCat;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
    }

    partial class ThisRibbonCollection
    {
        internal MainRibbon MainRibbon
        {
            get { return this.GetRibbon<MainRibbon>(); }
        }
    }
}
