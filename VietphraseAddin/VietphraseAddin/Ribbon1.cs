using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using TranslatorEngine;
namespace VietphraseAddin
{
    public partial class Ribbon1
    {
        static bool isInit = false;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
           
        }

        private void btnConvert_Click(object sender, RibbonControlEventArgs e)
        {
            if (!isInit)
            {
                try
                {
                    TranslatorEngine.TranslatorEngine.LoadDictionaries();
                    isInit = true;
                }
                catch
                {

                }
                
            }
            Range selection = (Globals.ThisAddIn.Application.Selection as Range).SpecialCells(XlCellType.xlCellTypeVisible);

            foreach (Range cell in selection.Cells)
            {
                try
                {
                    string rawText = cell.Value2.ToString();
                    if (rawText[0] != '#')
                    {
                        CharRange[] a;
                        CharRange[] b;
                        string translatedText = TranslatorEngine.TranslatorEngine.ChineseToVietPhraseOneMeaning(rawText,
                                0, 1, true, out a, out b);
                        translatedText = translatedText.TrimStart();

                        cell.Value2 = System.Text.RegularExpressions.Regex.Replace(translatedText, "^[a-z]", m => m.Value.ToUpper()); ;
                    }
                }
                catch
                {

                }
            }
        }

    }
}
