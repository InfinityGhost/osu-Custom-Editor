using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using OsuParsers;
using OsuParsers.Beatmaps;

namespace osu__Custom_Editor_v2
{
    class BeatmapEditor
    {
        public BeatmapEditor()
        {

        }

        #region Variables & Events

        public event EventHandler<string> Output;

        public string FolderPath => System.IO.Path.GetDirectoryName(Path);
        public string Path { get; private set; }
        public Beatmap Beatmap { get; set; }

        #endregion

        #region File Management

        public Task Load(string filePath)
        {
            try
            {
                Beatmap = Parser.ParseBeatmap(filePath);
                Output?.Invoke(this, $"Beatmap file \"{filePath}\" loaded.");
                Path = filePath;
            }
            catch
            {
                Output?.Invoke(this, $"Failed to load beatmap at \"{filePath}\".");
            }
            return Task.CompletedTask;
        }

        public Task Save(string filePath)
        {
            // TODO: add save function
            throw new NotImplementedException();
        }

        #endregion

    }
}
