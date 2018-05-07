using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Microsoft.Office;
using Microsoft.Office.Interop;
namespace Atlas
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            String allText = "Sentences\n";
            foreach (Range textRange in Globals.ThisAddIn.Application.ActiveDocument.Sentences)
            {
                allText += textRange.Start + "-" + textRange.End + "\n";
            }

            allText += "Paragraphs\n";
            foreach (Paragraph textRange in Globals.ThisAddIn.Application.ActiveDocument.Paragraphs)
            {
                allText += textRange.Range.Start + "-" + textRange.Range.End + "-" + textRange.Range.Text + "\n";
            }

            allText += "Sections\n";
            foreach (Section textRange in Globals.ThisAddIn.Application.ActiveDocument.Sections)
            {
                allText += textRange.Range.Start + "-" + textRange.Range.End + "\n";
            }

            allText += "Tables\n";
            foreach (Table textRange in Globals.ThisAddIn.Application.ActiveDocument.Tables)
            {
                allText += textRange.Range.Start + "-" + textRange.Range.End + "-" + textRange.ConvertToText(WdTableFieldSeparator.wdSeparateByDefaultListSeparator).Text + "\n";
            }



            using (StreamWriter outfile = new StreamWriter(@"C:\temp\AllTxtFiles.txt"))
            {
                outfile.Write(allText);
            }


        }

        private String exportDir(String docPath)
        {
            return Path.Combine(docPath, "exported");
        }

        private void prepareFolder(String docPath)
        {
            String exportedPath = exportDir(docPath);
            if (Directory.Exists(exportedPath))
                Directory.Delete(exportedPath, true);
            Directory.CreateDirectory(exportedPath);
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            Range rng = Globals.ThisAddIn.Application.ActiveDocument.Range(1, 3);
            //MessageBox.Show(rng.Font.Superscript.ToString());
            rng.Font.Superscript = Convert.ToInt32(true);
        }

        private String trimText(String text)
        {
            if (text == null) return "";
            char[] trimChars = { '\n', '\r', '', ' ', '\f' };
            return text.Trim(trimChars);
        }

        private String paragraphToText(Paragraph para)
        {
            //return para.Range.Text;
            String text = "";
            int kTrue = -1;
            for (int i = para.Range.Start; i < para.Range.End; i++)
            {
                Range rng = Globals.ThisAddIn.Application.ActiveDocument.Range(i, i + 1);
                if (rng.Font.Superscript == kTrue)
                {
                    int j;
                    for (j = i + 1; j < para.Range.End; j++)
                    {
                        if (Globals.ThisAddIn.Application.ActiveDocument.Range(j, j + 1).Font.Superscript != kTrue)
                            break;
                    }

                    text += String.Format("<sup>{0}</sup>", trimText(Globals.ThisAddIn.Application.ActiveDocument.Range(i, j).Text));
                    i = j - 1;
                    continue;
                }

                if (rng.Font.Subscript == kTrue)
                {
                    int j;
                    for (j = i + 1; j < para.Range.End; j++)
                    {
                        if (Globals.ThisAddIn.Application.ActiveDocument.Range(j, j + 1).Font.Subscript != kTrue)
                            break;
                    }

                    text += String.Format("<sub>{0}</sub>", trimText(Globals.ThisAddIn.Application.ActiveDocument.Range(i, j).Text));
                    i = j - 1;
                    continue;
                }

                if (rng.Font.Bold == kTrue)
                {
                    int j;
                    for (j = i + 1; j < para.Range.End; j++)
                    {
                        if (Globals.ThisAddIn.Application.ActiveDocument.Range(j, j + 1).Font.Bold != kTrue)
                            break;
                    }

                    text += String.Format("<b>{0}</b>", trimText(Globals.ThisAddIn.Application.ActiveDocument.Range(i, j).Text));
                    i = j - 1;
                    continue;
                }

                if (rng.Font.Italic == kTrue)
                {
                    int j;
                    for (j = i + 1; j < para.Range.End; j++)
                    {
                        if (Globals.ThisAddIn.Application.ActiveDocument.Range(j, j + 1).Font.Italic != kTrue)
                            break;
                    }

                    text += String.Format("<i>{0}</i>", trimText(Globals.ThisAddIn.Application.ActiveDocument.Range(i, j).Text));
                    i = j - 1;
                    continue;
                }

                if (rng.Font.Underline != WdUnderline.wdUnderlineNone)
                {
                    int j;
                    for (j = i + 1; j < para.Range.End; j++)
                    {
                        if (Globals.ThisAddIn.Application.ActiveDocument.Range(j, j + 1).Font.Underline == WdUnderline.wdUnderlineNone)
                            break;
                    }

                    text += String.Format("<b>{0}</b>", trimText(Globals.ThisAddIn.Application.ActiveDocument.Range(i, j).Text));
                    i = j - 1;
                    continue;
                }



                text += rng.Text;

            }
            return text;
        }
        private Range getObjectRange(Object obj)
        {
            if (obj is Paragraph)
            {
                return ((Paragraph)obj).Range;
            }

            if (obj is Table)
            {
                return ((Table)obj).Range;
            }

            if (obj is InlineShape)
            {
                return ((InlineShape)obj).Range;
            }

            if (obj is Shape)
            {
                return ((Shape)obj).Anchor;
            }
            return null;
        }

        private String tableToHTML(Table table)
        {
            return "";
        }


        public class RangeComparer : IComparer
        {
            private Range getObjectRange(Object obj)
            {
                if (obj is Paragraph)
                {
                    return ((Paragraph)obj).Range;
                }

                if (obj is Table)
                {
                    return ((Table)obj).Range;
                }

                if (obj is InlineShape)
                {
                    return ((InlineShape)obj).Range;
                }

                if (obj is Shape)
                {
                    return ((Shape)obj).Anchor;
                }
                return null;
            }
            int IComparer.Compare(Object x, Object y)
            {
                return getObjectRange(x).Start.CompareTo(getObjectRange(y).Start);
            }

        }


        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            prepareFolder(Globals.ThisAddIn.Application.ActiveDocument.Path);
            String allText = "";

            ArrayList docObjects = new ArrayList();

            foreach (Paragraph textPara in Globals.ThisAddIn.Application.ActiveDocument.Paragraphs)
            {
                if (!trimText(textPara.Range.Text).Equals(""))
                    docObjects.Add(textPara);
            };

            int numberOfTable = Globals.ThisAddIn.Application.ActiveDocument.Tables.Count;
            ArrayList removed = new ArrayList();
            foreach (Table table in Globals.ThisAddIn.Application.ActiveDocument.Tables)
            {
                int i = 0;
                for (i = 0; i < docObjects.Count; i++)
                {
                    Range objRng = getObjectRange(docObjects[i]);
                    if (objRng.Start >= table.Range.Start && objRng.End <= table.Range.End)
                    {
                        removed.Add(docObjects[i]);
                    }

                    if (objRng.Start > table.Range.End)
                        break;
                }

                foreach (object obj in removed)
                    docObjects.Remove(obj);
                docObjects.Add(table);
            }

            foreach (Shape shape in Globals.ThisAddIn.Application.ActiveDocument.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoPicture ||
                    shape.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                {
                    docObjects.Add(shape);
                }

                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                {
                    String a = shape.TextFrame.TextRange.Text;
                }
            }

            foreach (InlineShape shape in Globals.ThisAddIn.Application.ActiveDocument.InlineShapes)
            {
                if (shape.Type == WdInlineShapeType.wdInlineShapePicture)
                {
                    docObjects.Add(shape);
                }
            }
            printDebug(docObjects, "debug1.txt");
            docObjects.Sort(new RangeComparer());
            printDebug(docObjects, "debug2.txt");

            ///
            int tableIndex = 1;
            int pageIndex = 0;
            int pictureIndex = 0;
            foreach (Object obj in docObjects)
            {
                Range rng = getObjectRange(obj);
                if (Convert.ToInt32(rng.get_Information(WdInformation.wdActiveEndPageNumber)) != pageIndex)
                {
                    allText += "//-====PAGE====\n";
                    pageIndex = Convert.ToInt32(rng.get_Information(WdInformation.wdActiveEndPageNumber));
                }
                if (obj is Paragraph)
                {
                    Paragraph textPara = (Paragraph)obj;
                    if (!trimText(textPara.Range.Text).Equals(""))
                    {
                        if (textPara.Range.Font.Superscript == (int)WdConstants.wdUndefined)
                        {

                        }

                        allText += paragraphToText(textPara) + "\n";
                        //allText += textPara.Range.Text + "\n";
                    }
                }

                if (obj is Table)
                {
                    Table table = (Table)obj;
                    allText += String.Format(@"include table-{0}.html" + "\n", tableIndex);
                    tableIndex++;
                }

                if (obj is InlineShape)
                {
                    InlineShape picture = (InlineShape)obj;
                    allText += String.Format("img(src=\"images/picture-{0}.jpg\")" + "\n", pictureIndex);
                    exportShape((InlineShape)obj, pictureIndex);
                    pictureIndex++;
                }


                if (obj is Shape)
                {
                    Shape picture = (Shape)obj;
                    allText += String.Format("img(src=\"images/picture-{0}.jpg\")" + "\n", pictureIndex);
                    exportShape((Shape)obj, pictureIndex);
                    pictureIndex++;
                }

            }



            using (StreamWriter outfile = new StreamWriter(Path.Combine(exportDir(Globals.ThisAddIn.Application.ActiveDocument.Path), "atlas.txt")))
            {
                outfile.Write(allText);
            }
        }

        private void printDebug(ArrayList docObjects, String debugFile)
        {
            //print debug
            String debug = "";
            foreach (Object obj in docObjects)
            {
                Range rng = getObjectRange(obj);
                if (obj is Paragraph)
                {
                    debug += String.Format("Para {0} {1}\n", rng.Start, rng.End);
                }

                if (obj is Table)
                {
                    debug += String.Format("Table {0} {1}\n", rng.Start, rng.End);
                }

                if (obj is InlineShape)
                {
                    debug += String.Format("InlineShape {0} {1}\n", rng.Start, rng.End);
                }


                if (obj is Shape)
                {
                    debug += String.Format("Shape {0} {1}\n", rng.Start, rng.End);
                }
            }
            using (StreamWriter outfile = new StreamWriter(Path.Combine(exportDir(Globals.ThisAddIn.Application.ActiveDocument.Path), debugFile)))
            {
                outfile.Write(debug);
            }
        }

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            int tableIndex = 1;
            String exportFolder = exportDir(Globals.ThisAddIn.Application.ActiveDocument.Path);
            foreach(Table table in Globals.ThisAddIn.Application.ActiveDocument.Tables)
            {
                table.Range.Copy();
                Document doc = Globals.ThisAddIn.Application.Documents.Add();
                Globals.ThisAddIn.Application.ActiveWindow.Selection.Paste();
                doc.SaveAs2(Path.Combine(exportFolder, String.Format(@"table-{0}.html", tableIndex)), WdSaveFormat.wdFormatFilteredHTML, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                doc.Close();
                tableIndex++;
            }
            
        }

        private void button1_Click_1(object sender, RibbonControlEventArgs e)
        {
            //Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
           //ppApp.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
           // Microsoft.Office.Interop.PowerPoint.Presentation present = ppApp.Presentations.Add();
            //String a = Globals.ThisAddIn.Application.ActiveDocument.Path;
            int shapeIndex = 1;
            foreach (Shape shape in Globals.ThisAddIn.Application.ActiveDocument.Shapes)
            {
                shape.Select();
                Globals.ThisAddIn.Application.ActiveWindow.Selection.CopyAsPicture();
                System.Drawing.Image im = Clipboard.GetImage();
                im.Save(String.Format(@"C:\temp\picture-{0}.jpg", shapeIndex));
                shapeIndex++;
            }
        }

        private void exportShape(Shape shape, int index)
        {
            shape.Select();
            String path = Path.Combine(exportDir(Globals.ThisAddIn.Application.ActiveDocument.Path), String.Format(@"picture-{0}.jpg", index));
            Globals.ThisAddIn.Application.ActiveWindow.Selection.CopyAsPicture();
            System.Drawing.Image im = Clipboard.GetImage();
            try
            {
                im.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
            }
            
        }

        private void exportShape(InlineShape shape, int index)
        {
            shape.Select();
            String path = Path.Combine(exportDir(Globals.ThisAddIn.Application.ActiveDocument.Path), String.Format(@"picture-{0}.jpg", index));
            Globals.ThisAddIn.Application.ActiveWindow.Selection.CopyAsPicture();
            System.Drawing.Image im = Clipboard.GetImage();
            try
            {
                im.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
            }
        }
    }
}
