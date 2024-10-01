using LibVLCSharp.Shared;
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
        bool isStoped = false;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick; 
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = new Uri("C:\\Users\\quang\\Desktop\\MediaPlayer\\MediaPlayer\\spider-man.mp4");
            if(isStoped)
            {
                mediaPlayer.Stop();
                PlayStopIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                isStoped = false;
                timer.Stop();
            }
            else
            {
                mediaPlayer.Play();
                PlayStopIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                isStoped = true;
                timer.Start();
            }
            
            

        }

        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                mediaPlayer.Play();
                btn_pause.Content = "Pause";
                isPaused = false;
            }
            else
            {
                mediaPlayer.Pause();
                btn_pause.Content = "Resume";
                isPaused = true;
            }


        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
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
    }
}