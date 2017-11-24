using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using System.IO;
using WMPLib;

namespace GlassTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowsMediaPlayer wmp;
        private List<string> mp3list;
        private int currentTrack = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = Properties.Settings.Default.posY;
            this.Left = Properties.Settings.Default.posX;

            if (Properties.Settings.Default.path == String.Empty)
            {
                MessageBox.Show("Please set path.");
            }
            else
            {
                if (Directory.Exists(Properties.Settings.Default.path))
                {
                    mp3list = getMp3List(Properties.Settings.Default.path);
                }
                else
                {
                    MessageBox.Show("Path, error, please set new path.");
                }
            }

            wmp = new WindowsMediaPlayer();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            Properties.Settings.Default.posX = Convert.ToInt16(this.Left);
            Properties.Settings.Default.posY = Convert.ToInt16(this.Top);
            Properties.Settings.Default.Save();
        }

        private static List<string> getMp3List(string path)
        {
            var mp3List = new List<string>();
            foreach (var file in Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories))
            {
                mp3List.Add(file.ToString());
            }
            return mp3List;
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            if (wmp.playState == WMPPlayState.wmppsPlaying)
            {
                wmp.controls.pause();
            }
            else if (wmp.playState == WMPPlayState.wmppsPaused)
            {
                wmp.controls.play();
            }
            else
            {
                playTrack(currentTrack);
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            nextTrack();
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            prevTrack();
        }

        private void list_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            Properties.Settings.Default.path = dialog.SelectedPath;
            Properties.Settings.Default.Save();
            mp3list = getMp3List(Properties.Settings.Default.path);
        }

        private void appClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void playTrack(int index)
        {
            var selectedTrack = mp3list[index];
            wmp.currentPlaylist.clear();
            wmp.URL = selectedTrack;
            wmp.controls.play();
        }

        private void nextTrack()
        {
            if (currentTrack + 1 >= mp3list.Count)
            {
                currentTrack = 0;
            }
            else
            {
                currentTrack = currentTrack + 1;
            }

            playTrack(currentTrack);
        }

        private void prevTrack()
        {
            if (currentTrack - 1 <= 0)
            {
                currentTrack = mp3list.Count - 1;
            }
            else
            {
                currentTrack = currentTrack - 1;
            }

            playTrack(currentTrack);
        }
    }
}
