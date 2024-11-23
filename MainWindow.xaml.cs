using System.Windows;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // public MainWindow()
        // {
        //   
        // }

        public MainWindow(string filePath)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(filePath))
            {
                mainFrame.Navigate(new MainPage(filePath));
            }
            else
            {
                mainFrame.Navigate(new MainPage());
            }
           
        }
    }
}