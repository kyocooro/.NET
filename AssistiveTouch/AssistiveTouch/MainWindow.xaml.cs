using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Input;
using System.Drawing;

namespace AssistiveTouch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Top = System.Windows.SystemParameters.WorkArea.Bottom - this.Height;
            this.Left = System.Windows.SystemParameters.WorkArea.Right - this.Width;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);



            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }



        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;



        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd,

                                                  int nIndex,

                                                  int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd,

                                               int nIndex);

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (System.Exception)
            {
                
               
            }
            
        }

        private void Window_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            
            System.Windows.Forms.SendKeys.SendWait("^c");
            System.Windows.Forms.SendKeys.SendWait("{F8}");
        }
    }
}
