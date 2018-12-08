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

        public Beatmap Beatmap { private get; set; }

        private void UpdateProperties(object sender, EventArgs e = null)
        {
            Output?.Invoke(sender, "Updated properties.");
            if (Beatmap != null)
            {
                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        {
                            var metadata = Beatmap.MetadataSection;
                            metadata.TitleUnicode = SongTitleUnicode.Text;
                            metadata.Title = SongTitle.Text;
                            metadata.ArtistUnicode = SongArtistUnicode.Text;
                            metadata.Artist = SongArtist.Text;
                            metadata.Version = Difficulty.Text;
                            metadata.Source = Source.Text;
                            metadata.TagsString = Tags.Text;
                            break;
                        }
                    case "Slider":
                        {
                            var difficulty = Beatmap.DifficultySection;
                            difficulty.HPDrainRate = ConvertFloat(HPDrainRate.Value);
                            difficulty.CircleSize = ConvertFloat(CircleSize.Value);
                            difficulty.ApproachRate = ConvertFloat(ApproachRate.Value);
                            difficulty.OverallDifficulty = ConvertFloat(OverallDifficulty);

                            Beatmap.GeneralSection.StackLeniency = ConvertFloat(StackLeniency.Value);
                            break;
                        }
                    case "CheckBox":
                        {
                            var general = Beatmap.GeneralSection;
                            general.Countdown = Countdown.IsChecked ?? false;
                            general.WidescreenStoryboard = Widescreen.IsChecked ?? false;
                            general.StoryFireInFront = StoryFront.IsChecked ?? false;
                            general.EpilepsyWarning = Epilepsy.IsChecked ?? false;
                            general.LetterboxInBreaks = LetterboxBreaks.IsChecked ?? false;    
                            break;
                        }
                    case "ComboBox":
                        {
                            Beatmap.GeneralSection.ModeId = Mode.SelectedIndex;
                            break;
                        }
                    default:
                        {
                            Output?.Invoke(sender, "Unknown object update.");
                            break;
                        }
                }
                BeatmapUpdate?.Invoke(this, Beatmap);
            }
            else
                Output?.Invoke(sender, "Invalid beatmap file.");
        }

        public static float ConvertFloat(object value)
        {
            float y;
            float.TryParse(value.ToString(), out y);
            return y;
        }
    }
}
