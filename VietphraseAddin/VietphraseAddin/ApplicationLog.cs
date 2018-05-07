// Decompiled with JetBrains decompiler
// Type: TranslatorEngine.ApplicationLog
// Assembly: TranslatorEngine, Version=1.4.4937.24288, Culture=neutral, PublicKeyToken=null
// MVID: 636F5D06-194A-462F-B92A-7489F7DCC048
// Assembly location: D:\QuickTranslator\TranslatorEngine.dll

using System;
using System.IO;
using System.Text;

namespace TranslatorEngine
{
  public class ApplicationLog
  {
    public static void Log(string applicationPath, string application, Exception exception)
    {
      try
      {
        string str = Path.Combine(applicationPath, application + ".log");
        FileInfo fileInfo = new FileInfo(str);
        if (fileInfo.Exists && 1000000L < fileInfo.Length)
          fileInfo.Delete();
        string contents = string.Format("{0:G}: {1}\r\n", (object) DateTime.Now, (object) (exception.Message + (object) "\r\n" + (string) (object) exception.GetType() + "\r\n" + exception.StackTrace));
        File.AppendAllText(str, contents, Encoding.UTF8);
      }
      catch
      {
      }
    }
  }
}
