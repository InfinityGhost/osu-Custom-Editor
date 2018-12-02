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
using System.Diagnostics;

namespace osu__Custom_Editor_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Editor.Output += Output;
            Output(sender, "Loaded.");

            if (Debugger.IsAttached)
            {
                DebuggingMenu.SetValue(VisibilityProperty, Visibility.Visible);
            }

        }

        #region Variables & Events

        // TBA

        #endregion

        #region Console

        public async void Output(object sender, string text) => await Output(sender.GetType().Name + ": " + text);
        private async Task Output(string text)
        {
            await Console.Log(text);
            
        }

        public async void Status(object sender, string text) => await Status(text);
        public Task Status(string text)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                StatusBlock.Text = text;
            }));
            return Task.CompletedTask;
        }

        #endregion

        #region File Management

        // Loading

        public async void LoadFile(object sender, RoutedEventArgs e) 
        {
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "osu! Beatmap File (*.osu)|*.osu|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                try
                {
                    await LoadFile(dialog.FileName);
                }
                catch(Exception ex)
                {
                    Output(this, ex.ToString());
                }
        }
        public async Task LoadFile(string filename)
        {
            await Editor.Load(filename);
        }

        // Saving

        public async void SaveFile(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "osu! Beatmap File (*.osu)|*.osu|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                await SaveFile(dialog.FileName);
        }
        public async Task SaveFile(string filename)
        {
            
        }

        #endregion

        #region Tools

        public async Task UpdateVariable(object sender, object newValue)
        {
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                sender = newValue;
            }));
        }

        #endregion

        #region Misc.

        void ExitButton(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region Tests

        async void LoadTestFile(object sender, RoutedEventArgs e)
        {
            await Editor.Load(@"E:\osu!\Songs\785731 S3RL - Catchit (Radio Edit)\S3RL - Catchit (Radio Edit) (StarrStyx) [Insane].osu");
        }

        void GetBGImage(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }
}
