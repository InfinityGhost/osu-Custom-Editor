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

namespace osu__Custom_Editor_v2
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        public Console()
        {
            InitializeComponent();
        }

        public event EventHandler<string> Update;

        #region Console Text

        public string Prefix => DateTime.Now.ToLongTimeString() + ">";

        public async Task Log(string text)
        {
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                if (ConsoleBlock.Text != string.Empty)
                    ConsoleBlock.Text += Environment.NewLine + Prefix + text;
                else
                    ConsoleBlock.Text += Prefix + text;
                Update?.Invoke(this, text);
            }));
        }

        public async Task Clear()
        {
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                ConsoleBlock.Text = string.Empty;
            }));
        }

        public async Task CopyAll()
        {
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                Clipboard.SetText(ConsoleBlock.Text);
            }));
        }

        #endregion

        #region Buttons

        public async void CopyButton(object sender, RoutedEventArgs e) => await CopyAll();

        public async void ClearButton(object sender, RoutedEventArgs e) => await Clear();

        #endregion
    }
}
