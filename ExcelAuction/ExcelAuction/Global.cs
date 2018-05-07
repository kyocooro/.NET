using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
namespace ExcelAuction
{
    class Global
    {
        public static String authenticationCode = null;
        public static String accessCode = null;
        public static Uri returnURI = null;
        public static YahooClient client = new YahooClient("dj0zaiZpPWhzQVdWRFpuYXV0WiZzPWNvbnN1bWVyc2VjcmV0Jng9Yzk-", "");
        public static String storeLocation = "C:\\Auction\\";
        public static Range GetVisibleSelectionCells()
        {
            Range selection = (Globals.ThisAddIn.Application.Selection as Range);
            if (selection.Cells.Count > 1) selection = selection.SpecialCells(XlCellType.xlCellTypeVisible);
            return selection;
        }
    }
}
