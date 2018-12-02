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
    /// Interaction logic for EditorUI.xaml
    /// </summary>
    public partial class EditorUI : UserControl
    {
        public EditorUI()
        {
            InitializeComponent();
            AddEditorEvents();
        }

        public event EventHandler<string> Output;

        #region Editor

        BeatmapEditor Editor = new BeatmapEditor();

        public async Task Load(string filename)
        {
            await Editor.Load(filename);
            await LoadBackground();
        }

        public async Task Save(string filename)
        {
            await Editor.Save(filename);
        }

        public Task AddEditorEvents()
        {
            Editor.Output += Editor_Output;
            Output?.Invoke(this, "Editor events hooked.");
            return Task.CompletedTask;
        }

        private void Editor_Output(object sender, string e) => Output?.Invoke(sender, e);

        #endregion

        #region Properties

        public string BackgroundImage
        {
            set
            {
                mapbg.Source = new ImageSourceConverter().ConvertFromString(value) as ImageSource;
                Output?.Invoke(this, "Background image updated to " + value);
            }
            get
            {
                return ((BitmapFrame)mapbg.Source).Decoder.ToString();
            }
        }

        #endregion

        #region Playback Controls


        void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(sender, "Clicked");
        }

        void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(sender, "Clicked");
        }

        void StopButtonClick(object sender, RoutedEventArgs e)
        {
            Output?.Invoke(sender, "Clicked");
        }

        #endregion

        #region Methods

        public Task LoadBackground()
        {
            string bg = Editor.Beatmap.EventsSection.BackgroundImage;
            string bgpath = $@"{Editor.FolderPath}\{bg}";
            if (bg != string.Empty)
                BackgroundImage = bgpath;
            else
                Output?.Invoke(this, "No beatmap loaded.");
            return Task.CompletedTask;
        }

        public Task LoadElement(int time)
        {
            Screen.Children.Clear();
            var obj = Editor.Beatmap.HitObjects.Find(e => e.StartTime == time) ?? null;
            if (obj != null)
            {
                var shape = new Ellipse
                {
                    Width = 100,
                    Height = 100,
                };
                Screen.Children.Add(shape);
            }
            return Task.CompletedTask;
        }

        public Task MoveElement(object element, Point position)
        {
            Canvas.SetLeft(Screen, position.X);
            Canvas.SetTop(Screen, position.Y);
            return Task.CompletedTask;
        }

        #endregion

        public class Visual
        {
            public class Circle
            {
                public Circle(HitObject obj)
                {
                    Position = obj.Position;

                }

                Point Position { set; get; }

            }
        }
    }
}
