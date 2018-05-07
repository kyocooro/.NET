using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.IO;
using ExcelAuction.WebbrowserHandler;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ExcelAuction
{
    public partial class MainRibbon
    {
        private WebForm qaForm = null;
        
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            
            

        }

        private void btnLogin_Click(object sender, RibbonControlEventArgs e)
        {
            LoginForm form = new LoginForm();
            form.ShowDialog();
        }


        private List<YahooItem> getNewIDFrom(string lastID)
        {
            int total = 1, end = 1;
            int page = 0;
            List<YahooItem> newYahooItems = new List<YahooItem>();
            while (total >= end)
            {
                page++;
                List<YahooItem> itemsID = ExcelAuction.Global.client.GetSuccessfulItemsDetailAtPage(page, ref total, ref end);
                foreach (YahooItem item in itemsID)
                {
                    if (item.ID.Equals(lastID))
                        return newYahooItems;
                    else
                        newYahooItems.Add(item);
                }
            }
            return newYahooItems;
        }

        public string getCurrentBuyerAddress()
        {
            return Properties.Settings.Default["Address"].ToString();
        }

        public string getCurrentBuyerAddress(String info)
        {
            return string.Format(Properties.Settings.Default["Address"].ToString(), info);
        }
        private void btnGetLast_Click(object sender, RibbonControlEventArgs e)
        {
            Range firstCell = Globals.ThisAddIn.Application.get_Range("A2");
            String lastID = "";
            if (firstCell.Value2 != null)
                lastID = firstCell.Value2.ToString();
            List<YahooItem> newItems = getNewIDFrom(lastID);
            newItems.Reverse();
            foreach (YahooItem item in newItems)
            {
                Globals.ThisAddIn.Application.get_Range("A2").EntireRow.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
                Globals.ThisAddIn.Application.get_Range("A2").Value2 = item.ID;
                Globals.ThisAddIn.Application.get_Range("E2").Value2 = item.name.Normalize(NormalizationForm.FormKC);
                Globals.ThisAddIn.Application.get_Range("G2").Value2 = item.endPrice;
                Globals.ThisAddIn.Application.get_Range("H2").Value2 = item.seller;
                if (item.isStore)
                    Globals.ThisAddIn.Application.get_Range("H2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);
                if (item.isKantan)
                    Globals.ThisAddIn.Application.get_Range("A2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                Globals.ThisAddIn.Application.get_Range("I2").Value2 = item.endDate;
            }

            //Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet;
            //activeSheet.Rows[0].Insert(XlInsertShiftDirection.xlShiftDown, null);
        }

        private void btnOpenBrowser_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                String autionID = selectedCell.Value2.ToString();
                System.Diagnostics.Process.Start("firefox", @"http://page.auctions.yahoo.co.jp/jp/auction/" + autionID);
            }
        }

        private void btnLogout_Click(object sender, RibbonControlEventArgs e)
        {
            LoginForm form = new LoginForm();
            form.ShowDialog();
            form.logout();
        }

        private void btnCheckInput_Click(object sender, RibbonControlEventArgs e)
        {
            String lastID = "";
            List<YahooItem> newItems = getNewIDFrom(lastID);
            newItems.Reverse();
            foreach (YahooItem item in newItems)
            {
                for (int i = 1; i < 5000; i++ )
                {
                    if (item.isStore)
                        break;
                    Range cell = Globals.ThisAddIn.Application.get_Range("A" + Convert.ToString(i));
                    if (item.ID.Equals(cell.Value2))
                    {
                        
                        if ("address_inputing".Equals(item.progress))
                        {
                            cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                        else
                            cell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        break;
                    }
                }
                    
            }
        }

        private void btnShowQA_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                String autionID = Globals.ThisAddIn.Application.get_Range("A" + Convert.ToString(selectedCell.Row)).Value2.ToString();
                String qaPath = ExcelAuction.Global.storeLocation + "QA\\" + autionID + ".html";
                if (File.Exists(qaPath))
                {
                    if (qaForm == null || qaForm.isClosed)
                        qaForm = new WebForm();
                    qaForm.Text = autionID;
                    qaForm.showURL(new System.Uri(qaPath));
                    qaForm.Show();
                    qaForm.BringToFront();
                }

            }
        }

        private void btnChrome_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                String autionID = selectedCell.Value2.ToString();
                System.Diagnostics.Process.Start("chrome", @"http://page.auctions.yahoo.co.jp/jp/auction/" + autionID);
            }
        }

        private void btnDate_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                Range payDateRange = Globals.ThisAddIn.Application.get_Range("B" + Convert.ToString(selectedCell.Row));
                payDateRange.Value2 = DateTime.Today.ToString("dd/M/yyyy");
            }
        }

        private void btnPrevDate_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                Range payDateRange = Globals.ThisAddIn.Application.get_Range("B" + Convert.ToString(selectedCell.Row));
                payDateRange.Value2 = DateTime.Now.AddDays(-1).ToString("dd/M/yyyy");
            }

        }

        private void btnNextDate_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                Range payDateRange = Globals.ThisAddIn.Application.get_Range("B" + Convert.ToString(selectedCell.Row));
                payDateRange.Value2 = DateTime.Now.AddDays(1).ToString("dd/M/yyyy");
            }
        }

        private void btnClear_Click(object sender, RibbonControlEventArgs e)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            foreach (Range cell in selection.Cells)
            {
                try
                {
                    File.Delete(ExcelAuction.Global.storeLocation + "QA\\" + cell.Value2.ToString() + ".html");
                    File.Delete(ExcelAuction.Global.storeLocation + "Kantan\\" + cell.Value2.ToString() + ".html");
                }
                catch
                {

                }
            }
        }

        private void btnPayKantan_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                PayKantanManager manager = new PayKantanManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range priceCell = Globals.ThisAddIn.Application.get_Range("D" + cell.Row.ToString());

                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());
                    Range idCell = Globals.ThisAddIn.Application.get_Range("A" + cell.Row.ToString());

                    if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                        item.isStore = true;
                    if (sellerCell.Font.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))
                        item.isNewSystem = true;
                    


                    item.ID = idCell.Value2.ToString();
                    if (priceCell.Value2 == null)
                        item.payPrice = null;
                    else
                        item.payPrice = priceCell.Value2.ToString();
                    item.seller = sellerCell.Value2.ToString();

                    manager.itemsInfo.Insert(0, item);
                }
                manager.payKantan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnExportQA_Click(object sender, RibbonControlEventArgs e)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);

            string writeData = "";
            foreach (Range cell in selection.Cells)
            {
                try
                {
                    writeData += cell.Value2.ToString() + "\r\n";
                }
                catch
                {

                }
            }

            System.IO.File.WriteAllText(@"D:\Auction\LayQA.txt", writeData.TrimEnd('\r', '\n'));
        }

        private void btnExportChaoHoi_Click(object sender, RibbonControlEventArgs e)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);
            string writeData = "";
            foreach (Range cell in selection.Cells)
            {
                try
                {
                    writeData += cell.Value2.ToString() + "\r\n";
                }
                catch
                {

                }
            }

            System.IO.File.WriteAllText(@"D:\Auction\ChaoHoi.txt", writeData.TrimEnd('\r', '\n'));
        }

        private void btnExportCommon_Click(object sender, RibbonControlEventArgs e)
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range);
            if (selection.Cells.Count > 1) selection = selection.SpecialCells(XlCellType.xlCellTypeVisible); 
            string writeData = "";
            foreach (Range cell in selection.Cells)
            {
                try
                {
                    writeData += cell.Value2.ToString() + "\r\n";
                }
                catch
                {

                }
            }

            System.IO.File.WriteAllText(@"C:\Auction\Chung.txt", writeData.TrimEnd('\r', '\n'));
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                SendFirstMessageManager manager = new SendFirstMessageManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());

                    if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                        item.isStore = true;
                    item.ID = cell.Value2.ToString();
                    item.seller = sellerCell.Value2.ToString();

                    manager.itemsInfo.Insert(0, item);
                }

                DialogResult dialogResult = (new BuyerDialog()).ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    manager.SendFirstMsg();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnGetPaymentInfo_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                GetPaymentManager manager = new GetPaymentManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    item.ID = cell.Value2.ToString();
                    manager.itemsInfo.Insert(0, item);
                }
                manager.getPayment();
            }
            catch
            {

            }
        }

        private void btnGetQA_Click(object sender, RibbonControlEventArgs e)
        {
            
            try
            {
                GetQAManager manager = new GetQAManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());
                    Range idCell = Globals.ThisAddIn.Application.get_Range("A" + cell.Row.ToString());

                    if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                        item.isStore = true;
                    item.ID = idCell.Value2.ToString();
                    item.seller = sellerCell.Value2.ToString();

                    manager.itemsInfo.Insert(0, item);
                }
                manager.getQA();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSendPayment_Click(object sender, RibbonControlEventArgs e)
        {
            
            SendPaymentMsgManager manager = new SendPaymentMsgManager();
            try
            {
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());
                    Range idCell = Globals.ThisAddIn.Application.get_Range("A" + cell.Row.ToString());

                    if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                        item.isStore = true;
                    if (sellerCell.Font.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))
                        item.isNewSystem = true;

                    item.ID = idCell.Value2.ToString();
                    item.seller = sellerCell.Value2.ToString();

                    manager.itemsInfo.Insert(0, item);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            manager.SendPaymentMsg();


        }

        private void btnDownload_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    downloadAuctionItemInfo(cell.Value2.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("Tải Xong");
        }


        private void downloadAuctionItemInfo(string id)
        {
            string applicationPath = ExcelAuction.Global.storeLocation +  "Info\\";
            //DateTime endtime = Convert.ToDateTime(jsonInfo["EndTime"]);
            //String endDate = endtime.ToShortDateString().Replace('/', '_');
            string itemFolder = Path.Combine(applicationPath, id);

            if (!Directory.Exists(itemFolder))
            {
                JToken jsonInfo = ExcelAuction.Global.client.GetJsonItemInfo(id);
                if (jsonInfo == null)
                    return;
                Directory.CreateDirectory(itemFolder);

                File.WriteAllText(Path.Combine(itemFolder, "info.txt"), jsonInfo.ToString());

                WebClient webClient = new WebClient();
                try
                {
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image1"], Path.Combine(itemFolder, "1.jpg"));
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image2"], Path.Combine(itemFolder, "2.jpg"));
                    webClient.DownloadFile((string)jsonInfo["Img"]["Image3"], Path.Combine(itemFolder, "3.jpg"));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }

            }

        }

        private void btnShowInfo_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    AuctionInfoForm form = new AuctionInfoForm();
                    form.itemID = cell.Value2.ToString();
                    form.Show();
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void btnExport_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range nameCell = Globals.ThisAddIn.Application.get_Range("E" + cell.Row.ToString());
                    item.ID = cell.Value2.ToString();
                    item.name = nameCell.Value2.ToString();

                    string applicationPath = ExcelAuction.Global.storeLocation + "Info\\";
                    //DateTime endtime = Convert.ToDateTime(jsonInfo["EndTime"]);
                    //String endDate = endtime.ToShortDateString().Replace('/', '_');
                    string itemFolder = Path.Combine(applicationPath, item.ID);

                    if (Directory.Exists(itemFolder))
                    {
                        string copyPath = ExcelAuction.Global.storeLocation +  "Copy\\";
                        string copyFolder = Path.Combine(copyPath, item.ID + "__" + String.Join("", item.name.Split(Path.GetInvalidFileNameChars())));
                        Directory.CreateDirectory(copyFolder);
                        //picture
                        try
                        {
                            File.Copy(Path.Combine(itemFolder, "1.jpg"), Path.Combine(copyFolder, "1.jpg"));
                            File.Copy(Path.Combine(itemFolder, "2.jpg"), Path.Combine(copyFolder, "2.jpg"));
                            File.Copy(Path.Combine(itemFolder, "3.jpg"), Path.Combine(copyFolder, "3.jpg"));
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Chưa tải " + item.ID);
                    }

                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       

        private void drdBuyer_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            Properties.Settings.Default["AddressIndex"] = ((RibbonDropDown)sender).SelectedItemIndex;
            Properties.Settings.Default.Save();
        }

        private void btnSellerInfo_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                GetSellerInfoManager manager = new GetSellerInfoManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("H" + cell.Row.ToString());
                    Range idCell = Globals.ThisAddIn.Application.get_Range("A" + cell.Row.ToString());
                    if (idCell.Value2 == null)
                        continue;
                    item.ID = idCell.Value2.ToString();
                    
                    Range dateCell = Globals.ThisAddIn.Application.get_Range("B" + cell.Row.ToString());
                    Range endpriceCell = Globals.ThisAddIn.Application.get_Range("D" + cell.Row.ToString());
                    
                    if (sellerCell.Interior.Color == System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen))
                        item.isStore = true;
                    
                    item.seller = sellerCell.Value2.ToString();
                    item.endDate = "";
                    item.endPrice = 0;
                    try
                    {
                        item.endDate = dateCell.Value2.ToString().Replace('/', '_');
                        item.endPrice = Convert.ToDouble(endpriceCell.Value2.ToString());
                    }
                    catch (Exception)
                    {

                    }
                    
                    manager.itemsInfo.Insert(0, item);
                }
                manager.getSellerInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cbxSentPaymentNotify_Click(object sender, RibbonControlEventArgs e)
        {

        }



        private void btnGetCat_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Dictionary<string, string> categoryTable = new Dictionary<string, string>();
                categoryTable.Add("1", "CDプレーヤー;アンプ;CDデッキ;システムコンポ;DVDプレーヤー;リモコン;MDデッキ;カセットデッキ;オーディオ機器 ");
                categoryTable.Add("2", "スピーカー");
                categoryTable.Add("3", "プリンター");
                categoryTable.Add("4", "");
                categoryTable.Add("5", "");
                categoryTable.Add("6", "モニター");
                categoryTable.Add("7", "ベビーカー");
                categoryTable.Add("8", "ベビーチェア;チャイルドシート;ベビー家具 > イス");
                categoryTable.Add("9", "抱っこひも;だっこひも");
                categoryTable.Add("10", "陶磁;食器 > 食器;花瓶");
                categoryTable.Add("11", "工芸品 > 金属工芸;美術品;家具 > 西洋");
                categoryTable.Add("12", "ヘッドランプ");
                categoryTable.Add("13", "工具");
                categoryTable.Add("14", "玩具;おもちゃ、ゲーム");
                categoryTable.Add("15", "自転車");
                categoryTable.Add("16", "バイクパーツ;オートバイ > パーツ");
                categoryTable.Add("17", "");
                categoryTable.Add("18", "");
                categoryTable.Add("19", "");
                categoryTable.Add("20", "時計");
                categoryTable.Add("21", "カメラ、光学機器");
                categoryTable.Add("22", "");
                categoryTable.Add("23", "");
                categoryTable.Add("24", "");
                categoryTable.Add("25", "バッグ;ファッション");
                categoryTable.Add("26", "");
                categoryTable.Add("27", "");
                categoryTable.Add("28", "香水");
                categoryTable.Add("29", "釣り具;フィッシング > ロッド");
                categoryTable.Add("30", "家庭用機器");
                categoryTable.Add("31", " エンジン");
                categoryTable.Add("32", "ボート > パーツ;ボート > トレーラー;ボート > アンカー");
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    JToken jsonInfo = null;
                    try
                    {
                        jsonInfo = ExcelAuction.Global.client.GetJsonItemInfo(cell.Value2.ToString());
                        
                    }
                    catch (WebException)
                    {
                        
 
                    }
                    if (jsonInfo == null)
                        continue;

                    Range categoryCell = Globals.ThisAddIn.Application.get_Range("M" + cell.Row.ToString());
                    string productCategory = (string)jsonInfo["CategoryPath"];
                    categoryCell.Value2 = productCategory;

                    (Globals.ThisAddIn.Application.get_Range("L" + cell.Row.ToString())).Value2 = "";
                    foreach (KeyValuePair<string, string> pair in categoryTable)
                    {
                        if (!pair.Value.Equals("") && pair.Value.Split(';').Any(productCategory.Contains))
                        {
                            (Globals.ThisAddIn.Application.get_Range("L" + cell.Row.ToString())).Value2 = pair.Key;
                        }
                    }
                    
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("Tải Xong");
        }

        private void button1_Click_1(object sender, RibbonControlEventArgs e)
        {
            try
            {
                ExcelAuction.Tools.ExtractTwiiterManager manager = new ExcelAuction.Tools.ExtractTwiiterManager();
                foreach (Range cell in ExcelAuction.Global.GetVisibleSelectionCells().Cells)
                {
                    YahooItem item = new YahooItem();
                    Range sellerCell = Globals.ThisAddIn.Application.get_Range("G" + cell.Row.ToString());
                    Range nameCell = Globals.ThisAddIn.Application.get_Range("D" + cell.Row.ToString());
                    item.ID = cell.Value2.ToString();
                    item.seller = sellerCell.Value2.ToString();
                    item.name = nameCell.Value2.ToString();
                    

                    manager.itemsInfo.Insert(0, item);
                }

                ((ExcelAuction.Tools.ExtractTwiiterManager)manager).begin();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //Dictionary<string, List<AcountantItem>> processTable = new Dictionary<string, List<AcountantItem>>();
            //for (int i = 2; i < 1000; i++)
            //{
            //    AcountantItem accItem = new AcountantItem();
            //    try
            //    {
            //        accItem.date = (Globals.ThisAddIn.Application.get_Range("A" + i)).Value2.ToString();
            //    }
            //    catch (Exception)
            //    {

            //        continue;
            //    }

            //    if (accItem.date != null && !accItem.date.Equals(""))
            //    {
                   
            //        accItem.rowNumber = i;
            //        try
            //        {
            //            accItem.incomeVal = Convert.ToInt32(Globals.ThisAddIn.Application.get_Range("B" + i).Value2.ToString());
            //        }
            //        catch (Exception)
            //        {
            //            accItem.incomeVal = 0;
                     
            //        }

            //        try
            //        {
            //            accItem.outcomeVal = Convert.ToInt32(Globals.ThisAddIn.Application.get_Range("C" + i).Value2.ToString());
            //        }
            //        catch (Exception)
            //        {

            //            accItem.outcomeVal = 0;
            //        }

            //        try
            //        {
            //            string customer = Globals.ThisAddIn.Application.get_Range("F" + i).Value2.ToString();
            //            if (Globals.ThisAddIn.Application.get_Range("E" + i).Value2 != null)
            //                accItem.desc += Globals.ThisAddIn.Application.get_Range("E" + i).Value2.ToString();
            //            if (Globals.ThisAddIn.Application.get_Range("H" + i).Value2 != null)
            //                accItem.desc += Globals.ThisAddIn.Application.get_Range("H" + i).Value2.ToString();
                        
            //            if (!processTable.ContainsKey(customer))
            //            {
            //                processTable.Add(customer, new List<AcountantItem>());
            //            }
            //            processTable[customer].Add(accItem);
            //        }
            //        catch (Exception ex)
            //        {

            //            continue;
            //        }
                    
                    
            //    }
            //}

            //foreach (KeyValuePair<string, List<AcountantItem>> pair in processTable)
            //{

            //    processTable[pair.Key].Sort((x, y) => x.date.CompareTo(y.date));
            //}


            //foreach (KeyValuePair<string, List<AcountantItem>> pair in processTable)
            //{
            //    int sum = 0;
            //    int lastIncomeCell = pair.Value[0].rowNumber;
            //    List<AcountantItem> income = new List<AcountantItem>();
            //    List<AcountantItem> outcome = new List<AcountantItem>();
            //    for (int i = 0; i < pair.Value.Count; i++)
            //    {
            //        if (pair.Value[i].incomeVal > 0)
            //            income.Add(pair.Value[i]);
            //        else
            //            outcome.Add(pair.Value[i]);
            //    }

            //    income.Sort((x, y) => x.rowNumber.CompareTo(y.rowNumber));
            //    outcome.Sort((x, y) => x.rowNumber.CompareTo(y.rowNumber));


            //    int j = 0;
            //    for (int i = 0; i < income.Count; i++)
            //    {
            //        sum += income[i].incomeVal;
            //        if (j < outcome.Count)
            //        {
            //            if (sum > (outcome[j].outcomeVal * (1044.0 / 1080)) )
            //            {
            //                sum -= (int)(outcome[j].outcomeVal * (1044.0 / 1080));
            //                (Globals.ThisAddIn.Application.get_Range("J" + income[i].rowNumber)).Value2 = outcome[j].date;
            //                (Globals.ThisAddIn.Application.get_Range("K" + income[i].rowNumber)).Value2 = outcome[j].outcomeVal;
            //                (Globals.ThisAddIn.Application.get_Range("I" + income[i].rowNumber)).Value2 = sum;
            //                (Globals.ThisAddIn.Application.get_Range("L" + income[i].rowNumber)).Value2 = outcome[j].desc;
            //                j++;
            //            }
            //        }

            //    }
            //}
        }
    }
}
