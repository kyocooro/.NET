using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.IO;

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
                Globals.ThisAddIn.Application.get_Range("E2").Value2 = item.name;
                Globals.ThisAddIn.Application.get_Range("G2").Value2 = item.endPrice;
                Globals.ThisAddIn.Application.get_Range("H2").Value2 = item.seller;
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
                System.Diagnostics.Process.Start("firefox", @"http://auctions.yahoo.co.jp/jp/auction/" + autionID);
            }
        }

        private void btnLogout_Click(object sender, RibbonControlEventArgs e)
        {
            LoginForm form = new LoginForm();
            form.ShowDialog();
            form.logout();
        }

        private void btnLoadImage_Click(object sender, RibbonControlEventArgs e)
        {
            String worksheetPath = Globals.ThisAddIn.Application.ActiveWorkbook.Path;
            for (int i = 2; i < 5000; i++)
            {
                Range firstCell = Globals.ThisAddIn.Application.get_Range("A" + i);
                if (firstCell.Value2 == null)
                    break;
                String itemID = firstCell.Value2.ToString();
                if (itemID.Equals(""))
                    break;
                else
                {
                    try
                    {
                        Comment comment = Globals.ThisAddIn.Application.get_Range("B" + i).AddComment("");
                        comment.Visible = false;
                        Globals.ThisAddIn.Application.get_Range("B" + i).Comment.Shape.Fill.UserPicture(Path.Combine(worksheetPath, itemID, "1.jpg"));
                        comment = Globals.ThisAddIn.Application.get_Range("C" + i).AddComment("");
                        comment.Visible = false;
                        Globals.ThisAddIn.Application.get_Range("C" + i).Comment.Shape.Fill.UserPicture(Path.Combine(worksheetPath, itemID, "2.jpg"));
                        comment = Globals.ThisAddIn.Application.get_Range("D" + i).AddComment("");
                        comment.Visible = false;
                        Globals.ThisAddIn.Application.get_Range("D" + i).Comment.Shape.Fill.UserPicture(Path.Combine(worksheetPath, itemID, "3.jpg"));
                    }
                    catch (System.Exception ex)
                    {
                    	
                    }
                    
                }
            }
            
            
            
            //
            //Range firstCell = Globals.ThisAddIn.Application.get_Range("A2");
            //List<YahooItem> newItems = getNewIDFrom();
            //newItems.Reverse();
            //foreach (YahooItem item in newItems)
            //{
            //    Globals.ThisAddIn.Application.get_Range("A2").EntireRow.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            //    Globals.ThisAddIn.Application.get_Range("A2").Value2 = item.ID;
            //    Globals.ThisAddIn.Application.get_Range("E2").Value2 = item.name;
            //    Globals.ThisAddIn.Application.get_Range("G2").Value2 = item.endPrice;
            //    Globals.ThisAddIn.Application.get_Range("H2").Value2 = item.seller;
            //    Globals.ThisAddIn.Application.get_Range("I2").Value2 = item.endDate;
            //}
        }

        private void btnShowQA_Click(object sender, RibbonControlEventArgs e)
        {
            Range selectedCell = Globals.ThisAddIn.Application.Selection as Range;
            if (selectedCell != null)
            {
                String autionID = Globals.ThisAddIn.Application.get_Range("A" + Convert.ToString(selectedCell.Row)).Value2.ToString();
                String qaPath = "D:\\Auction\\QA\\" + autionID + ".html";
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
    }
}
