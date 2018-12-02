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
using Point = System.Drawing.Point;
using OsuBeatmapParser.Beatmaps.Objects;

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

        #region Methods

        public Task LoadBackground()
        {
            if (Editor.Beatmap != null)
            {
                string bg = Editor.Beatmap.EventsSection.BackgroundImage;
                string bgpath = $@"{Editor.FolderPath}\{bg}";
                if (bg != string.Empty)
                    BackgroundImage = bgpath;
                else
                    Output?.Invoke(this, "No background image to load.");
            }
            else
                Output?.Invoke(this, "No beatmap loaded.");
            return Task.CompletedTask;
        }

        #endregion

        #region Playback Control

        // TBA

        #endregion

        #region Commmands

        void HandleCommand(object sender, string command)
        {
            
        }

        #endregion

        #region Elements

        public Task LoadElement(int time)
        {
            Field.Children.Clear();
            var obj = Editor.Beatmap.HitObjects.Find(e => e.StartTime == time) ?? null;
            if (obj != null)
            {
                var shape = new Visual.Circle(obj);
                var element = shape.Object;
                Field.Children.Add(element);
                MoveElement(element, obj.Position);
            }
            return Task.CompletedTask;
        }

        public Task MoveElement(UIElement element, Point position)
        {
            Canvas.SetLeft(element, position.X);
            Canvas.SetTop(element, position.Y);
            return Task.CompletedTask;
        }

        public class Visual
        {
            public class Circle
            {
                public Circle(HitObject obj)
                {
                    Position = obj.Position;
                }

                public Circle(HitObject obj, int cs)
                {
                    Position = obj.Position;

                }

                public Point Position { set; get; }

                public double Size { set; get; }

                public UIElement Object => new Ellipse
                {
                    Width = Size,
                    Height = Size,
                };
            }
        }

        private void Field_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // TODO: add field scaling
            Canvas canvas = sender as Canvas;
        }

        #endregion


    }
}
