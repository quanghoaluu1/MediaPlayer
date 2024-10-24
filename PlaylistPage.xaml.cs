﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {

        public ObservableCollection<string> PlaylistItems { get; set; }
        private string currentPlaylist = "";
        public PlaylistPage()
        {
            InitializeComponent();
            PlaylistManager.LoadPlaylist();
            PlaylistItems = new ObservableCollection<string>(PlaylistManager.Playlist.Keys);
            this.DataContext = this;
            playlist_lbox.ItemsSource = PlaylistItems;
        }


        private void return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void createPlaylist_btn_Click(object sender, RoutedEventArgs e)
        {
            string playlistName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the playlist", "New Playlist", "Playlist Name");

            if (!string.IsNullOrEmpty(playlistName) && !PlaylistItems.Contains(playlistName))
            {
                PlaylistItems.Add(playlistName);
                PlaylistManager.Playlist.Add(playlistName, new List<string>());
                PlaylistManager.SavePlaylist();
            }
            else
            {
                MessageBox.Show("Playlist name already exists or is empty");
            }

        }

        private void deletePlaylist_btn_Click(object sender, RoutedEventArgs e)
        {
            if (playlist_lbox.SelectedItem != null)
            {
                string selectedPlaylist = playlist_lbox.SelectedItem.ToString();
                PlaylistItems.Remove(selectedPlaylist);
                PlaylistManager.Playlist.Remove(selectedPlaylist);
                PlaylistManager.SavePlaylist();
            }
        }

        private void playlist_lbox_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string selectedPlaylist = playlist_lbox.SelectedItem.ToString();
            if(string.IsNullOrEmpty(selectedPlaylist))
            {
                return;
            }

            ContextMenu cm = new ContextMenu();

            MenuItem deleteItem = new MenuItem() { Header = "Delete Playlist" };
            deleteItem.Click += (s, ev) =>
            {
                PlaylistItems.Remove(selectedPlaylist);
                PlaylistManager.Playlist.Remove(selectedPlaylist);
                PlaylistManager.SavePlaylist();
            };

            MenuItem renameItem = new MenuItem() { Header = "Rename Playlist" };
            renameItem.Click += (s, ev) =>
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter the new name of the playlist", "Rename Playlist", selectedPlaylist);
                if (!string.IsNullOrEmpty(newName) && !PlaylistItems.Contains(newName))
                {
                    PlaylistItems[PlaylistItems.IndexOf(selectedPlaylist)] = newName;
                    PlaylistManager.Playlist.Add(newName, PlaylistManager.Playlist[selectedPlaylist]);
                    PlaylistManager.Playlist.Remove(selectedPlaylist);
                    PlaylistManager.SavePlaylist();
                }
                else
                {
                    MessageBox.Show("Playlist name already exists or is empty");
                }
            };

            MenuItem addFileItem = new MenuItem() { Header = "Add File" };
            addFileItem.Click += (s, ev) =>
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "Media Files|*.mp3;*.mp4;*.wav;*.wma;*.avi;*.wmv;*.mov;*.flv;*.mkv;*.webm";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        PlaylistManager.AddFileToPlaylist(selectedPlaylist, file);
                    }
                    PlaylistManager.SavePlaylist();
                }
            };

            cm.Items.Add(deleteItem);
            cm.Items.Add(renameItem);
            cm.Items.Add(addFileItem);

            cm.IsOpen = true;

            
        }

        private void playlist_lbox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string selectedPlaylist = playlist_lbox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedPlaylist) && PlaylistManager.Playlist.ContainsKey(selectedPlaylist))
            {
                currentPlaylist = selectedPlaylist;
                fileList_lbox.ItemsSource = PlaylistManager.Playlist[selectedPlaylist];
            }
        }

        private void fileList_lbox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(fileList_lbox.SelectedItems == null) { return; }
            else
            {
                string selectedFile = fileList_lbox.SelectedItem as string;
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    MessageBox.Show(selectedFile);
                    MainPage mainPage = new MainPage(selectedFile);
                    this.NavigationService.Navigate(mainPage);
                }
            }         
        }
    }
}
