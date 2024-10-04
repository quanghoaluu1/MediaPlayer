using LibVLCSharp.Shared;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        bool isPaused = false;
        bool isStoped = true;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick; 
        }

        private void btn_playPause_Click(object sender, RoutedEventArgs e)
        {
            if(isPaused || isStoped)
            {
                mediaPlayer.Play();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                isPaused = false;
                timer.Start();
            }
            else
            {
                mediaPlayer.Pause();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                isPaused = true;
                timer.Stop();
            }
            
            

        }

        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            mediaPlayer.Stop();
            isStoped = true;
            timer.Stop();
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
        private void seekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(slider_timeSlider.Value);
            }
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video File|*.mp4"
            };
            if(openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Source = new Uri(openFileDialog.FileName);
                mediaPlayer.Play();
                timer.Start();
            }
        }


        private void btn_forward5s_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan timeSpan = mediaPlayer.NaturalDuration.TimeSpan;
                if(mediaPlayer.Position.TotalSeconds + 5 < timeSpan.TotalSeconds)
                {
                    mediaPlayer.Position = mediaPlayer.Position.Add(TimeSpan.FromSeconds(5));
                }
                else
                {
                    mediaPlayer.Position = timeSpan;
                }
            }

        }

        private void btn_rewind5s_Click(object sender, RoutedEventArgs e)
        {
            if(mediaPlayer.Position.TotalSeconds > 5)
            {
                mediaPlayer.Position = mediaPlayer.Position.Subtract(TimeSpan.FromSeconds(5));
            }
            else
            {
                mediaPlayer.Position = TimeSpan.Zero;
            }

        }
    }
}