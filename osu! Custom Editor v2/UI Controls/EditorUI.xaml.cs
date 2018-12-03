﻿using System;
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
using Decoder = osu__Custom_Editor_v2.Tools.Decoder;

using OsuBeatmapParser.Beatmaps.Objects.Standard;

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
            Overlay.SetValue(VisibilityProperty, Visibility.Visible);
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

        async void HandleCommand(object sender, string command)
        {
            var cmd = command.Split(' ').First() ?? command;
            string remainder = command.Replace($"{cmd} ", string.Empty) ?? string.Empty;
            switch (cmd)
            {
                case "goto":
                    {
                        try
                        {
                            await LoadElement(Decoder.Timestamp(remainder));
                        }
                        catch (NullReferenceException)
                        {
                            Output?.Invoke(Editor, "No beatmap loaded.");
                        }
                        catch (Exception ex)
                        {
                            Output?.Invoke(Editor, ex.Message);
                        }
                        break;
                    }
                case "render":
                    {
                        Field.Children.Clear();
                        var x = remainder.Split(' ');
                        var position = new Point(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]));
                        Visual.Object obj = new Visual.Object
                        {
                            Position = position,
                        };

                        var elem = obj.Element;
                        Field.Children.Add(elem);
                        await MoveElement(elem, position);
                        Output?.Invoke(this, $"Rendered new object @ {position}");
                        break;
                    }
                default:
                    {
                        Output?.Invoke(this, "Invalid command.");
                        break;
                    }
            }
        }

        #endregion

        #region Elements

        public Task LoadElement(int time)
        {
            Field.Children.Clear();
            var obj = Editor.Beatmap.HitObjects.Find(e => e.StartTime == time) ?? null;
            var cs = Editor.Beatmap?.DifficultySection.CircleSize ?? 0;

            if (obj != null)
            {
                Visual.Object shape;
                switch(obj.GetType().Name)
                {
                    case "StandardHitCircle":
                        {
                            shape = new Visual.Circle(obj as StandardHitCircle, cs);
                            break;
                        }
                    default:
                        {
                            shape = new Visual.Object();
                            break;
                        }
                }

                var element = shape.Element;
                Field.Children.Add(element);

                var position = new Point
                {
                    X = Convert.ToInt32(shape.Position.X - shape.ScaledSize /2.0),
                    Y = Convert.ToInt32(shape.Position.Y - shape.ScaledSize /2.0)
                };

                MoveElement(element, position);
                Output?.Invoke(element, $"Object rendered.");
            }
            else
                throw new Exception("Object not found");
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
            public class Object
            {
                public Object()
                {
                    Color = Brushes.White;
                    Size = 4;
                }

                public Point Position { set; get; }
                public double Size { set; get; }
                public Brush Color { set; get; }

                public double ScaledSize => 32 * (1 - 0.7 * (Size - 5) / 5);

                public virtual UIElement Element => new Rectangle
                {
                    Width = ScaledSize,
                    Height = ScaledSize,
                    Fill = Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = Size / 16,
                };
            }   

            public class Circle : Object
            {
                public Circle(StandardHitCircle obj)
                {
                    Position = obj.Position;
                }

                public Circle(StandardHitCircle obj, float cs)
                {
                    Position = obj.Position;
                    Size = cs;
                }

                public override UIElement Element => new Ellipse
                {
                    Width = ScaledSize,
                    Height = ScaledSize,
                    Fill = Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = Size / 16,
                };
            }
            
            public class Slider : Object
            {
                public Slider(StandardSlider slider)
                {

                }

                public Slider(StandardSlider slider, float cs)
                {
                    Size = cs;
                }
            }

            public class Spinner : Object
            {
                public Spinner(StandardSpinner spinner)
                {
                    
                }

                public override UIElement Element => new Ellipse
                {
                    Width = 620,
                    Height = 620,
                    Fill = Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = Size / 16,
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
