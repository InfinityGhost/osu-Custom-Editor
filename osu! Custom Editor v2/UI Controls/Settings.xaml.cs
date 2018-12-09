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

using OsuParsers;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Sections;

namespace osu__Custom_Editor_v2
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        public event EventHandler<string> Output;
        public event EventHandler<Beatmap> BeatmapUpdate;

        public Beatmap Beatmap
        {
            get => _beatmap;
            set
            {
                _beatmap = value;
                Metadata.DataContext = _beatmap.MetadataSection;
                Difficulty.DataContext = _beatmap.DifficultySection;
                Advanced.DataContext = _beatmap.GeneralSection;
                Design.DataContext = _beatmap.GeneralSection;
            }
        }
        private Beatmap _beatmap;


    }
}
