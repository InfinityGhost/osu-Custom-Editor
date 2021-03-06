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

namespace osu__Custom_Editor_v2
{
    /// <summary>
    /// Interaction logic for CommandBox.xaml
    /// </summary>
    public partial class CommandBox : UserControl
    {
        public event EventHandler<string> Output;
        public event EventHandler<string> CommandOutput;
        public event EventHandler<string> Paste;

        public CommandBox() => InitializeComponent();

        void Box_KeyDown(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                CommandOutput?.Invoke(this, textbox.Text);
                textbox.Text = string.Empty;
            }
        }

        #region Properties 

        public new Brush Foreground
        {
            set => Box.Foreground = value;
            get => Box.Foreground;
        }
        public new Brush Background
        {
            set => Box.Background = value;
            get => Box.Background;
        }

        #endregion

        #region Tools

        static bool IsKeyDown(Key key)
        {
            switch (Keyboard.GetKeyStates(key))
            {
                case KeyStates.Toggled:
                case KeyStates.Down:
                    return true;
                default:
                    return false;
            }
        }

        static bool AreKeysDown(IEnumerable<Key> keys)
        {
            foreach (var key in keys)
                if (!IsKeyDown(key))
                    return false;
            return true;
        }

        #endregion
    }
}
