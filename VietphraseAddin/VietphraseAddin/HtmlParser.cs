// Decompiled with JetBrains decompiler
// Type: TranslatorEngine.HtmlParser
// Assembly: TranslatorEngine, Version=1.4.4937.24288, Culture=neutral, PublicKeyToken=null
// MVID: 636F5D06-194A-462F-B92A-7489F7DCC048
// Assembly location: D:\QuickTranslator\TranslatorEngine.dll

using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TranslatorEngine
{
  public class HtmlParser
  {
    private static bool dirty = true;
    private static string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string[] titleTags;
    private static string[] contentTags;
    private static string[] removedTags;

    public static string GetChineseContent(string htmlContent, bool needMarkChapterHeaders)
    {
      HtmlParser.LoadConfiguration();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str1 in HtmlParser.titleTags)
      {
        if (!string.IsNullOrEmpty(str1) && !str1.StartsWith("#") && htmlContent.ToLower().Contains(str1.ToLower()))
        {
          string str2 = htmlContent.Substring(htmlContent.ToLower().IndexOf(str1.ToLower()) + str1.Length);
          string str3 = str1.Substring(str1.LastIndexOf('<') + 1);
          string str4 = str3.Substring(0, str3.IndexOfAny(new char[2]
          {
            ' ',
            '>'
          }));
          if (str2.ToLower().Contains("</" + str4.ToLower() + ">"))
          {
            stringBuilder.AppendLine((needMarkChapterHeaders ? "$CHAPTER_HEADER$. " : "") + str2.Substring(0, str2.ToLower().IndexOf("</" + str4.ToLower() + ">")).TrimStart(" 　\t".ToCharArray()));
            break;
          }
        }
      }
      foreach (string str1 in HtmlParser.contentTags)
      {
        if (!string.IsNullOrEmpty(str1) && !str1.StartsWith("#") && htmlContent.ToLower().Contains(str1.ToLower()))
        {
          string str2 = htmlContent.Substring(htmlContent.ToLower().IndexOf(str1.ToLower()) + str1.Length);
          if (str1 != "<!--bodybegin-->")
          {
            string str3 = str1.Substring(str1.LastIndexOf('<') + 1);
            string str4 = str3.Substring(0, str3.IndexOfAny(new char[2]
            {
              ' ',
              '>'
            }));
            if (str2.ToLower().Contains("</" + str4.ToLower().TrimStart('/') + ">"))
            {
              stringBuilder.AppendLine(str2.Substring(0, str2.ToLower().IndexOf("</" + str4.ToLower().TrimStart('/') + ">")));
              break;
            }
          }
          else
          {
            string str3 = "<!--bodyend-->";
            if (str2.Contains(str3))
              stringBuilder.AppendLine(str2.Substring(0, str2.ToLower().IndexOf(str3.ToLower())));
          }
        }
      }
      string str = ((object) stringBuilder).ToString();
      foreach (string oldValue in HtmlParser.removedTags)
      {
        if (!string.IsNullOrEmpty(oldValue) && !oldValue.StartsWith("#"))
          str = str.Replace(oldValue, string.Empty);
      }
      return Regex.Replace(str.Replace("<p>", "\n").Replace("</p>", "\n").Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n").Replace("<BR>", "\n").Replace("<BR/>", "\n").Replace("<BR />", "\n").Replace("&nbsp;", "").Replace("&lt;", "").Replace("&gt;", ""), "<(.|\\n)*?>", string.Empty);
    }

    private static void LoadConfiguration()
    {
      if (!HtmlParser.dirty)
        return;
      HtmlParser.titleTags = File.ReadAllLines(Path.Combine(HtmlParser.directoryPath, "HtmlChapterTitleTags.config"));
      HtmlParser.contentTags = File.ReadAllLines(Path.Combine(HtmlParser.directoryPath, "HtmlChapterContentTags.config"));
      HtmlParser.removedTags = File.ReadAllLines(Path.Combine(HtmlParser.directoryPath, "HtmlRemovedTags.config"));
      HtmlParser.dirty = false;
    }
  }
}
