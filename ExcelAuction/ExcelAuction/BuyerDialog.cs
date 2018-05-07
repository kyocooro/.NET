using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelAuction
{
    public partial class BuyerDialog : Form
    {
        private Dictionary<string, string> addressInfo = new Dictionary<string, string>();
        public BuyerDialog()
        {
            InitializeComponent();
        }

        private void BuyerDialog_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(ExcelAuction.Global.storeLocation + "Buyer.txt");

                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    addressInfo.Add(line.Split(';')[0], string.Join("\r\n", line.Split(';')));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            for (int i = 1; i <= addressInfo.Keys.Count; i++)
            {
                List<RadioButton> buttons = grbBuyer.Controls.OfType<RadioButton>().ToList();
                foreach (RadioButton button in buttons)
                {
                    if (button.TabIndex == i)
                    {
                        button.Text = addressInfo.Keys.ElementAt(i - 1);
                        if (i == Convert.ToInt32(Properties.Settings.Default["AddressIndex"]))
                        {
                            button.Select();
                        }
                    }
                        
                }
            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedButton = grbBuyer.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);
            Properties.Settings.Default["AddressIndex"] = ((RadioButton)checkedButton).TabIndex;
            Properties.Settings.Default["Address"] = addressInfo[((RadioButton)checkedButton).Text];
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void grbBuyer_Enter(object sender, EventArgs e)
        {

        }
    }
}
