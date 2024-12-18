﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;


namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        DispatcherTimer timer;

        PlayerState playerState = PlayerState.PLAYING;
        public MainPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        public MainPage(string filePath)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
            this.PlayMedia(filePath, fileExtension);
        }

        private void PlayMedia(string filePath, string fileExtension)
        {
            mediaPlayer.Source = new Uri(filePath);
            fileNameTextBlock.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);

            if (fileExtension == ".mp3")
            {
                mediaPlayer.Visibility = Visibility.Collapsed;
                image_audioImage.Visibility = Visibility.Visible;

                mediaPlayer.Play();
            }
            else if (fileExtension == ".mp4")
            {
                mediaPlayer.Visibility = Visibility.Visible;
                image_audioImage.Visibility = Visibility.Collapsed;

                mediaPlayer.Play();
            }
            timer.Start();
        }

        private void Btn_playPause_Click(object sender, RoutedEventArgs e)
        {
            if (playerState == PlayerState.PLAYING)
            {
                ChangePlayerState(PlayerState.PAUSED);
            }
            else
            {
                ChangePlayerState(PlayerState.PLAYING);
            }

        }


        private void ChangePlayerState(PlayerState state)
        {
            playerState = state;
            switch (state)
            {
                case PlayerState.PLAYING:
                    mediaPlayer.Play();
                    PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    timer.Start();
                    break;
                case PlayerState.PAUSED:
                    PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    mediaPlayer.Pause();
                    timer.Stop();
                    break;
                case PlayerState.STOPPED:
                    PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    mediaPlayer.Stop();
                    timer.Stop();
                    break;

            }
        }


        private void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            ChangePlayerState(PlayerState.STOPPED);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                slider_timeSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                slider_timeSlider.Value = mediaPlayer.Position.TotalSeconds;

                currentTime.Text = mediaPlayer.Position.ToString(@"hh\:mm\:ss");
                totalTime.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            }
        }
        private void SeekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(slider_timeSlider.Value);
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files (*.mp4)|*.mp4|MKV File (*.mkv)|*.mkv|Audio Files (*.mp3)|*.mp3|All Files (*.*)|*.*";


            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                this.PlayMedia(filePath, fileExtension);
            }
        }


        private void Btn_forward5s_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan timeSpan = mediaPlayer.NaturalDuration.TimeSpan;
                if (mediaPlayer.Position.TotalSeconds + 5 < timeSpan.TotalSeconds)
                {
                    mediaPlayer.Position = mediaPlayer.Position.Add(TimeSpan.FromSeconds(5));
                }
                else
                {
                    mediaPlayer.Position = timeSpan;
                }
            }

        }

        private void Btn_rewind5s_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Position.TotalSeconds > 5)
            {
                mediaPlayer.Position = mediaPlayer.Position.Subtract(TimeSpan.FromSeconds(5));
            }
            else
            {
                mediaPlayer.Position = TimeSpan.Zero;
            }

        }

        private void Slider_volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = slider_volumeSlider.Value;
        }

        private void playlist_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlaylistPage());
        }
    }
}
