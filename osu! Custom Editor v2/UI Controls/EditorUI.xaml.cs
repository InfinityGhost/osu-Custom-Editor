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

        public string BackgroundPath
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


    }
}
