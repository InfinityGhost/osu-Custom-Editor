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
    /// Interaction logic for PlaybackControl.xaml
    /// </summary>
    public partial class PlaybackControl : UserControl
    {
        public PlaybackControl()
        {
            InitializeComponent();
        }

        public event EventHandler<string> Output;

        public event RoutedEventHandler Pause;
        public event RoutedEventHandler Play;
        public event RoutedEventHandler Stop;

        public bool IsPlaying { get; private set; }

        void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(this, "Pause clicked.");
            Pause?.Invoke(sender, e);
            IsPlaying = false;
        }

        void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(this, "Play clicked.");
            Play?.Invoke(sender, e);
            IsPlaying = true;
        }

        void StopButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(this, "Stop clicked.");
            Stop?.Invoke(sender, e);
            IsPlaying = false;
        }
    }
}
