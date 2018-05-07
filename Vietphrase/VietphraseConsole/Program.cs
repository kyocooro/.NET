using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorEngine;
namespace VietphraseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TranslatorEngine.TranslatorEngine.LoadDictionaries();
            CharRange[] a;
            CharRange[] b;
            string output = TranslatorEngine.TranslatorEngine.ChineseToVietPhraseOneMeaning(args[0],
                0, 1, true, out a, out b);
            Console.Write(output);
        }
    }
}
