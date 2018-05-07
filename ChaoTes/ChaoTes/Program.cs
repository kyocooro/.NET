using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoTes
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal a = 0.3243243245452459772452457646134145762525345m;
            decimal b = 0.3243243245452459772452457646134145762525344m;
            string result = a.ToString() + "\t" + b.ToString();
            for (int i = 1 ; i < 10000; i++)
            {
                a = a * 2; b = b * 2;
                if (a > 1) a -= 1;
                if (b > 1) b -= 1;
                result += "\r\n" + a.ToString() + "\t" + b.ToString();
            }
            System.IO.File.WriteAllText(@"output.csv", result);
        }
    }
}
