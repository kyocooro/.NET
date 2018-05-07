// Decompiled with JetBrains decompiler
// Type: TranslatorEngine.DictionaryConfigurationHelper
// Assembly: TranslatorEngine, Version=1.4.4937.24288, Culture=neutral, PublicKeyToken=null
// MVID: 636F5D06-194A-462F-B92A-7489F7DCC048
// Assembly location: D:\QuickTranslator\TranslatorEngine.dll

using System.IO;
using System.Reflection;

namespace TranslatorEngine
{
  public class DictionaryConfigurationHelper
  {
    private static string directoryPath = @"D:\QuickTranslator"; //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string thuatToanNhan = string.Empty;

    public static bool IsNhanByPronouns
    {
      get
      {
        if (string.IsNullOrEmpty(DictionaryConfigurationHelper.thuatToanNhan))
          DictionaryConfigurationHelper.readThuatToanNhan();
        return DictionaryConfigurationHelper.thuatToanNhan == "1";
      }
    }

    public static bool IsNhanByPronounsAndNames
    {
      get
      {
        if (string.IsNullOrEmpty(DictionaryConfigurationHelper.thuatToanNhan))
          DictionaryConfigurationHelper.readThuatToanNhan();
        return DictionaryConfigurationHelper.thuatToanNhan == "2";
      }
    }

    public static bool IsNhanByPronounsAndNamesAndVietPhrase
    {
      get
      {
        if (string.IsNullOrEmpty(DictionaryConfigurationHelper.thuatToanNhan))
          DictionaryConfigurationHelper.readThuatToanNhan();
        return DictionaryConfigurationHelper.thuatToanNhan == "3";
      }
    }

    public static string GetNamesPhuDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("NamesPhu");
    }

    public static string GetNamesDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("Names");
    }

    public static string GetNamesDictionaryHistoryPath()
    {
      return Path.Combine(Path.GetDirectoryName(DictionaryConfigurationHelper.GetNamesDictionaryPath()), Path.GetFileNameWithoutExtension(DictionaryConfigurationHelper.GetNamesDictionaryPath()) + "History" + Path.GetExtension(DictionaryConfigurationHelper.GetNamesDictionaryPath()));
    }

    public static string GetNamesPhuDictionaryHistoryPath()
    {
      return Path.Combine(Path.GetDirectoryName(DictionaryConfigurationHelper.GetNamesPhuDictionaryPath()), Path.GetFileNameWithoutExtension(DictionaryConfigurationHelper.GetNamesPhuDictionaryPath()) + "History" + Path.GetExtension(DictionaryConfigurationHelper.GetNamesPhuDictionaryPath()));
    }

    public static string GetVietPhraseDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("VietPhrase");
    }

    public static string GetVietPhraseDictionaryHistoryPath()
    {
      return Path.Combine(Path.GetDirectoryName(DictionaryConfigurationHelper.GetVietPhraseDictionaryPath()), Path.GetFileNameWithoutExtension(DictionaryConfigurationHelper.GetVietPhraseDictionaryPath()) + "History" + Path.GetExtension(DictionaryConfigurationHelper.GetVietPhraseDictionaryPath()));
    }

    public static string GetChinesePhienAmWordsDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("ChinesePhienAmWords");
    }

    public static string GetChinesePhienAmWordsDictionaryHistoryPath()
    {
      return Path.Combine(Path.GetDirectoryName(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath()), Path.GetFileNameWithoutExtension(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath()) + "History" + Path.GetExtension(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath()));
    }

    public static string GetChinesePhienAmEnglishWordsDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("ChinesePhienAmEnglishWords");
    }

    public static string GetCEDictDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("CEDict");
    }

    public static string GetBabylonDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("Babylon");
    }

    public static string GetLacVietDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("LacViet");
    }

    public static string GetThieuChuuDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("ThieuChuu");
    }

    public static string GetIgnoredChinesePhraseListPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("IgnoredChinesePhrases");
    }

    public static string GetLuatNhanDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("LuatNhan");
    }

    public static string GetPronounsDictionaryPath()
    {
      return DictionaryConfigurationHelper.GetDictionaryPathByKey("Pronouns");
    }

    private static string GetDictionaryPathByKey(string dictionaryKey)
    {
      string[] strArray = File.ReadAllLines(Path.Combine(DictionaryConfigurationHelper.directoryPath, "Dictionaries.config"));
      string str1 = string.Empty;
      foreach (string str2 in strArray)
      {
        if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("#") && str2.StartsWith(dictionaryKey + "="))
        {
          str1 = str2.Split('=')[1];
          break;
        }
      }
      if (!Path.IsPathRooted(str1))
        str1 = Path.Combine(DictionaryConfigurationHelper.directoryPath, str1);
      if (!File.Exists(str1))
        throw new FileNotFoundException("Dictionary Not Found: " + str1);
      else
        return str1;
    }

    private static void readThuatToanNhan()
    {
      foreach (string str in File.ReadAllLines(Path.Combine(DictionaryConfigurationHelper.directoryPath, "Dictionaries.config")))
      {
        if (!string.IsNullOrEmpty(str) && !str.StartsWith("#") && str.StartsWith("ThuatToanNhan="))
        {
          DictionaryConfigurationHelper.thuatToanNhan = str.Split('=')[1];
          break;
        }
      }
    }
  }
}
