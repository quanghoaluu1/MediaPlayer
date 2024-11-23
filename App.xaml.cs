using System.Configuration;
using System.Data;
using System.Windows;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
           string filePath = e.Args.Length > 0 ? e.Args[0] : null;
           MainWindow mainWindow = new MainWindow(filePath);
           mainWindow.Show();
        }
    }

}
