// Decompiled with JetBrains decompiler
// Type: TranslatorEngine.TranslatorEngine
// Assembly: TranslatorEngine, Version=1.4.4937.24288, Culture=neutral, PublicKeyToken=null
// MVID: 636F5D06-194A-462F-B92A-7489F7DCC048
// Assembly location: D:\QuickTranslator\TranslatorEngine.dll

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TranslatorEngine
{
  public class TranslatorEngine
  {
    private static bool dictionaryDirty = true;
    private static Dictionary<string, string> hanVietDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> vietPhraseDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> thieuChuuDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> lacVietDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> cedictDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> chinesePhienAmEnglishDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> vietPhraseOneMeaningDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> onlyVietPhraseDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> onlyNameDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> onlyNameOneMeaningDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> onlyNameChinhDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> onlyNamePhuDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> luatNhanDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> pronounDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> pronounOneMeaningDictionary = new Dictionary<string, string>();
    private static Dictionary<string, string> nhanByDictionary = (Dictionary<string, string>) null;
    private static Dictionary<string, string> nhanByOneMeaningDictionary = (Dictionary<string, string>) null;
    private static DataSet onlyVietPhraseDictionaryHistoryDataSet = new DataSet();
    private static DataSet onlyNameDictionaryHistoryDataSet = new DataSet();
    private static DataSet onlyNamePhuDictionaryHistoryDataSet = new DataSet();
    private static DataSet hanVietDictionaryHistoryDataSet = new DataSet();
    private static List<string> ignoredChinesePhraseList = new List<string>();
    private static List<string> ignoredChinesePhraseForBrowserList = new List<string>();
    private static object lockObject = new object();
    private static string NULL_STRING = Convert.ToChar(0).ToString();
    public static string LastTranslatedWord_HanViet = "";
    public static string LastTranslatedWord_VietPhrase = "";
    public static string LastTranslatedWord_VietPhraseOneMeaning = "";
    private static char[] trimCharsForAnalyzer = new char[3]
    {
      ' ',
      '\n',
      '\t'
    };
    public const int CHINESE_LOOKUP_MAX_LENGTH = 20;

    public static bool DictionaryDirty
    {
      get
      {
        return TranslatorEngine.dictionaryDirty;
      }
      set
      {
        TranslatorEngine.dictionaryDirty = value;
      }
    }

    public static string GetVietPhraseOrNameValueFromKey(string key)
    {
      if (!TranslatorEngine.vietPhraseDictionary.ContainsKey(key))
        return (string) null;
      else
        return TranslatorEngine.vietPhraseDictionary[key];
    }

    public static string GetVietPhraseValueFromKey(string key)
    {
      if (!TranslatorEngine.onlyVietPhraseDictionary.ContainsKey(key))
        return (string) null;
      else
        return TranslatorEngine.onlyVietPhraseDictionary[key];
    }

    public static string GetNameValueFromKey(string key)
    {
      if (!TranslatorEngine.onlyNameDictionary.ContainsKey(key))
        return (string) null;
      else
        return TranslatorEngine.onlyNameDictionary[key];
    }

    public static string GetNameValueFromKey(string key, bool isNameChinh)
    {
      Dictionary<string, string> dictionary = isNameChinh ? TranslatorEngine.onlyNameChinhDictionary : TranslatorEngine.onlyNamePhuDictionary;
      if (!dictionary.ContainsKey(key))
        return (string) null;
      else
        return dictionary[key];
    }

    public static void DeleteKeyFromVietPhraseDictionary(string key, bool sorting)
    {
      TranslatorEngine.vietPhraseDictionary.Remove(key);
      TranslatorEngine.vietPhraseOneMeaningDictionary.Remove(key);
      TranslatorEngine.onlyVietPhraseDictionary.Remove(key);
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref TranslatorEngine.onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(TranslatorEngine.onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
      TranslatorEngine.writeVietPhraseHistoryLog(key, "Deleted");
    }

    public static void DeleteKeyFromNameDictionary(string key, bool sorting, bool isNameChinh)
    {
      TranslatorEngine.vietPhraseDictionary.Remove(key);
      TranslatorEngine.vietPhraseOneMeaningDictionary.Remove(key);
      TranslatorEngine.onlyNameDictionary.Remove(key);
      TranslatorEngine.onlyNameOneMeaningDictionary.Remove(key);
      Dictionary<string, string> dictionary = isNameChinh ? TranslatorEngine.onlyNameChinhDictionary : TranslatorEngine.onlyNamePhuDictionary;
      if (!dictionary.ContainsKey(key))
        return;
      dictionary.Remove(key);
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
      TranslatorEngine.writeNamesHistoryLog(key, "Deleted", isNameChinh);
    }

    public static void DeleteKeyFromPhienAmDictionary(string key, bool sorting)
    {
      TranslatorEngine.hanVietDictionary.Remove(key);
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref TranslatorEngine.hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(TranslatorEngine.hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
      TranslatorEngine.writePhienAmHistoryLog(key, "Deleted");
    }

    public static void UpdateVietPhraseDictionary(string key, string value, bool sorting)
    {
      if (TranslatorEngine.vietPhraseDictionary.ContainsKey(key))
        TranslatorEngine.vietPhraseDictionary[key] = value;
      else
        TranslatorEngine.vietPhraseDictionary.Add(key, value);
      if (TranslatorEngine.vietPhraseOneMeaningDictionary.ContainsKey(key))
        TranslatorEngine.vietPhraseOneMeaningDictionary[key] = value.Split(new char[2]
        {
          '/',
          '|'
        })[0];
      else
        TranslatorEngine.vietPhraseOneMeaningDictionary.Add(key, value.Split(new char[2]
        {
          '/',
          '|'
        })[0]);
      if (TranslatorEngine.onlyVietPhraseDictionary.ContainsKey(key))
      {
        TranslatorEngine.onlyVietPhraseDictionary[key] = value;
        TranslatorEngine.writeVietPhraseHistoryLog(key, "Updated");
      }
      else
      {
        if (sorting)
          TranslatorEngine.onlyVietPhraseDictionary.Add(key, value);
        else
          TranslatorEngine.onlyVietPhraseDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.onlyVietPhraseDictionary, key, value);
        TranslatorEngine.writeVietPhraseHistoryLog(key, "Added");
      }
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref TranslatorEngine.onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(TranslatorEngine.onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
    }

    private static Dictionary<string, string> AddEntryToDictionaryWithoutSorting(Dictionary<string, string> dictionary, string key, string value)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
      dictionary1.Add(key, value);
      return dictionary1;
    }

    public static void UpdateNameDictionary(string key, string value, bool sorting, bool isNameChinh)
    {
      if (TranslatorEngine.vietPhraseDictionary.ContainsKey(key))
        TranslatorEngine.vietPhraseDictionary[key] = value;
      else
        TranslatorEngine.vietPhraseDictionary.Add(key, value);
      if (TranslatorEngine.vietPhraseOneMeaningDictionary.ContainsKey(key))
        TranslatorEngine.vietPhraseOneMeaningDictionary[key] = value.Split(new char[2]
        {
          '/',
          '|'
        })[0];
      else
        TranslatorEngine.vietPhraseOneMeaningDictionary.Add(key, value.Split(new char[2]
        {
          '/',
          '|'
        })[0]);
      Dictionary<string, string> dictionary = isNameChinh ? TranslatorEngine.onlyNameChinhDictionary : TranslatorEngine.onlyNamePhuDictionary;
      if (dictionary.ContainsKey(key))
      {
        dictionary[key] = value;
        TranslatorEngine.writeNamesHistoryLog(key, "Updated", isNameChinh);
      }
      else
      {
        if (sorting)
          dictionary.Add(key, value);
        else if (isNameChinh)
        {
          TranslatorEngine.onlyNameChinhDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.onlyNameChinhDictionary, key, value);
          dictionary = TranslatorEngine.onlyNameChinhDictionary;
        }
        else
        {
          TranslatorEngine.onlyNamePhuDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.onlyNamePhuDictionary, key, value);
          dictionary = TranslatorEngine.onlyNamePhuDictionary;
        }
        TranslatorEngine.writeNamesHistoryLog(key, "Added", isNameChinh);
      }
      if (TranslatorEngine.onlyNameDictionary.ContainsKey(key))
      {
        TranslatorEngine.onlyNameDictionary[key] = value;
        TranslatorEngine.onlyNameOneMeaningDictionary[key] = value.Split(new char[2]
        {
          '/',
          '|'
        })[0];
      }
      else if (sorting)
      {
        TranslatorEngine.onlyNameDictionary.Add(key, value);
        TranslatorEngine.onlyNameOneMeaningDictionary.Add(key, value.Split(new char[2]
        {
          '/',
          '|'
        })[0]);
      }
      else
      {
        TranslatorEngine.onlyNameDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.onlyNameDictionary, key, value);
        TranslatorEngine.onlyNameOneMeaningDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.onlyNameOneMeaningDictionary, key, value.Split(new char[2]
        {
          '/',
          '|'
        })[0]);
      }
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
    }

    public static void UpdatePhienAmDictionary(string key, string value, bool sorting)
    {
      if (TranslatorEngine.hanVietDictionary.ContainsKey(key))
      {
        TranslatorEngine.hanVietDictionary[key] = value;
        TranslatorEngine.writePhienAmHistoryLog(key, "Updated");
      }
      else
      {
        if (sorting)
          TranslatorEngine.hanVietDictionary.Add(key, value);
        else
          TranslatorEngine.hanVietDictionary = TranslatorEngine.AddEntryToDictionaryWithoutSorting(TranslatorEngine.hanVietDictionary, key, value);
        TranslatorEngine.writePhienAmHistoryLog(key, "Added");
      }
      if (sorting)
        TranslatorEngine.SaveDictionaryToFile(ref TranslatorEngine.hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
      else
        TranslatorEngine.SaveDictionaryToFileWithoutSorting(TranslatorEngine.hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
    }

    public static void SaveDictionaryToFileWithoutSorting(Dictionary<string, string> dictionary, string filePath)
    {
      string str = filePath + (object) "." + (string) (object) DateTime.Now.Ticks;
      if (File.Exists(filePath))
        File.Copy(filePath, str, true);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        stringBuilder.Append(keyValuePair.Key).Append("=").AppendLine(keyValuePair.Value);
      try
      {
        File.WriteAllText(filePath, ((object) stringBuilder).ToString(), Encoding.UTF8);
      }
      catch (Exception ex)
      {
        try
        {
          File.Copy(str, filePath, true);
        }
        catch
        {
        }
        throw ex;
      }
      if (!File.Exists(filePath))
        return;
      File.Delete(str);
    }

    public static void SaveDictionaryToFile(ref Dictionary<string, string> dictionary, string filePath)
    {
      IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = Enumerable.ThenBy<KeyValuePair<string, string>, string>(Enumerable.OrderByDescending<KeyValuePair<string, string>, int>((IEnumerable<KeyValuePair<string, string>>) dictionary, (Func<KeyValuePair<string, string>, int>) (pair => pair.Key.Length)), (Func<KeyValuePair<string, string>, string>) (pair => pair.Key));
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      string str = filePath + (object) "." + (string) (object) DateTime.Now.Ticks;
      if (File.Exists(filePath))
        File.Copy(filePath, str, true);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) orderedEnumerable)
      {
        stringBuilder.Append(keyValuePair.Key).Append("=").AppendLine(keyValuePair.Value);
        dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
      }
      dictionary = dictionary1;
      try
      {
        File.WriteAllText(filePath, ((object) stringBuilder).ToString(), Encoding.UTF8);
      }
      catch (Exception ex)
      {
        try
        {
          File.Copy(str, filePath, true);
        }
        catch
        {
        }
        throw ex;
      }
      if (!File.Exists(filePath))
        return;
      File.Delete(str);
    }

    public static string ChineseToHanViet(string chinese, out CharRange[] chineseHanVietMappingArray)
    {
      TranslatorEngine.LastTranslatedWord_HanViet = "";
      List<CharRange> list = new List<CharRange>();
      StringBuilder result = new StringBuilder();
      int length1 = chinese.Length;
      for (int index = 0; index < length1 - 1; ++index)
      {
        int length2 = ((object) result).ToString().Length;
        char ch = chinese[index];
        char character = chinese[index + 1];
        if (TranslatorEngine.isChinese(ch))
        {
          if (TranslatorEngine.isChinese(character))
          {
            TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(ch), ref TranslatorEngine.LastTranslatedWord_HanViet, ref length2);
            result.Append(" ");
            TranslatorEngine.LastTranslatedWord_HanViet = TranslatorEngine.LastTranslatedWord_HanViet + " ";
            list.Add(new CharRange(length2, TranslatorEngine.ChineseToHanViet(ch).Length));
          }
          else
          {
            TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(ch), ref TranslatorEngine.LastTranslatedWord_HanViet, ref length2);
            list.Add(new CharRange(length2, TranslatorEngine.ChineseToHanViet(ch).Length));
          }
        }
        else
        {
          result.Append(ch);
          TranslatorEngine.LastTranslatedWord_HanViet = TranslatorEngine.LastTranslatedWord_HanViet + ch.ToString();
          list.Add(new CharRange(length2, 1));
        }
      }
      if (TranslatorEngine.isChinese(chinese[length1 - 1]))
      {
        TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(chinese[length1 - 1]), ref TranslatorEngine.LastTranslatedWord_HanViet);
        list.Add(new CharRange(((object) result).ToString().Length, TranslatorEngine.ChineseToHanViet(chinese[length1 - 1]).Length));
      }
      else
      {
        result.Append(chinese[length1 - 1]);
        TranslatorEngine.LastTranslatedWord_HanViet = TranslatorEngine.LastTranslatedWord_HanViet + chinese[length1 - 1].ToString();
        list.Add(new CharRange(((object) result).ToString().Length, 1));
      }
      chineseHanVietMappingArray = list.ToArray();
      TranslatorEngine.LastTranslatedWord_HanViet = "";
      return ((object) result).ToString();
    }

    public static string ChineseToHanVietForBrowser(string chinese)
    {
      if (string.IsNullOrEmpty(chinese))
        return "";
      chinese = TranslatorEngine.StandardizeInputForBrowser(chinese);
      StringBuilder stringBuilder = new StringBuilder();
      string[] strArray = TranslatorEngine.classifyWordsIntoLatinAndChinese(chinese);
      int length = strArray.Length;
      for (int index = 0; index < length; ++index)
      {
        string chinese1 = strArray[index];
        if (!string.IsNullOrEmpty(chinese1))
        {
          string text;
          if (TranslatorEngine.isChinese(chinese1[0]))
          {
            CharRange[] chineseHanVietMappingArray;
            text = TranslatorEngine.ChineseToHanViet(chinese1, out chineseHanVietMappingArray).TrimStart();
            if (index == 0 || !strArray[index - 1].EndsWith(", "))
              text = TranslatorEngine.toUpperCase(text);
          }
          else
            text = chinese1;
          stringBuilder.Append(text);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ChineseToHanVietForBatch(string chinese)
    {
      string lastTranslatedWord = "";
      StringBuilder result = new StringBuilder();
      int length = chinese.Length;
      for (int index = 0; index < length - 1; ++index)
      {
        char ch = chinese[index];
        char character = chinese[index + 1];
        if (TranslatorEngine.isChinese(ch))
        {
          if (TranslatorEngine.isChinese(character))
          {
            TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(ch), ref lastTranslatedWord);
            result.Append(" ");
            lastTranslatedWord = lastTranslatedWord + " ";
          }
          else
            TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(ch), ref lastTranslatedWord);
        }
        else
        {
          result.Append(ch);
          lastTranslatedWord = lastTranslatedWord + ch.ToString();
        }
      }
      if (TranslatorEngine.isChinese(chinese[length - 1]))
      {
        TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.ChineseToHanViet(chinese[length - 1]), ref lastTranslatedWord);
      }
      else
      {
        result.Append(chinese[length - 1]);
        lastTranslatedWord = lastTranslatedWord + chinese[length - 1].ToString();
      }
      return ((object) result).ToString();
    }

    public static string ChineseToHanViet(char chinese)
    {
      if ((int) chinese == 32)
        return "";
      if (!TranslatorEngine.hanVietDictionary.ContainsKey(chinese.ToString()))
        return TranslatorEngine.ToNarrow(chinese.ToString());
      else
        return TranslatorEngine.hanVietDictionary[chinese.ToString()];
    }

    public static string ChineseToVietPhrase(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName, out CharRange[] chinesePhraseRanges, out CharRange[] vietPhraseRanges)
    {
      TranslatorEngine.LastTranslatedWord_VietPhrase = "";
      List<CharRange> list1 = new List<CharRange>();
      List<CharRange> list2 = new List<CharRange>();
      StringBuilder result = new StringBuilder();
      int num1 = chinese.Length - 1;
      int index1 = 0;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      TranslatorEngine.loadNhanByDictionary();
      while (index1 <= num1)
      {
        bool flag1 = false;
        bool flag2 = true;
        for (int index2 = 20; index2 > 0; --index2)
        {
          if (chinese.Length >= index1 + index2)
          {
            if (TranslatorEngine.vietPhraseDictionary.ContainsKey(chinese.Substring(index1, index2)))
            {
              if ((!prioritizedName || !TranslatorEngine.containsName(chinese, index1, index2)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || TranslatorEngine.isLongestPhraseInSentence(chinese, index1, index2, TranslatorEngine.vietPhraseDictionary, translationAlgorithm) || prioritizedName && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(index1, index2))))
              {
                list1.Add(new CharRange(index1, index2));
                if (wrapType == 0)
                {
                  TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)], ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length));
                }
                else if (wrapType == 1 || wrapType == 11)
                {
                  TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)] + "]", ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length - 2, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length + 2));
                }
                else if (TranslatorEngine.hasOnlyOneMeaning(TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)]))
                {
                  TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)], ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length));
                }
                else
                {
                  TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)] + "]", ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length - 2, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)].Length + 2));
                }
                if (TranslatorEngine.nextCharIsChinese(chinese, index1 + index2 - 1))
                {
                  result.Append(" ");
                  TranslatorEngine.LastTranslatedWord_VietPhrase = TranslatorEngine.LastTranslatedWord_VietPhrase + " ";
                }
                flag1 = true;
                index1 += index2;
                break;
              }
            }
            else if (!chinese.Substring(index1, index2).Contains("\n") && !chinese.Substring(index1, index2).Contains("\t") && (TranslatorEngine.nhanByDictionary != null && flag2) && (2 < index2 && num2 < index1 + index2 - 1 && TranslatorEngine.IsAllChinese(chinese.Substring(index1, index2))))
            {
              if (index1 < num3)
              {
                if (num3 < index1 + index2 && index2 <= num4 - num3)
                  index2 = num3 - index1 + 1;
              }
              else
              {
                string luatNhan = string.Empty;
                int matchedLength = -1;
                int num5 = TranslatorEngine.containsLuatNhan(chinese.Substring(index1, index2), TranslatorEngine.nhanByDictionary, out luatNhan, out matchedLength);
                num3 = index1 + num5;
                num4 = num3 + matchedLength;
                if (num5 == 0)
                {
                  if (TranslatorEngine.isLongestPhraseInSentence(chinese, index1 - 1, matchedLength - 1, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm))
                  {
                    int length = matchedLength;
                    list1.Add(new CharRange(index1, length));
                    string str = TranslatorEngine.ChineseToLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByDictionary);
                    if (wrapType == 0)
                    {
                      TranslatorEngine.appendTranslatedWord(result, str, ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                      list2.Add(new CharRange(((object) result).ToString().Length - str.Length, str.Length));
                    }
                    else if (wrapType == 1 || wrapType == 11)
                    {
                      TranslatorEngine.appendTranslatedWord(result, "[" + str + "]", ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                      list2.Add(new CharRange(((object) result).ToString().Length - str.Length - 2, str.Length + 2));
                    }
                    else if (TranslatorEngine.hasOnlyOneMeaning(str))
                    {
                      TranslatorEngine.appendTranslatedWord(result, str, ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                      list2.Add(new CharRange(((object) result).ToString().Length - str.Length, str.Length));
                    }
                    else
                    {
                      TranslatorEngine.appendTranslatedWord(result, "[" + str + "]", ref TranslatorEngine.LastTranslatedWord_VietPhrase);
                      list2.Add(new CharRange(((object) result).ToString().Length - str.Length - 2, str.Length + 2));
                    }
                    if (TranslatorEngine.nextCharIsChinese(chinese, index1 + length - 1))
                    {
                      result.Append(" ");
                      TranslatorEngine.LastTranslatedWord_VietPhrase = TranslatorEngine.LastTranslatedWord_VietPhrase + " ";
                    }
                    flag1 = true;
                    index1 += length;
                    break;
                  }
                }
                else if (0 >= num5)
                {
                  num2 = index1 + index2 - 1;
                  flag2 = false;
                  int length = 100;
                  while (index1 + length < chinese.Length && TranslatorEngine.isChinese(chinese[index1 + length - 1]))
                    ++length;
                  if (index1 + length <= chinese.Length && TranslatorEngine.containsLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByDictionary, out luatNhan, out matchedLength) < 0)
                    num2 = index1 + length - 1;
                }
              }
            }
          }
        }
        if (!flag1)
        {
          int length1 = ((object) result).ToString().Length;
          int length2 = TranslatorEngine.ChineseToHanViet(chinese[index1]).Length;
          list1.Add(new CharRange(index1, 1));
          if (TranslatorEngine.isChinese(chinese[index1]))
          {
            TranslatorEngine.appendTranslatedWord(result, (wrapType != 1 ? "" : "[") + TranslatorEngine.ChineseToHanViet(chinese[index1]) + (wrapType != 1 ? "" : "]"), ref TranslatorEngine.LastTranslatedWord_VietPhrase);
            if (TranslatorEngine.nextCharIsChinese(chinese, index1))
            {
              result.Append(" ");
              TranslatorEngine.LastTranslatedWord_VietPhrase = TranslatorEngine.LastTranslatedWord_VietPhrase + " ";
            }
            length2 += wrapType != 1 ? 0 : 2;
          }
          else if (((int) chinese[index1] == 34 || (int) chinese[index1] == 39) && (!TranslatorEngine.LastTranslatedWord_VietPhrase.EndsWith(" ") && !TranslatorEngine.LastTranslatedWord_VietPhrase.EndsWith(".")) && (!TranslatorEngine.LastTranslatedWord_VietPhrase.EndsWith("?") && !TranslatorEngine.LastTranslatedWord_VietPhrase.EndsWith("!") && (!TranslatorEngine.LastTranslatedWord_VietPhrase.EndsWith("\t") && index1 < chinese.Length - 1)) && ((int) chinese[index1 + 1] != 32 && (int) chinese[index1 + 1] != 44))
          {
            result.Append(" ").Append(chinese[index1]);
            TranslatorEngine.LastTranslatedWord_VietPhrase = TranslatorEngine.LastTranslatedWord_VietPhrase + " " + chinese[index1].ToString();
          }
          else
          {
            result.Append(chinese[index1]);
            TranslatorEngine.LastTranslatedWord_VietPhrase = TranslatorEngine.LastTranslatedWord_VietPhrase + chinese[index1].ToString();
            length2 = 1;
          }
          list2.Add(new CharRange(length1, length2));
          ++index1;
        }
      }
      chinesePhraseRanges = list1.ToArray();
      vietPhraseRanges = list2.ToArray();
      TranslatorEngine.LastTranslatedWord_VietPhrase = "";
      return ((object) result).ToString();
    }

    public static string ChineseToVietPhraseForBrowser(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
    {
      chinese = TranslatorEngine.StandardizeInputForBrowser(chinese);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string chinese1 in TranslatorEngine.classifyWordsIntoLatinAndChinese(chinese))
      {
        if (!string.IsNullOrEmpty(chinese1))
        {
          if (TranslatorEngine.isChinese(chinese1[0]))
          {
            CharRange[] chinesePhraseRanges;
            CharRange[] vietPhraseRanges;
            stringBuilder.Append(TranslatorEngine.ChineseToVietPhrase(chinese1, wrapType, translationAlgorithm, prioritizedName, out chinesePhraseRanges, out vietPhraseRanges));
          }
          else
            stringBuilder.Append(chinese1);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ChineseToVietPhraseForBatch(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
    {
      string lastTranslatedWord = "";
      StringBuilder result = new StringBuilder();
      int num1 = chinese.Length - 1;
      int index1 = 0;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      while (index1 <= num1)
      {
        bool flag1 = false;
        bool flag2 = true;
        for (int index2 = 20; index2 > 0; --index2)
        {
          if (chinese.Length >= index1 + index2)
          {
            if (TranslatorEngine.vietPhraseDictionary.ContainsKey(chinese.Substring(index1, index2)))
            {
              if ((!prioritizedName || !TranslatorEngine.containsName(chinese, index1, index2)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || TranslatorEngine.isLongestPhraseInSentence(chinese, index1, index2, TranslatorEngine.vietPhraseDictionary, translationAlgorithm) || prioritizedName && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(index1, index2))))
              {
                if (!string.IsNullOrEmpty(TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)]))
                {
                  if (wrapType == 0)
                    TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)], ref lastTranslatedWord);
                  else if (wrapType == 1 || wrapType == 11)
                    TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)] + "]", ref lastTranslatedWord);
                  else if (TranslatorEngine.hasOnlyOneMeaning(TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)]))
                    TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)], ref lastTranslatedWord);
                  else
                    TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseDictionary[chinese.Substring(index1, index2)] + "]", ref lastTranslatedWord);
                  if (TranslatorEngine.nextCharIsChinese(chinese, index1 + index2 - 1))
                  {
                    result.Append(" ");
                    lastTranslatedWord = lastTranslatedWord + " ";
                  }
                }
                flag1 = true;
                index1 += index2;
                break;
              }
            }
            else if (!chinese.Substring(index1, index2).Contains("\n") && !chinese.Substring(index1, index2).Contains("\t") && (TranslatorEngine.nhanByDictionary != null && flag2) && (2 < index2 && num2 < index1 + index2 - 1 && TranslatorEngine.IsAllChinese(chinese.Substring(index1, index2))))
            {
              if (index1 < num3)
              {
                if (num3 < index1 + index2 && index2 <= num4 - num3)
                  index2 = num3 - index1 + 1;
              }
              else
              {
                string luatNhan = string.Empty;
                int matchedLength = -1;
                int num5 = TranslatorEngine.containsLuatNhan(chinese.Substring(index1, index2), TranslatorEngine.nhanByDictionary, out luatNhan, out matchedLength);
                num3 = index1 + num5;
                num4 = num3 + matchedLength;
                if (num5 == 0)
                {
                  if (TranslatorEngine.isLongestPhraseInSentence(chinese, index1 - 1, matchedLength - 1, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm))
                  {
                    int length = matchedLength;
                    string str = TranslatorEngine.ChineseToLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByDictionary);
                    if (wrapType == 0)
                      TranslatorEngine.appendTranslatedWord(result, str, ref lastTranslatedWord);
                    else if (wrapType == 1 || wrapType == 11)
                      TranslatorEngine.appendTranslatedWord(result, "[" + str + "]", ref lastTranslatedWord);
                    else if (TranslatorEngine.hasOnlyOneMeaning(str))
                      TranslatorEngine.appendTranslatedWord(result, str, ref lastTranslatedWord);
                    else
                      TranslatorEngine.appendTranslatedWord(result, "[" + str + "]", ref lastTranslatedWord);
                    if (TranslatorEngine.nextCharIsChinese(chinese, index1 + length - 1))
                    {
                      result.Append(" ");
                      lastTranslatedWord = lastTranslatedWord + " ";
                    }
                    flag1 = true;
                    index1 += length;
                    break;
                  }
                }
                else if (0 >= num5)
                {
                  num2 = index1 + index2 - 1;
                  flag2 = false;
                  int length = 100;
                  while (index1 + length < chinese.Length && TranslatorEngine.isChinese(chinese[index1 + length - 1]))
                    ++length;
                  if (index1 + length <= chinese.Length && TranslatorEngine.containsLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByDictionary, out luatNhan, out matchedLength) < 0)
                    num2 = index1 + length - 1;
                }
              }
            }
          }
        }
        if (!flag1)
        {
          if (TranslatorEngine.isChinese(chinese[index1]))
          {
            TranslatorEngine.appendTranslatedWord(result, (wrapType != 1 ? "" : "[") + TranslatorEngine.ChineseToHanViet(chinese[index1]) + (wrapType != 1 ? "" : "]"), ref lastTranslatedWord);
            if (TranslatorEngine.nextCharIsChinese(chinese, index1))
            {
              result.Append(" ");
              lastTranslatedWord = lastTranslatedWord + " ";
            }
          }
          else if (((int) chinese[index1] == 34 || (int) chinese[index1] == 39) && (!lastTranslatedWord.EndsWith(" ") && !lastTranslatedWord.EndsWith(".")) && (!lastTranslatedWord.EndsWith("?") && !lastTranslatedWord.EndsWith("!") && (!lastTranslatedWord.EndsWith("\t") && index1 < chinese.Length - 1)) && ((int) chinese[index1 + 1] != 32 && (int) chinese[index1 + 1] != 44))
          {
            result.Append(" ").Append(chinese[index1]);
            lastTranslatedWord = lastTranslatedWord + " " + chinese[index1].ToString();
          }
          else
          {
            result.Append(chinese[index1]);
            lastTranslatedWord = lastTranslatedWord + chinese[index1].ToString();
          }
          ++index1;
        }
      }
      return ((object) result).ToString().Replace("  ", " ");
    }

    public static string ChineseToVietPhraseOneMeaning(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName, out CharRange[] chinesePhraseRanges, out CharRange[] vietPhraseRanges)
    {
      TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = "";
      List<CharRange> list1 = new List<CharRange>();
      List<CharRange> list2 = new List<CharRange>();
      StringBuilder result = new StringBuilder();
      int num1 = chinese.Length - 1;
      int index1 = 0;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      TranslatorEngine.loadNhanByOneMeaningDictionary();
      while (index1 <= num1)
      {
        bool flag1 = false;
        bool flag2 = true;
        for (int index2 = 20; index2 > 0; --index2)
        {
          if (chinese.Length >= index1 + index2)
          {
            if (TranslatorEngine.vietPhraseOneMeaningDictionary.ContainsKey(chinese.Substring(index1, index2)))
            {
              if ((!prioritizedName || !TranslatorEngine.containsName(chinese, index1, index2)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || TranslatorEngine.isLongestPhraseInSentence(chinese, index1, index2, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm) || prioritizedName && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(index1, index2))))
              {
                list1.Add(new CharRange(index1, index2));
                if (wrapType == 0)
                {
                  TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)], ref TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)].Length, TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)].Length));
                }
                else
                {
                  TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)] + "]", ref TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning);
                  list2.Add(new CharRange(((object) result).ToString().Length - TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)].Length - 2, TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)].Length + 2));
                }
                if (TranslatorEngine.nextCharIsChinese(chinese, index1 + index2 - 1))
                {
                  result.Append(" ");
                  TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning + " ";
                }
                flag1 = true;
                index1 += index2;
                break;
              }
            }
            else if (!chinese.Substring(index1, index2).Contains("\n") && !chinese.Substring(index1, index2).Contains("\t") && (TranslatorEngine.nhanByOneMeaningDictionary != null && flag2) && (2 < index2 && num2 < index1 + index2 - 1 && TranslatorEngine.IsAllChinese(chinese.Substring(index1, index2))))
            {
              if (index1 < num3)
              {
                if (num3 < index1 + index2 && index2 <= num4 - num3)
                  index2 = num3 - index1 + 1;
              }
              else
              {
                string luatNhan = string.Empty;
                int matchedLength = -1;
                int num5 = TranslatorEngine.containsLuatNhan(chinese.Substring(index1, index2), TranslatorEngine.nhanByOneMeaningDictionary, out luatNhan, out matchedLength);
                num3 = index1 + num5;
                num4 = num3 + matchedLength;
                if (num5 == 0)
                {
                  if (TranslatorEngine.isLongestPhraseInSentence(chinese, index1 - 1, matchedLength - 1, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm))
                  {
                    int length = matchedLength;
                    list1.Add(new CharRange(index1, length));
                    string translatedText = TranslatorEngine.ChineseToLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByOneMeaningDictionary);
                    if (wrapType == 0)
                    {
                      TranslatorEngine.appendTranslatedWord(result, translatedText, ref TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning);
                      list2.Add(new CharRange(((object) result).ToString().Length - translatedText.Length, translatedText.Length));
                    }
                    else
                    {
                      TranslatorEngine.appendTranslatedWord(result, "[" + translatedText + "]", ref TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning);
                      list2.Add(new CharRange(((object) result).ToString().Length - translatedText.Length - 2, translatedText.Length + 2));
                    }
                    if (TranslatorEngine.nextCharIsChinese(chinese, index1 + length - 1))
                    {
                      result.Append(" ");
                      TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning + " ";
                    }
                    flag1 = true;
                    index1 += length;
                    break;
                  }
                }
                else if (0 >= num5)
                {
                  num2 = index1 + index2 - 1;
                  flag2 = false;
                  int length = 100;
                  while (index1 + length < chinese.Length && TranslatorEngine.isChinese(chinese[index1 + length - 1]))
                    ++length;
                  if (index1 + length <= chinese.Length && TranslatorEngine.containsLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByOneMeaningDictionary, out luatNhan, out matchedLength) < 0)
                    num2 = index1 + length - 1;
                }
              }
            }
          }
        }
        if (!flag1)
        {
          int length1 = ((object) result).ToString().Length;
          int length2 = TranslatorEngine.ChineseToHanViet(chinese[index1]).Length;
          list1.Add(new CharRange(index1, 1));
          if (TranslatorEngine.isChinese(chinese[index1]))
          {
            TranslatorEngine.appendTranslatedWord(result, (wrapType != 1 ? "" : "[") + TranslatorEngine.ChineseToHanViet(chinese[index1]) + (wrapType != 1 ? "" : "]"), ref TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning);
            if (TranslatorEngine.nextCharIsChinese(chinese, index1))
            {
              result.Append(" ");
              TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning + " ";
            }
            length2 += wrapType != 1 ? 0 : 2;
          }
          else if (((int) chinese[index1] == 34 || (int) chinese[index1] == 39) && (!TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning.EndsWith(" ") && !TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning.EndsWith(".")) && (!TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning.EndsWith("?") && !TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning.EndsWith("!") && (!TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning.EndsWith("\t") && index1 < chinese.Length - 1)) && ((int) chinese[index1 + 1] != 32 && (int) chinese[index1 + 1] != 44))
          {
            result.Append(" ").Append(chinese[index1]);
            TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning + " " + chinese[index1].ToString();
          }
          else
          {
            result.Append(chinese[index1]);
            TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning + chinese[index1].ToString();
            length2 = 1;
          }
          list2.Add(new CharRange(length1, length2));
          ++index1;
        }
      }
      chinesePhraseRanges = list1.ToArray();
      vietPhraseRanges = list2.ToArray();
      TranslatorEngine.LastTranslatedWord_VietPhraseOneMeaning = "";
      return ((object) result).ToString();
    }

    public static string ChineseToVietPhraseOneMeaningForBrowser(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
    {
      chinese = TranslatorEngine.StandardizeInputForBrowser(chinese);
      StringBuilder stringBuilder = new StringBuilder();
      string[] strArray = TranslatorEngine.classifyWordsIntoLatinAndChinese(chinese);
      int length = strArray.Length;
      for (int index = 0; index < length; ++index)
      {
        string chinese1 = strArray[index];
        if (!string.IsNullOrEmpty(chinese1))
        {
          string text;
          if (TranslatorEngine.isChinese(chinese1[0]))
          {
            CharRange[] chinesePhraseRanges;
            CharRange[] vietPhraseRanges;
            text = TranslatorEngine.ChineseToVietPhraseOneMeaning(chinese1, wrapType, translationAlgorithm, prioritizedName, out chinesePhraseRanges, out vietPhraseRanges).TrimStart();
            if (index == 0 || !strArray[index - 1].EndsWith(", "))
              text = TranslatorEngine.toUpperCase(text);
          }
          else
            text = chinese1;
          stringBuilder.Append(text);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ChineseToVietPhraseOneMeaningForProxy(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
    {
      chinese = TranslatorEngine.StandardizeInputForProxy(chinese);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string chinese1 in TranslatorEngine.classifyWordsIntoLatinAndChineseForProxy(chinese))
      {
        if (!string.IsNullOrEmpty(chinese1))
        {
          if (TranslatorEngine.isChinese(chinese1[0]))
          {
            CharRange[] chinesePhraseRanges;
            CharRange[] vietPhraseRanges;
            stringBuilder.Append(TranslatorEngine.ChineseToVietPhraseOneMeaning(chinese1, wrapType, translationAlgorithm, prioritizedName, out chinesePhraseRanges, out vietPhraseRanges));
          }
          else
            stringBuilder.Append(chinese1);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ChineseToVietPhraseOneMeaningForBatch(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
    {
      DateTime now = DateTime.Now;
      string lastTranslatedWord = "";
      StringBuilder result = new StringBuilder();
      int num1 = chinese.Length - 1;
      int index1 = 0;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      while (index1 <= num1)
      {
        bool flag1 = false;
        bool flag2 = true;
        if ((int) chinese[index1] != 10 && (int) chinese[index1] != 9)
        {
          for (int index2 = 20; index2 > 0; --index2)
          {
            if (chinese.Length >= index1 + index2)
            {
              if (TranslatorEngine.vietPhraseOneMeaningDictionary.ContainsKey(chinese.Substring(index1, index2)))
              {
                if ((!prioritizedName || !TranslatorEngine.containsName(chinese, index1, index2)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || TranslatorEngine.isLongestPhraseInSentence(chinese, index1, index2, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm) || prioritizedName && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(index1, index2))))
                {
                  if (!string.IsNullOrEmpty(TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)]))
                  {
                    if (wrapType == 0)
                      TranslatorEngine.appendTranslatedWord(result, TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)], ref lastTranslatedWord);
                    else
                      TranslatorEngine.appendTranslatedWord(result, "[" + TranslatorEngine.vietPhraseOneMeaningDictionary[chinese.Substring(index1, index2)] + "]", ref lastTranslatedWord);
                    if (TranslatorEngine.nextCharIsChinese(chinese, index1 + index2 - 1))
                    {
                      result.Append(" ");
                      lastTranslatedWord = lastTranslatedWord + " ";
                    }
                  }
                  flag1 = true;
                  index1 += index2;
                  break;
                }
              }
              else if (!chinese.Substring(index1, index2).Contains("\n") && !chinese.Substring(index1, index2).Contains("\t") && (TranslatorEngine.nhanByOneMeaningDictionary != null && flag2) && (2 < index2 && num2 < index1 + index2 - 1 && TranslatorEngine.IsAllChinese(chinese.Substring(index1, index2))))
              {
                if (index1 < num3)
                {
                  if (num3 < index1 + index2 && index2 <= num4 - num3)
                    index2 = num3 - index1 + 1;
                }
                else
                {
                  string luatNhan = string.Empty;
                  int matchedLength = -1;
                  int num5 = TranslatorEngine.containsLuatNhan(chinese.Substring(index1, index2), TranslatorEngine.nhanByOneMeaningDictionary, out luatNhan, out matchedLength);
                  num3 = index1 + num5;
                  num4 = num3 + matchedLength;
                  if (num5 == 0)
                  {
                    if (TranslatorEngine.isLongestPhraseInSentence(chinese, index1 - 1, matchedLength - 1, TranslatorEngine.vietPhraseOneMeaningDictionary, translationAlgorithm))
                    {
                      int length = matchedLength;
                      string translatedText = TranslatorEngine.ChineseToLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByOneMeaningDictionary);
                      if (wrapType == 0)
                        TranslatorEngine.appendTranslatedWord(result, translatedText, ref lastTranslatedWord);
                      else
                        TranslatorEngine.appendTranslatedWord(result, "[" + translatedText + "]", ref lastTranslatedWord);
                      if (TranslatorEngine.nextCharIsChinese(chinese, index1 + length - 1))
                      {
                        result.Append(" ");
                        lastTranslatedWord = lastTranslatedWord + " ";
                      }
                      flag1 = true;
                      index1 += length;
                      break;
                    }
                  }
                  else if (0 >= num5)
                  {
                    num2 = index1 + index2 - 1;
                    flag2 = false;
                    int length = 100;
                    while (index1 + length < chinese.Length && TranslatorEngine.isChinese(chinese[index1 + length - 1]))
                      ++length;
                    if (index1 + length <= chinese.Length && TranslatorEngine.containsLuatNhan(chinese.Substring(index1, length), TranslatorEngine.nhanByOneMeaningDictionary, out luatNhan, out matchedLength) < 0)
                      num2 = index1 + length - 1;
                  }
                }
              }
            }
          }
        }
        if (!flag1)
        {
          if (TranslatorEngine.isChinese(chinese[index1]))
          {
            TranslatorEngine.appendTranslatedWord(result, (wrapType != 1 ? "" : "[") + TranslatorEngine.ChineseToHanViet(chinese[index1]) + (wrapType != 1 ? "" : "]"), ref lastTranslatedWord);
            if (TranslatorEngine.nextCharIsChinese(chinese, index1))
            {
              result.Append(" ");
              lastTranslatedWord = lastTranslatedWord + " ";
            }
          }
          else if (((int) chinese[index1] == 34 || (int) chinese[index1] == 39) && (!lastTranslatedWord.EndsWith(" ") && !lastTranslatedWord.EndsWith(".")) && (!lastTranslatedWord.EndsWith("?") && !lastTranslatedWord.EndsWith("!") && (!lastTranslatedWord.EndsWith("\t") && index1 < chinese.Length - 1)) && ((int) chinese[index1 + 1] != 32 && (int) chinese[index1 + 1] != 44))
          {
            result.Append(" ").Append(chinese[index1]);
            lastTranslatedWord = lastTranslatedWord + " " + chinese[index1].ToString();
          }
          else
          {
            result.Append(chinese[index1]);
            lastTranslatedWord = lastTranslatedWord + chinese[index1].ToString();
          }
          ++index1;
        }
      }
      return ((object) result).ToString();
    }

    public static string ChineseToNameForBatch(string chinese)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = chinese.Length - 1;
      int startIndex = 0;
      while (startIndex <= num)
      {
        bool flag = false;
        if (TranslatorEngine.isChinese(chinese[startIndex]))
        {
          for (int length = 20; length > 0; --length)
          {
            if (chinese.Length >= startIndex + length && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(startIndex, length)))
            {
              stringBuilder.Append(TranslatorEngine.onlyNameDictionary[chinese.Substring(startIndex, length)]);
              flag = true;
              startIndex += length;
              break;
            }
          }
        }
        if (!flag)
        {
          stringBuilder.Append(chinese[startIndex]);
          ++startIndex;
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ChineseToMeanings(string chinese, out int phraseTranslatedLength)
    {
      string str1 = "";
      if (chinese.Length == 0)
      {
        phraseTranslatedLength = 0;
        return "";
      }
      else
      {
        int num = 0;
        for (int length = 20; length > 0; --length)
        {
          if (chinese.Length >= length && !chinese.Substring(0, length).Contains("\n") && !chinese.Substring(0, length).Contains("\t"))
          {
            if (TranslatorEngine.containsLuatNhan(chinese.Substring(0, length), TranslatorEngine.vietPhraseDictionary) == 0)
            {
              if (TranslatorEngine.matchesLuatNhan(chinese.Substring(0, length), TranslatorEngine.vietPhraseDictionary))
              {
                string luatNhan = string.Empty;
                TranslatorEngine.ChineseToLuatNhan(chinese.Substring(0, length), TranslatorEngine.vietPhraseDictionary, out luatNhan);
                str1 = str1 + luatNhan + " <<Luật Nhân>> " + TranslatorEngine.luatNhanDictionary[luatNhan].Replace("/", "; ") + "\n-----------------\n";
                if (num == 0)
                  num = length;
              }
            }
            else
              break;
          }
        }
        for (int length = 20; length > 0; --length)
        {
          if (chinese.Length >= length)
          {
            string key = chinese.Substring(0, length);
            if (TranslatorEngine.vietPhraseDictionary.ContainsKey(key))
            {
              str1 = str1 + key + " <<VietPhrase>> " + TranslatorEngine.vietPhraseDictionary[key].Replace("/", "; ") + "\n-----------------\n";
              if (num == 0)
                num = key.Length;
            }
          }
        }
        for (int length = 20; length > 0; --length)
        {
          if (chinese.Length >= length)
          {
            string key = chinese.Substring(0, length);
            if (TranslatorEngine.lacVietDictionary.ContainsKey(key))
            {
              str1 = str1 + key + " <<Lạc Việt>>\n" + TranslatorEngine.lacVietDictionary[key] + "\n-----------------\n";
              if (num == 0)
                num = 1;
            }
          }
        }
        for (int length = 20; length > 0; --length)
        {
          if (chinese.Length >= length)
          {
            string key = chinese.Substring(0, length);
            if (TranslatorEngine.cedictDictionary.ContainsKey(key))
            {
              str1 = str1 + key + " <<Cedict or Babylon>> " + TranslatorEngine.cedictDictionary[key].Replace("] /", "] ").Replace("/", "; ") + "\n-----------------\n";
              if (num == 0)
                num = 1;
            }
          }
        }
        if (TranslatorEngine.thieuChuuDictionary.ContainsKey(chinese[0].ToString()))
        {
          num = num == 0 ? 1 : num;
          str1 = str1 + (object) chinese[0] + " <<Thiều Chửu>> " + TranslatorEngine.thieuChuuDictionary[chinese[0].ToString()] + "\n-----------------\n";
        }
        int length1 = chinese.Length < 10 ? chinese.Length : 10;
        string str2 = str1 + chinese.Substring(0, length1).Trim("\n\t ".ToCharArray()) + " <<Phiên Âm English>> ";
        for (int index = 0; index < length1; ++index)
          str2 = !TranslatorEngine.chinesePhienAmEnglishDictionary.ContainsKey(chinese[index].ToString()) ? str2 + TranslatorEngine.ChineseToHanViet(chinese[index]) + " " : str2 + "[" + TranslatorEngine.chinesePhienAmEnglishDictionary[chinese[index].ToString()] + "] ";
        if (num == 0)
        {
          num = 1;
          str2 = (string) (object) chinese[0] + (object) "\n-----------------\nNot Found";
        }
        phraseTranslatedLength = num;
        return str2;
      }
    }

    public static void LoadDictionaries()
    {
      lock (TranslatorEngine.lockObject)
      {
        if (!TranslatorEngine.dictionaryDirty)
          return;
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadHanVietDictionaryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadThieuChuuDictionaryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadLacVietDictionaryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadCedictDictionaryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadChinesePhienAmEnglishDictionaryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadIgnoredChinesePhraseListsWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadOnlyNameDictionaryHistoryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadOnlyNamePhuDictionaryHistoryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadOnlyVietPhraseDictionaryHistoryWithNewThread));
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadHanVietDictionaryHistoryWithNewThread));
        ManualResetEvent[] local_0 = new ManualResetEvent[1]
        {
          new ManualResetEvent(false)
        };
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadLuatNhanDictionaryWithNewThread), (object) local_0[0]);
        ManualResetEvent[] local_1 = new ManualResetEvent[1]
        {
          new ManualResetEvent(false)
        };
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadPronounDictionaryWithNewThread), (object) local_1[0]);
        ManualResetEvent[] local_2 = new ManualResetEvent[1]
        {
          new ManualResetEvent(false)
        };
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadOnlyVietPhraseDictionaryWithNewThread), (object) local_2[0]);
        ManualResetEvent[] local_3 = new ManualResetEvent[1]
        {
          new ManualResetEvent(false)
        };
        ThreadPool.QueueUserWorkItem(new WaitCallback(TranslatorEngine.loadOnlyNameDictionaryWithNewThread), (object) local_3[0]);
        WaitHandle.WaitAll((WaitHandle[]) local_1);
        WaitHandle.WaitAll((WaitHandle[]) local_2);
        WaitHandle.WaitAll((WaitHandle[]) local_3);
        TranslatorEngine.loadVietPhraseDictionary();
        TranslatorEngine.vietPhraseDictionaryToVietPhraseOneMeaningDictionary();
        TranslatorEngine.pronounDictionaryToPronounOneMeaningDictionary();
        TranslatorEngine.loadNhanByDictionary();
        TranslatorEngine.loadNhanByOneMeaningDictionary();
        WaitHandle.WaitAll((WaitHandle[]) local_0);
        TranslatorEngine.dictionaryDirty = false;
      }
    }

    private static void loadLuatNhanDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadLuatNhanDictionary();
      ((EventWaitHandle) stateInfo).Set();
    }

    private static void loadLuatNhanDictionary()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetLuatNhanDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          if (!str.StartsWith("#"))
          {
            string[] strArray = str.Split('=');
            if (strArray.Length == 2 && !dictionary.ContainsKey(strArray[0]))
              dictionary.Add(strArray[0], strArray[1]);
          }
        }
      }
      IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = Enumerable.ThenBy<KeyValuePair<string, string>, string>(Enumerable.OrderByDescending<KeyValuePair<string, string>, int>((IEnumerable<KeyValuePair<string, string>>) dictionary, (Func<KeyValuePair<string, string>, int>) (pair => pair.Key.Length)), (Func<KeyValuePair<string, string>, string>) (pair => pair.Key));
      TranslatorEngine.luatNhanDictionary.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) orderedEnumerable)
        TranslatorEngine.luatNhanDictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }

    private static int compareLuatNhan(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
    {
      if (x.Key.StartsWith("{0}") || x.Key.EndsWith("{0}"))
      {
        if (!y.Key.StartsWith("{0}") && !y.Key.EndsWith("{0}"))
          return 1;
      }
      else if (y.Key.StartsWith("{0}") || y.Key.EndsWith("{0}"))
        return -1;
      return y.Key.Length - x.Key.Length;
    }

    private static void loadHanVietDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadHanVietDictionary();
    }

    private static void loadHanVietDictionary()
    {
      TranslatorEngine.hanVietDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.hanVietDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.hanVietDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadVietPhraseDictionary()
    {
      TranslatorEngine.vietPhraseDictionary.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.onlyNameDictionary)
      {
        if (!TranslatorEngine.vietPhraseDictionary.ContainsKey(keyValuePair.Key))
          TranslatorEngine.vietPhraseDictionary.Add(keyValuePair.Key, keyValuePair.Value);
      }
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.onlyVietPhraseDictionary)
      {
        if (!TranslatorEngine.vietPhraseDictionary.ContainsKey(keyValuePair.Key))
          TranslatorEngine.vietPhraseDictionary.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }

    private static void loadOnlyVietPhraseDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadOnlyVietPhraseDictionary();
      ((EventWaitHandle) stateInfo).Set();
    }

    private static void loadOnlyVietPhraseDictionary()
    {
      TranslatorEngine.onlyVietPhraseDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetVietPhraseDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.onlyVietPhraseDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.onlyVietPhraseDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadOnlyNameDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadOnlyNameDictionary();
      ((EventWaitHandle) stateInfo).Set();
    }

    private static void loadOnlyNameDictionary()
    {
      TranslatorEngine.onlyNameDictionary.Clear();
      TranslatorEngine.onlyNameOneMeaningDictionary.Clear();
      TranslatorEngine.onlyNameChinhDictionary.Clear();
      TranslatorEngine.onlyNamePhuDictionary.Clear();
      char[] chArray = "/|".ToCharArray();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetNamesDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.onlyNameDictionary.ContainsKey(strArray[0]))
          {
            TranslatorEngine.onlyNameDictionary.Add(strArray[0], strArray[1]);
            TranslatorEngine.onlyNameOneMeaningDictionary.Add(strArray[0], strArray[1].Split(chArray)[0]);
            TranslatorEngine.onlyNameChinhDictionary.Add(strArray[0], strArray[1]);
          }
        }
      }
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetNamesPhuDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.onlyNamePhuDictionary.ContainsKey(strArray[0]))
          {
            if (TranslatorEngine.onlyNameDictionary.ContainsKey(strArray[0]))
            {
              TranslatorEngine.onlyNameDictionary[strArray[0]] = strArray[1];
              TranslatorEngine.onlyNameOneMeaningDictionary[strArray[0]] = strArray[1].Split(chArray)[0];
            }
            else
            {
              TranslatorEngine.onlyNameDictionary.Add(strArray[0], strArray[1]);
              TranslatorEngine.onlyNameOneMeaningDictionary.Add(strArray[0], strArray[1].Split(chArray)[0]);
            }
            TranslatorEngine.onlyNamePhuDictionary.Add(strArray[0], strArray[1]);
          }
        }
      }
    }

    private static void vietPhraseDictionaryToVietPhraseOneMeaningDictionary()
    {
      TranslatorEngine.vietPhraseOneMeaningDictionary.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.vietPhraseDictionary)
      {
        Dictionary<string, string> dictionary = TranslatorEngine.vietPhraseOneMeaningDictionary;
        string key = keyValuePair.Key;
        string str;
        if (!keyValuePair.Value.Contains("/") && !keyValuePair.Value.Contains("|"))
          str = keyValuePair.Value;
        else
          str = keyValuePair.Value.Split(new char[2]
          {
            '/',
            '|'
          })[0];
        dictionary.Add(key, str);
      }
    }

    private static void pronounDictionaryToPronounOneMeaningDictionary()
    {
      TranslatorEngine.pronounOneMeaningDictionary.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.pronounDictionary)
      {
        Dictionary<string, string> dictionary = TranslatorEngine.pronounOneMeaningDictionary;
        string key = keyValuePair.Key;
        string str;
        if (!keyValuePair.Value.Contains("/") && !keyValuePair.Value.Contains("|"))
          str = keyValuePair.Value;
        else
          str = keyValuePair.Value.Split(new char[2]
          {
            '/',
            '|'
          })[0];
        dictionary.Add(key, str);
      }
    }

    private static void loadNhanByDictionary()
    {
      if (DictionaryConfigurationHelper.IsNhanByPronouns)
        TranslatorEngine.nhanByDictionary = TranslatorEngine.pronounDictionary;
      else if (DictionaryConfigurationHelper.IsNhanByPronounsAndNames)
      {
        TranslatorEngine.nhanByDictionary = new Dictionary<string, string>((IDictionary<string, string>) TranslatorEngine.pronounDictionary);
        foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.onlyNameDictionary)
        {
          if (!TranslatorEngine.nhanByDictionary.ContainsKey(keyValuePair.Key))
            TranslatorEngine.nhanByDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else if (DictionaryConfigurationHelper.IsNhanByPronounsAndNamesAndVietPhrase)
      {
        TranslatorEngine.nhanByDictionary = new Dictionary<string, string>((IDictionary<string, string>) TranslatorEngine.pronounDictionary);
        foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.vietPhraseDictionary)
        {
          if (!TranslatorEngine.nhanByDictionary.ContainsKey(keyValuePair.Key))
            TranslatorEngine.nhanByDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else
        TranslatorEngine.nhanByDictionary = (Dictionary<string, string>) null;
    }

    private static void loadNhanByOneMeaningDictionary()
    {
      if (DictionaryConfigurationHelper.IsNhanByPronouns)
        TranslatorEngine.nhanByOneMeaningDictionary = TranslatorEngine.pronounOneMeaningDictionary;
      else if (DictionaryConfigurationHelper.IsNhanByPronounsAndNames)
      {
        TranslatorEngine.nhanByOneMeaningDictionary = new Dictionary<string, string>((IDictionary<string, string>) TranslatorEngine.pronounOneMeaningDictionary);
        foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.onlyNameOneMeaningDictionary)
        {
          if (!TranslatorEngine.nhanByOneMeaningDictionary.ContainsKey(keyValuePair.Key))
            TranslatorEngine.nhanByOneMeaningDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else if (DictionaryConfigurationHelper.IsNhanByPronounsAndNamesAndVietPhrase)
      {
        TranslatorEngine.nhanByOneMeaningDictionary = new Dictionary<string, string>((IDictionary<string, string>) TranslatorEngine.pronounOneMeaningDictionary);
        foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.vietPhraseOneMeaningDictionary)
        {
          if (!TranslatorEngine.nhanByOneMeaningDictionary.ContainsKey(keyValuePair.Key))
            TranslatorEngine.nhanByOneMeaningDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else
        TranslatorEngine.nhanByOneMeaningDictionary = (Dictionary<string, string>) null;
    }

    private static void loadThieuChuuDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadThieuChuuDictionary();
    }

    private static void loadThieuChuuDictionary()
    {
      TranslatorEngine.thieuChuuDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetThieuChuuDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.thieuChuuDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.thieuChuuDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadLacVietDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadLacVietDictionary();
    }

    private static void loadLacVietDictionary()
    {
      TranslatorEngine.lacVietDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetLacVietDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.lacVietDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.lacVietDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadCedictDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadCedictDictionary();
    }

    private static void loadChinesePhienAmEnglishDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadChinesePhienAmEnglishDictionary();
    }

    private static void loadPronounDictionaryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadPronounDictionary();
      ((EventWaitHandle) stateInfo).Set();
    }

    private static void loadIgnoredChinesePhraseListsWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadIgnoredChinesePhraseLists();
    }

    private static void loadOnlyVietPhraseDictionaryHistoryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadOnlyVietPhraseDictionaryHistory();
    }

    private static void loadOnlyNameDictionaryHistoryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadOnlyNameDictionaryHistory();
    }

    private static void loadOnlyNamePhuDictionaryHistoryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadOnlyNamePhuDictionaryHistory();
    }

    private static void loadHanVietDictionaryHistoryWithNewThread(object stateInfo)
    {
      TranslatorEngine.loadHanVietDictionaryHistory();
    }

    private static void loadOnlyVietPhraseDictionaryHistory()
    {
      TranslatorEngine.LoadDictionaryHistory(DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath(), ref TranslatorEngine.onlyVietPhraseDictionaryHistoryDataSet);
    }

    private static void loadOnlyNameDictionaryHistory()
    {
      TranslatorEngine.LoadDictionaryHistory(DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath(), ref TranslatorEngine.onlyNameDictionaryHistoryDataSet);
    }

    private static void loadOnlyNamePhuDictionaryHistory()
    {
      TranslatorEngine.LoadDictionaryHistory(DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath(), ref TranslatorEngine.onlyNamePhuDictionaryHistoryDataSet);
    }

    private static void loadHanVietDictionaryHistory()
    {
      TranslatorEngine.LoadDictionaryHistory(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath(), ref TranslatorEngine.hanVietDictionaryHistoryDataSet);
    }

    public static void LoadDictionaryHistory(string dictionaryHistoryPath, ref DataSet dictionaryHistoryDataSet)
    {
      dictionaryHistoryDataSet.Clear();
      string name1 = "DictionaryHistory";
      if (!dictionaryHistoryDataSet.Tables.Contains(name1))
      {
        dictionaryHistoryDataSet.Tables.Add(name1);
        dictionaryHistoryDataSet.Tables[name1].Columns.Add("Entry", Type.GetType("System.String"));
        dictionaryHistoryDataSet.Tables[name1].Columns.Add("Action", Type.GetType("System.String"));
        dictionaryHistoryDataSet.Tables[name1].Columns.Add("User Name", Type.GetType("System.String"));
        dictionaryHistoryDataSet.Tables[name1].Columns.Add("Updated Date", Type.GetType("System.DateTime"));
        dictionaryHistoryDataSet.Tables[name1].PrimaryKey = new DataColumn[1]
        {
          dictionaryHistoryDataSet.Tables[name1].Columns["Entry"]
        };
      }
      if (!File.Exists(dictionaryHistoryPath))
        return;
      string name2 = "GB2312";
      using (TextReader textReader = (TextReader) new StreamReader(dictionaryHistoryPath, Encoding.GetEncoding(name2)))
      {
        textReader.ReadLine();
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('\t');
          if (strArray.Length == 4)
          {
            DataRow dataRow = dictionaryHistoryDataSet.Tables[name1].Rows.Find((object) strArray[0]);
            if (dataRow == null)
            {
              dictionaryHistoryDataSet.Tables[name1].Rows.Add((object) strArray[0], (object) strArray[1], (object) strArray[2], (object) DateTime.ParseExact(strArray[3], "yyyy-MM-dd HH:mm:ss.fffzzz", (IFormatProvider) null));
            }
            else
            {
              dataRow[1] = (object) strArray[1];
              dataRow[2] = (object) strArray[2];
              dataRow[3] = (object) DateTime.ParseExact(strArray[3], "yyyy-MM-dd HH:mm:ss.fffzzz", (IFormatProvider) null);
            }
          }
        }
      }
    }

    private static void loadCedictDictionary()
    {
      TranslatorEngine.cedictDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetCEDictDictionaryPath(), Encoding.UTF8))
      {
        string str1;
        while ((str1 = textReader.ReadLine()) != null)
        {
          if (!str1.StartsWith("#"))
          {
            string str2 = str1.Substring(0, str1.IndexOf(" ["));
            char[] chArray = new char[1]
            {
              ' '
            };
            foreach (string key in str2.Split(chArray))
            {
              if (!TranslatorEngine.cedictDictionary.ContainsKey(key))
                TranslatorEngine.cedictDictionary.Add(key, str1.Substring(str1.IndexOf(" [")));
            }
          }
        }
      }
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetBabylonDictionaryPath(), Encoding.UTF8))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (!TranslatorEngine.cedictDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.cedictDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadChinesePhienAmEnglishDictionary()
    {
      TranslatorEngine.chinesePhienAmEnglishDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetChinesePhienAmEnglishWordsDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.chinesePhienAmEnglishDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.chinesePhienAmEnglishDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    private static void loadPronounDictionary()
    {
      TranslatorEngine.pronounDictionary.Clear();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetPronounsDictionaryPath(), true))
      {
        string str;
        while ((str = textReader.ReadLine()) != null)
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && !TranslatorEngine.pronounDictionary.ContainsKey(strArray[0]))
            TranslatorEngine.pronounDictionary.Add(strArray[0], strArray[1]);
        }
      }
    }

    public static void AddIgnoredChinesePhrase(string ignoredChinesePhrase)
    {
      if (TranslatorEngine.ignoredChinesePhraseList.Contains(ignoredChinesePhrase))
        return;
      TranslatorEngine.ignoredChinesePhraseList.Add(ignoredChinesePhrase);
      try
      {
        File.WriteAllLines(DictionaryConfigurationHelper.GetIgnoredChinesePhraseListPath(), TranslatorEngine.ignoredChinesePhraseList.ToArray(), Encoding.UTF8);
      }
      catch
      {
      }
      TranslatorEngine.loadIgnoredChinesePhraseLists();
    }

    private static void loadIgnoredChinesePhraseLists()
    {
      TranslatorEngine.ignoredChinesePhraseList.Clear();
      TranslatorEngine.ignoredChinesePhraseForBrowserList.Clear();
      char[] chArray = "\t\n".ToCharArray();
      using (TextReader textReader = (TextReader) new StreamReader(DictionaryConfigurationHelper.GetIgnoredChinesePhraseListPath(), true))
      {
        string original;
        while ((original = textReader.ReadLine()) != null)
        {
          if (!string.IsNullOrEmpty(original))
          {
            string str1 = TranslatorEngine.standardizeInputWithoutRemovingIgnoredChinesePhrases(original).Trim(chArray);
            if (!string.IsNullOrEmpty(str1) && !TranslatorEngine.ignoredChinesePhraseList.Contains(str1))
              TranslatorEngine.ignoredChinesePhraseList.Add(str1);
            string str2 = TranslatorEngine.standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(original).Trim(chArray);
            if (!string.IsNullOrEmpty(str2) && !TranslatorEngine.ignoredChinesePhraseForBrowserList.Contains(str2))
              TranslatorEngine.ignoredChinesePhraseForBrowserList.Add(str2);
          }
        }
      }
      TranslatorEngine.ignoredChinesePhraseList.Sort(new Comparison<string>(TranslatorEngine.compareStringByDescending));
      TranslatorEngine.ignoredChinesePhraseForBrowserList.Sort(new Comparison<string>(TranslatorEngine.compareStringByDescending));
    }

    private static int compareStringByDescending(string x, string y)
    {
      if (x == null)
      {
        return y == null ? 0 : 1;
      }
      else
      {
        if (y == null)
          return -1;
        int num = x.Length.CompareTo(y.Length);
        if (num != 0)
          return num * -1;
        else
          return x.CompareTo(y) * -1;
      }
    }

    public static string StandardizeInput(string original)
    {
      return TranslatorEngine.removeIgnoredChinesePhrases(TranslatorEngine.standardizeInputWithoutRemovingIgnoredChinesePhrases(original));
    }

    private static string standardizeInputWithoutRemovingIgnoredChinesePhrases(string original)
    {
      if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
        return "";
      string str1 = TranslatorEngine.ToSimplified(original);
      string[] strArray1 = new string[16]
      {
        "，",
        "。",
        "：",
        "“",
        "”",
        "‘",
        "’",
        "？",
        "！",
        "「",
        "」",
        "．",
        "、",
        "　",
        "…",
        TranslatorEngine.NULL_STRING
      };
      string[] strArray2 = new string[16]
      {
        ", ",
        ".",
        ": ",
        "\"",
        "\" ",
        "'",
        "' ",
        "?",
        "!",
        "\"",
        "\" ",
        ".",
        ", ",
        " ",
        "...",
        ""
      };
      for (int index = 0; index < strArray1.Length; ++index)
        str1 = str1.Replace(strArray1[index], strArray2[index]);
      string str2 = TranslatorEngine.ToNarrow(str1.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n").Replace(" ,", ","));
      int length = str2.Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < length - 1; ++index)
      {
        char ch = str2[index];
        char character = str2[index + 1];
        if (!char.IsControl(ch) || (int) ch == 9 || ((int) ch == 10 || (int) ch == 13))
        {
          if (TranslatorEngine.isChinese(ch))
          {
            if (!TranslatorEngine.isChinese(character) && (int) character != 44 && ((int) character != 46 && (int) character != 58) && ((int) character != 59 && (int) character != 34 && ((int) character != 39 && (int) character != 63)) && ((int) character != 32 && (int) character != 33 && (int) character != 41))
              stringBuilder.Append(ch).Append(" ");
            else
              stringBuilder.Append(ch);
          }
          else if ((int) ch == 9 || (int) ch == 32 || ((int) ch == 34 || (int) ch == 39) || ((int) ch == 10 || (int) ch == 40))
            stringBuilder.Append(ch);
          else if ((int) ch == 33 || (int) ch == 46 || (int) ch == 63)
          {
            if ((int) character == 34 || (int) character == 32 || (int) character == 39)
              stringBuilder.Append(ch);
            else
              stringBuilder.Append(ch).Append(" ");
          }
          else if (TranslatorEngine.isChinese(character))
            stringBuilder.Append(ch).Append(" ");
          else
            stringBuilder.Append(ch);
        }
      }
      stringBuilder.Append(str2[length - 1]);
      return TranslatorEngine.indentAllLines(((object) stringBuilder).ToString(), true).Replace(". . . . . .", "...");
    }

    public static string StandardizeInputForBrowser(string original)
    {
      return TranslatorEngine.removeIgnoredChinesePhrasesForBrowser(TranslatorEngine.standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(original));
    }

    private static string standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(string original)
    {
      if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
        return "";
      string str1 = TranslatorEngine.ToSimplified(original);
      string[] strArray1 = new string[16]
      {
        "，",
        "。",
        "：",
        "“",
        "”",
        "‘",
        "’",
        "？",
        "！",
        "「",
        "」",
        "．",
        "、",
        "　",
        "…",
        TranslatorEngine.NULL_STRING
      };
      string[] strArray2 = new string[16]
      {
        ", ",
        ".",
        ": ",
        "\"",
        "\" ",
        "'",
        "' ",
        "?",
        "!",
        "\"",
        "\" ",
        ".",
        ", ",
        " ",
        "...",
        ""
      };
      for (int index = 0; index < strArray1.Length; ++index)
        str1 = str1.Replace(strArray1[index], strArray2[index]);
      string str2 = TranslatorEngine.ToNarrow(str1.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n"));
      int length = str2.Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < length - 1; ++index)
      {
        char character1 = str2[index];
        char character2 = str2[index + 1];
        if (TranslatorEngine.isChinese(character1))
        {
          if (!TranslatorEngine.isChinese(character2) && (int) character2 != 44 && ((int) character2 != 46 && (int) character2 != 58) && ((int) character2 != 59 && (int) character2 != 34 && ((int) character2 != 39 && (int) character2 != 63)) && ((int) character2 != 32 && (int) character2 != 33))
            stringBuilder.Append(character1).Append(" ");
          else
            stringBuilder.Append(character1);
        }
        else if ((int) character1 == 9 || (int) character1 == 32 || ((int) character1 == 34 || (int) character1 == 39) || (int) character1 == 10)
          stringBuilder.Append(character1);
        else if (TranslatorEngine.isChinese(character2))
          stringBuilder.Append(character1).Append(" ");
        else
          stringBuilder.Append(character1);
      }
      stringBuilder.Append(str2[length - 1]);
      return TranslatorEngine.indentAllLines(((object) stringBuilder).ToString());
    }

    public static string StandardizeInputForProxy(string original)
    {
      return TranslatorEngine.removeIgnoredChinesePhrasesForBrowser(TranslatorEngine.standardizeInputForProxyWithoutRemovingIgnoredChinesePhrases(original));
    }

    private static string standardizeInputForProxyWithoutRemovingIgnoredChinesePhrases(string original)
    {
      if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
        return "";
      string str1 = TranslatorEngine.ToSimplified(original);
      string[] strArray1 = new string[16]
      {
        "，",
        "。",
        "：",
        "“",
        "”",
        "‘",
        "’",
        "？",
        "！",
        "「",
        "」",
        "．",
        "、",
        "　",
        "…",
        TranslatorEngine.NULL_STRING
      };
      string[] strArray2 = new string[16]
      {
        ", ",
        ".",
        ": ",
        "\"",
        "\" ",
        "'",
        "' ",
        "?",
        "!",
        "\"",
        "\" ",
        ".",
        ", ",
        " ",
        "...",
        ""
      };
      for (int index = 0; index < strArray1.Length; ++index)
        str1 = str1.Replace(strArray1[index], strArray2[index]);
      string str2 = TranslatorEngine.ToNarrow(str1.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n"));
      int length = str2.Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < length - 1; ++index)
      {
        char character1 = str2[index];
        char character2 = str2[index + 1];
        if (TranslatorEngine.isChinese(character1))
        {
          if (!TranslatorEngine.isChinese(character2) && (int) character2 != 44 && ((int) character2 != 46 && (int) character2 != 58) && ((int) character2 != 59 && (int) character2 != 34 && ((int) character2 != 39 && (int) character2 != 63)) && ((int) character2 != 32 && (int) character2 != 33))
            stringBuilder.Append(character1).Append(" ");
          else
            stringBuilder.Append(character1);
        }
        else if ((int) character1 == 9 || (int) character1 == 32 || ((int) character1 == 34 || (int) character1 == 39) || (int) character1 == 10)
          stringBuilder.Append(character1);
        else if (TranslatorEngine.isChinese(character2))
          stringBuilder.Append(character1).Append(" ");
        else
          stringBuilder.Append(character1);
      }
      stringBuilder.Append(str2[length - 1]);
      return str2;
    }

    private static string indentAllLines(string text, bool insertBlankLine)
    {
      string[] strArray = text.Split(new char[1]
      {
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in strArray)
      {
        if (!string.IsNullOrEmpty(str.Trim()))
          stringBuilder.Append("\t" + str.Trim()).Append("\n").Append(insertBlankLine ? "\n" : "");
      }
      return ((object) stringBuilder).ToString();
    }

    private static string indentAllLines(string text)
    {
      return TranslatorEngine.indentAllLines(text, false);
    }

    private static bool isChinese(char character)
    {
      return TranslatorEngine.hanVietDictionary.ContainsKey(character.ToString());
    }

    public static bool IsChinese(char character)
    {
      return TranslatorEngine.isChinese(character);
    }

    public static bool IsAllChinese(string text)
    {
      foreach (char character in text)
      {
        if (!TranslatorEngine.isChinese(character))
          return false;
      }
      return true;
    }

    private static bool hasOnlyOneMeaning(string meaning)
    {
      return meaning.Split(new char[2]
      {
        '/',
        '|'
      }).Length == 1;
    }

    internal static string ToSimplified(string str)
    {
      return Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
    }

    internal static string ToWide(string str)
    {
      int length = str.Length;
      int index1;
      for (index1 = 0; index1 < length; ++index1)
      {
        char ch = str[index1];
        if ((int) ch >= 33 && (int) ch <= 126)
          break;
      }
      if (index1 >= length)
        return str;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index2 = 0; index2 < length; ++index2)
      {
        char ch = str[index2];
        if ((int) ch >= 33 && (int) ch <= 126)
          stringBuilder.Append((char) ((int) ch - 33 + 65281));
        else
          stringBuilder.Append(ch);
      }
      return ((object) stringBuilder).ToString();
    }

    internal static string ToNarrow(string str)
    {
      int length = str.Length;
      int index1;
      for (index1 = 0; index1 < length; ++index1)
      {
        char ch = str[index1];
        if ((int) ch >= 65281 && (int) ch <= 65374)
          break;
      }
      if (index1 >= length)
        return str;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index2 = 0; index2 < length; ++index2)
      {
        char ch = str[index2];
        if ((int) ch >= 65281 && (int) ch <= 65374)
          stringBuilder.Append((char) ((int) ch - 65281 + 33));
        else
          stringBuilder.Append(ch);
      }
      return ((object) stringBuilder).ToString();
    }

    private static void appendTranslatedWord(StringBuilder result, string translatedText, ref string lastTranslatedWord)
    {
      int startIndexOfNextTranslatedText = 0;
      TranslatorEngine.appendTranslatedWord(result, translatedText, ref lastTranslatedWord, ref startIndexOfNextTranslatedText);
    }

    private static void appendTranslatedWord(StringBuilder result, string translatedText, ref string lastTranslatedWord, ref int startIndexOfNextTranslatedText)
    {
      lastTranslatedWord = lastTranslatedWord.EndsWith("\n") || lastTranslatedWord.EndsWith("\t") || (lastTranslatedWord.EndsWith(". ") || lastTranslatedWord.EndsWith("\"")) || (lastTranslatedWord.EndsWith("'") || lastTranslatedWord.EndsWith("? ") || (lastTranslatedWord.EndsWith("! ") || lastTranslatedWord.EndsWith(".\" "))) || (lastTranslatedWord.EndsWith("?\" ") || lastTranslatedWord.EndsWith("!\" ") || lastTranslatedWord.EndsWith(": ")) ? TranslatorEngine.toUpperCase(translatedText) : (lastTranslatedWord.EndsWith(" ") || lastTranslatedWord.EndsWith("(") ? translatedText : " " + translatedText);
      if ((string.IsNullOrEmpty(translatedText) || (int) translatedText[0] == 44 || ((int) translatedText[0] == 46 || (int) translatedText[0] == 63) || (int) translatedText[0] == 33) && (0 < result.Length && (int) result[result.Length - 1] == 32))
      {
        result = result.Remove(result.Length - 1, 1);
        --startIndexOfNextTranslatedText;
      }
      result.Append(lastTranslatedWord);
    }

    private static string toUpperCase(string text)
    {
      if (string.IsNullOrEmpty(text))
        return text;
      if (text.StartsWith("[") && 2 <= text.Length)
        return "[" + (object) char.ToUpper(text[1]) + (string) (text.Length <= 2 ? (object) "" : (object) text.Substring(2));
      else
        return (string) (object) char.ToUpper(text[0]) + (text.Length <= 1 ? (object) "" : (object) text.Substring(1));
    }

    private static bool nextCharIsChinese(string chinese, int currentPhraseEndIndex)
    {
      if (chinese.Length - 1 <= currentPhraseEndIndex)
        return false;
      else
        return TranslatorEngine.isChinese(chinese[currentPhraseEndIndex + 1]);
    }

    private static string[] classifyWordsIntoLatinAndChinese(string inputText)
    {
      List<string> list = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char character in inputText)
      {
        if (TranslatorEngine.isChinese(character))
        {
          if (flag)
          {
            stringBuilder.Append(character);
          }
          else
          {
            list.Add(((object) stringBuilder).ToString());
            stringBuilder.Length = 0;
            stringBuilder.Append(character);
          }
          flag = true;
        }
        else
        {
          if (!flag)
          {
            stringBuilder.Append(character);
          }
          else
          {
            list.Add(((object) stringBuilder).ToString());
            stringBuilder.Length = 0;
            stringBuilder.Append(character);
          }
          flag = false;
        }
      }
      list.Add(((object) stringBuilder).ToString());
      return list.ToArray();
    }

    private static string[] classifyWordsIntoLatinAndChineseForProxy(string inputText)
    {
      List<string> list = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      foreach (char character in inputText)
      {
        if (flag2)
        {
          stringBuilder.Append(character);
          flag1 = false;
          if ((int) character == 62)
          {
            list.Add(((object) stringBuilder).ToString());
            stringBuilder.Length = 0;
            flag2 = false;
          }
        }
        else if ((int) character == 60)
        {
          list.Add(((object) stringBuilder).ToString());
          stringBuilder.Length = 0;
          stringBuilder.Append(character);
          flag2 = true;
          flag1 = false;
        }
        else if (TranslatorEngine.isChinese(character))
        {
          if (flag1)
          {
            stringBuilder.Append(character);
          }
          else
          {
            list.Add(((object) stringBuilder).ToString());
            stringBuilder.Length = 0;
            stringBuilder.Append(character);
          }
          flag1 = true;
        }
        else
        {
          if (!flag1)
          {
            stringBuilder.Append(character);
          }
          else
          {
            list.Add(((object) stringBuilder).ToString());
            stringBuilder.Length = 0;
            stringBuilder.Append(character);
          }
          flag1 = false;
        }
      }
      list.Add(((object) stringBuilder).ToString());
      return list.ToArray();
    }

    public static bool IsInVietPhrase(string chinese)
    {
      return TranslatorEngine.vietPhraseDictionary.ContainsKey(chinese);
    }

    public static string ChineseToHanVietForAnalyzer(string chinese)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in chinese)
      {
        if (TranslatorEngine.hanVietDictionary.ContainsKey(ch.ToString()))
          stringBuilder.Append(TranslatorEngine.hanVietDictionary[ch.ToString()] + " ");
        else
          stringBuilder.Append((string) (object) ch + (object) " ");
      }
      return ((object) stringBuilder).ToString().Trim();
    }

    public static string ChineseToVietPhraseForAnalyzer(string chinese, int translationAlgorithm, bool prioritizedName)
    {
      return TranslatorEngine.ChineseToVietPhraseForBrowser(chinese, 11, translationAlgorithm, prioritizedName).Trim(TranslatorEngine.trimCharsForAnalyzer);
    }

    private static bool containsName(string chinese, int startIndex, int phraseLength)
    {
      if (phraseLength < 2 || TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(startIndex, phraseLength)))
        return false;
      int num1 = startIndex + phraseLength - 1;
      int num2 = 2;
      for (int startIndex1 = startIndex + 1; startIndex1 <= num1; ++startIndex1)
      {
        for (int length = 20; length >= num2; --length)
        {
          if (chinese.Length >= startIndex1 + length && TranslatorEngine.onlyNameDictionary.ContainsKey(chinese.Substring(startIndex1, length)))
            return true;
        }
      }
      return false;
    }

    private static bool isLongestPhraseInSentence(string chinese, int startIndex, int phraseLength, Dictionary<string, string> dictionary, int translationAlgorithm)
    {
      if (phraseLength < 2)
        return true;
      int num1 = translationAlgorithm == 0 ? phraseLength : (phraseLength < 3 ? 3 : phraseLength);
      int num2 = startIndex + phraseLength - 1;
      for (int startIndex1 = startIndex + 1; startIndex1 <= num2; ++startIndex1)
      {
        for (int length = 20; length > num1; --length)
        {
          if (chinese.Length >= startIndex1 + length && dictionary.ContainsKey(chinese.Substring(startIndex1, length)))
            return false;
        }
      }
      return true;
    }

    public static int GetVietPhraseDictionaryCount()
    {
      return TranslatorEngine.onlyVietPhraseDictionary.Count;
    }

    public static int GetNameDictionaryCount(bool isNameChinh)
    {
      if (!isNameChinh)
        return TranslatorEngine.onlyNamePhuDictionary.Count;
      else
        return TranslatorEngine.onlyNameChinhDictionary.Count;
    }

    public static int GetPhienAmDictionaryCount()
    {
      return TranslatorEngine.hanVietDictionary.Count;
    }

    public static bool ExistInPhienAmDictionary(string chinese)
    {
      if (chinese.Length != 1)
        return false;
      else
        return TranslatorEngine.hanVietDictionary.ContainsKey(chinese);
    }

    private static void updateHistoryLogInCache(string key, string action, ref DataSet dictionaryHistoryDataSet)
    {
      string index = "DictionaryHistory";
      DataRow dataRow = dictionaryHistoryDataSet.Tables[index].Rows.Find((object) key);
      if (dataRow == null)
      {
        dictionaryHistoryDataSet.Tables[index].Rows.Add((object) key, (object) action, (object) Environment.GetEnvironmentVariable("USERNAME"), (object) DateTime.Now);
      }
      else
      {
        dataRow[1] = (object) action;
        dataRow[2] = (object) Environment.GetEnvironmentVariable("USERNAME");
        dataRow[3] = (object) DateTime.Now;
      }
    }

    private static void writeVietPhraseHistoryLog(string key, string action)
    {
      TranslatorEngine.updateHistoryLogInCache(key, action, ref TranslatorEngine.onlyVietPhraseDictionaryHistoryDataSet);
      TranslatorEngine.WriteHistoryLog(key, action, DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath());
    }

    private static void writeNamesHistoryLog(string key, string action, bool isNameChinh)
    {
      DataSet dictionaryHistoryDataSet = isNameChinh ? TranslatorEngine.onlyNameDictionaryHistoryDataSet : TranslatorEngine.onlyNamePhuDictionaryHistoryDataSet;
      TranslatorEngine.updateHistoryLogInCache(key, action, ref dictionaryHistoryDataSet);
      TranslatorEngine.WriteHistoryLog(key, action, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath());
    }

    private static void writePhienAmHistoryLog(string key, string action)
    {
      TranslatorEngine.updateHistoryLogInCache(key, action, ref TranslatorEngine.hanVietDictionaryHistoryDataSet);
      TranslatorEngine.WriteHistoryLog(key, action, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath());
    }

    public static string GetVietPhraseHistoryLogRecord(string key)
    {
      return TranslatorEngine.getDictionaryHistoryLogRecordInCache(key, TranslatorEngine.onlyVietPhraseDictionaryHistoryDataSet);
    }

    public static string GetNameHistoryLogRecord(string key, bool isNameChinh)
    {
      return TranslatorEngine.getDictionaryHistoryLogRecordInCache(key, isNameChinh ? TranslatorEngine.onlyNameDictionaryHistoryDataSet : TranslatorEngine.onlyNamePhuDictionaryHistoryDataSet);
    }

    public static string GetPhienAmHistoryLogRecord(string key)
    {
      return TranslatorEngine.getDictionaryHistoryLogRecordInCache(key, TranslatorEngine.hanVietDictionaryHistoryDataSet);
    }

    private static string getDictionaryHistoryLogRecordInCache(string key, DataSet dictionaryHistoryDataSet)
    {
      string index = "DictionaryHistory";
      DataRow dataRow = dictionaryHistoryDataSet.Tables[index].Rows.Find((object) key);
      if (dataRow == null)
        return "";
      else
        return string.Format("Entry này đã được <{0}> bởi <{1}> vào <{2}>.", dataRow[1], dataRow[2], (object) ((DateTime) dataRow[3]).ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
    }

    public static void CompressPhienAmDictionaryHistory()
    {
      TranslatorEngine.CompressDictionaryHistory(TranslatorEngine.hanVietDictionaryHistoryDataSet, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath());
    }

    public static void CompressOnlyVietPhraseDictionaryHistory()
    {
      TranslatorEngine.CompressDictionaryHistory(TranslatorEngine.onlyVietPhraseDictionaryHistoryDataSet, DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath());
    }

    public static void CompressOnlyNameDictionaryHistory(bool isNameChinh)
    {
      TranslatorEngine.CompressDictionaryHistory(isNameChinh ? TranslatorEngine.onlyNameDictionaryHistoryDataSet : TranslatorEngine.onlyNamePhuDictionaryHistoryDataSet, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath());
    }

    private static void CompressDictionaryHistory(DataSet dictionaryHistoryDataSet, string dictionaryHistoryFilePath)
    {
      string index = "DictionaryHistory";
      string str = dictionaryHistoryFilePath + (object) "." + (string) (object) DateTime.Now.Ticks;
      if (File.Exists(dictionaryHistoryFilePath))
        File.Copy(dictionaryHistoryFilePath, str, true);
      using (TextWriter textWriter = (TextWriter) new StreamWriter(dictionaryHistoryFilePath, false, Encoding.UTF8))
      {
        try
        {
          textWriter.WriteLine("Entry\tAction\tUser Name\tUpdated Date");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dictionaryHistoryDataSet.Tables[index].Rows)
            textWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", dataRow[0], dataRow[1], dataRow[2], (object) ((DateTime) dataRow[3]).ToString("yyyy-MM-dd HH:mm:ss.fffzzz")));
        }
        catch (Exception ex)
        {
          try
          {
            textWriter.Close();
          }
          catch
          {
          }
          if (File.Exists(dictionaryHistoryFilePath))
          {
            try
            {
              File.Copy(str, dictionaryHistoryFilePath, true);
            }
            catch
            {
            }
          }
          throw ex;
        }
        finally
        {
          File.Delete(str);
        }
      }
    }

    public static void WriteHistoryLog(string key, string action, string logPath)
    {
      if (!File.Exists(logPath))
        File.AppendAllText(logPath, "Entry\tAction\tUser Name\tUpdated Date\r\n", Encoding.UTF8);
      File.AppendAllText(logPath, key + "\t" + action + "\t" + Environment.GetEnvironmentVariable("USERNAME") + "\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz") + "\r\n", Encoding.UTF8);
    }

    public static void CreateHistoryLog(string key, string action, ref StringBuilder historyLogs)
    {
      historyLogs.AppendLine(key + "\t" + action + "\t" + Environment.GetEnvironmentVariable("USERNAME") + "\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
    }

    public static void WriteHistoryLog(string historyLogs, string logPath)
    {
      if (!File.Exists(logPath))
        File.AppendAllText(logPath, "Entry\tAction\tUser Name\tUpdated Date\r\n", Encoding.UTF8);
      File.AppendAllText(logPath, historyLogs, Encoding.UTF8);
    }

    private static string removeIgnoredChinesePhrases(string standardizedChinese)
    {
      if (string.IsNullOrEmpty(standardizedChinese))
        return string.Empty;
      string str = standardizedChinese;
      foreach (string oldValue in TranslatorEngine.ignoredChinesePhraseList)
        str = str.Replace(oldValue, string.Empty);
      return str.Replace("\t\n\n", string.Empty);
    }

    private static string removeIgnoredChinesePhrasesForBrowser(string standardizedChinese)
    {
      if (string.IsNullOrEmpty(standardizedChinese))
        return string.Empty;
      string str = standardizedChinese;
      foreach (string oldValue in TranslatorEngine.ignoredChinesePhraseForBrowserList)
        str = str.Replace(oldValue, string.Empty);
      return str.Replace("\t\n\n", string.Empty);
    }

    private static int containsLuatNhan(string chinese, Dictionary<string, string> dictionary)
    {
      string luatNhan;
      int matchedLength;
      return TranslatorEngine.containsLuatNhan(chinese, dictionary, out luatNhan, out matchedLength);
    }

    private static int containsLuatNhan(string chinese, Dictionary<string, string> dictionary, out string luatNhan, out int matchedLength)
    {
      int length1 = chinese.Length;
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.luatNhanDictionary)
      {
        if (length1 >= keyValuePair.Key.Length - 2)
        {
          string pattern = keyValuePair.Key.Replace("{0}", "([^,\\. ?]{1,10})");
          Match match = Regex.Match(chinese, pattern);
          int num = 0;
          do
          {
            if (match.Success)
            {
              string key = match.Groups[1].Value;
              if (keyValuePair.Key.StartsWith("{0}"))
              {
                for (int startIndex = 0; startIndex < key.Length; ++startIndex)
                {
                  if (dictionary.ContainsKey(key.Substring(startIndex)))
                  {
                    luatNhan = pattern;
                    matchedLength = match.Length - startIndex;
                    return match.Index + startIndex;
                  }
                }
              }
              else if (keyValuePair.Key.EndsWith("{0}"))
              {
                for (int length2 = key.Length; 0 < length2; --length2)
                {
                  if (dictionary.ContainsKey(key.Substring(0, length2)))
                  {
                    luatNhan = pattern;
                    matchedLength = match.Length - (key.Length - length2);
                    return match.Index;
                  }
                }
              }
              else if (dictionary.ContainsKey(key))
              {
                luatNhan = pattern;
                matchedLength = match.Length;
                return match.Index;
              }
              match = match.NextMatch();
              ++num;
            }
            else
              break;
          }
          while (num <= 1);
        }
      }
      luatNhan = string.Empty;
      matchedLength = -1;
      return -1;
    }

    private static bool matchesLuatNhan(string chinese, Dictionary<string, string> dictionary)
    {
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.luatNhanDictionary)
      {
        string str = keyValuePair.Key.Replace("{0}", "(.+)");
        Match match = Regex.Match(chinese, "^" + str + "$");
        if (match.Success && dictionary.ContainsKey(match.Groups[1].Value))
          return true;
      }
      return false;
    }

    private static bool matchesLuatNhan(string chinese, Dictionary<string, string> dictionary, string luatNhan)
    {
      Match match = Regex.Match(chinese, "^" + luatNhan + "$");
      return match.Success && dictionary.ContainsKey(match.Groups[1].Value);
    }

    public static string ChineseToLuatNhan(string chinese, Dictionary<string, string> dictionary)
    {
      string luatNhan = string.Empty;
      return TranslatorEngine.ChineseToLuatNhan(chinese, dictionary, out luatNhan);
    }

    public static string ChineseToLuatNhan(string chinese, Dictionary<string, string> dictionary, out string luatNhan)
    {
      int length = chinese.Length;
      foreach (KeyValuePair<string, string> keyValuePair in TranslatorEngine.luatNhanDictionary)
      {
        string str = keyValuePair.Key.Replace("{0}", "(.+)");
        Match match = Regex.Match(chinese, "^" + str + "$");
        if (match.Success && dictionary.ContainsKey(match.Groups[1].Value))
        {
          string[] strArray = dictionary[match.Groups[1].Value].Split(new char[2]
          {
            '/',
            '|'
          });
          StringBuilder stringBuilder = new StringBuilder();
          foreach (string newValue in strArray)
          {
            stringBuilder.Append(keyValuePair.Value.Replace("{0}", newValue));
            stringBuilder.Append("/");
          }
          luatNhan = keyValuePair.Key;
          return ((object) stringBuilder).ToString().Trim('/');
        }
      }
      throw new NotImplementedException("Lỗi xử lý luật nhân cho cụm từ: " + chinese);
    }
  }
}
