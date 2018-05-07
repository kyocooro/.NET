using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslatorEngine;

namespace Vietphrase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TranslatorEngine.TranslatorEngine.LoadDictionaries();
            CharRange[] a;
            CharRange[] b;
            string output = TranslatorEngine.TranslatorEngine.ChineseToVietPhraseOneMeaning("越皇血浆喷涌",
                0, 1, true, out a, out b);
            MessageBox.Show(output);
        }
    }
}
