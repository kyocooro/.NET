// Decompiled with JetBrains decompiler
// Type: TranslatorEngine.CharRange
// Assembly: TranslatorEngine, Version=1.4.4937.24288, Culture=neutral, PublicKeyToken=null
// MVID: 636F5D06-194A-462F-B92A-7489F7DCC048
// Assembly location: D:\QuickTranslator\TranslatorEngine.dll

namespace TranslatorEngine
{
  public class CharRange
  {
    private int startIndex;
    private int length;

    public int StartIndex
    {
      get
      {
        return this.startIndex;
      }
      set
      {
        this.startIndex = value;
      }
    }

    public int Length
    {
      get
      {
        return this.length;
      }
      set
      {
        this.length = value;
      }
    }

    public CharRange(int startIndex, int length)
    {
      this.startIndex = startIndex;
      this.length = length;
    }

    public bool IsInRange(int index)
    {
      if (this.startIndex <= index)
        return index <= this.startIndex + this.length - 1;
      else
        return false;
    }

    public int GetEndIndex()
    {
      return this.startIndex + this.length - 1;
    }
  }
}
