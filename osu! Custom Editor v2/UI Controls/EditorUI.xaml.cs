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
using OsuParsers.Beatmaps.Objects.Standard;
using System.Media;

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
            AudioOutput.Open(Editor.AudioPath);
            Controls.TimeBox.DataContext = AudioOutput.Position;
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
                mapbg.Source = Tools.Images.GetSource(value);
                Output?.Invoke(this, "Background image updated to " + value);
            }
            get
            {
                return Tools.Images.GetPath(mapbg);
            }
        }

        public OsuParsers.Beatmaps.Beatmap Beatmap
        {
            get => Editor.Beatmap;
            set => Editor.Beatmap = value;
        }

        public AudioPlayer AudioOutput { private set; get; } = new AudioPlayer();

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

        private async void Controls_Play(object sender, RoutedEventArgs e)
        {
            await AudioOutput.Play();
            await UpdateTime();
        }

        private async void Controls_Pause(object sender, RoutedEventArgs e)
        {
            await AudioOutput.Pause();
            await UpdateTime();
        }

        private async void Controls_Stop(object sender, RoutedEventArgs e)
        {
            await AudioOutput.Stop();
            await UpdateTime();
        }

        private async void PlayPause(object sender, ExecutedRoutedEventArgs e)
        {
            if (AudioOutput.IsPlaying)
                await AudioOutput.Pause();
            else
                await AudioOutput.Play();
            await UpdateTime();
        }

        public Task UpdateTime()
        {
            Controls.TimeBox.DataContext = AudioOutput.Position;
            return Task.CompletedTask;
        }

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
                            //await LoadElement(Decoder.Timestamp(remainder));
                            await LoadElements(Decoder.Timestamp(remainder));
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
                var shape = GetVisualObject(obj, cs);

                var element = shape.Element;
                Field.Children.Add(element);

                MoveElement(element, shape.CenterPos);
                Output?.Invoke(element, $"Object rendered.");
            }
            else
                throw new Exception("Object not found");
            return Task.CompletedTask;
        }

        public Task LoadElements(int time, int ar = 9)
        {
            Field.Children.Clear();
            var preempt = Tools.OsuCalc.Preempt(ar) * 2;
            var objs = Editor.Beatmap?.HitObjects.Where(e => e.StartTime <= time + preempt && e.StartTime >= time - preempt) ?? null;
            var cs = Editor.Beatmap?.DifficultySection.CircleSize ?? 0;
            
            if (objs != null)
            {
                foreach(var obj in objs)
                {
                    var visual = GetVisualObject(obj, cs);
                    PlaceElement(visual);
                    Output?.Invoke(visual, $"Object @ {obj.StartTime}ms rendered");
                }
            }
            return Task.CompletedTask;
        }

        public Task PlaceElement(Visual.Object visual)
        {
            var element = visual.Element;
            Field.Children.Add(element);
            MoveElement(element, visual.CenterPos);
            return Task.CompletedTask;
        }

        public Task MoveElement(UIElement element, Point position)
        {
            Canvas.SetLeft(element, position.X);
            Canvas.SetTop(element, position.Y);
            return Task.CompletedTask;
        }

        public Visual.Object GetVisualObject(OsuParsers.Beatmaps.Objects.HitObject hitObject, float cs = 4)
        {
            switch (hitObject.GetType().Name)
            {
                case "StandardHitCircle":
                    return new Visual.Circle(hitObject as StandardHitCircle, cs);
                case "StandardSlider":
                    return new Visual.Slider(hitObject as StandardSlider, cs);
                case "StandardSpinner":
                    return new Visual.Spinner(hitObject as StandardSpinner);
                default:
                    return new Visual.Object();
            }
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

                public double Diameter => (32 * (1 - 0.7 * (Size - 5) / 5)) * 2;

                public Point CenterPos => new Point
                {
                    X = Convert.ToInt32(Position.X - Size / 2),
                    Y = Convert.ToInt32(Position.Y - Size / 2),
                };

                public virtual UIElement Element => new Rectangle
                {
                    Width = Diameter,
                    Height = Diameter,
                    Fill = Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = Size / 8,
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
                    Width = Diameter,
                    Height = Diameter,
                    Fill = Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = Size / 8,
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

                public new Point Position => new Point(256, 192);
                public new double Size => throw new Exception("Value not available for this object type.");

                public override UIElement Element => new Ellipse
                {
                    Width = 384,
                    Height = 384,
                    Fill = Tools.ColorHelper.HexToBrush("#A7000000"),
                    Stroke = Brushes.Black,
                    StrokeThickness = 16,
                };
            }
        }


        #endregion
    }
}
