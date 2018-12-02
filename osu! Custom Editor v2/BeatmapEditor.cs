﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using OsuBeatmapParser;
using OsuBeatmapParser.Beatmaps;

namespace osu__Custom_Editor_v2
{
    class BeatmapEditor
    {
        public BeatmapEditor()
        {

        }

        #region Variables & Events

        public event EventHandler<string> Output;

        public string FolderPath => Path.Replace(Path.Split('\\').LastOrDefault(), string.Empty);
        public string Path { get; set; }
        public Beatmap Beatmap { get; set; }

        #endregion

        #region File Management

        public Task Load(string filePath)
        {
            Beatmap = Parser.ParseBeatmap(filePath);
            Output?.Invoke(this, "Beatmap file \"" + filePath + "\" loaded.");
            Path = filePath;
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
