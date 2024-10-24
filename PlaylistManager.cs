using Newtonsoft.Json;
using System.IO;
namespace MediaPlayer
{
    class PlaylistManager
    {
        private static string playlistFile = "playlists.json";

        public static Dictionary<string, List<string>> Playlist { get; private set; } = new Dictionary<string, List<string>>();

        public static void SavePlaylist()
        {
            string json = JsonConvert.SerializeObject(Playlist);
            File.WriteAllText(playlistFile, json);
        }

        public static void LoadPlaylist()
        {
            if (File.Exists(playlistFile))
            {
                string json = File.ReadAllText(playlistFile);
                Playlist = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            }
            else
            {
                Playlist = new Dictionary<string, List<string>>();
            }
        }
        
        public static bool AddFileToPlaylist(string playlistName, string file)
        {
            if (Playlist.ContainsKey(playlistName))
            {
                if (!Playlist[playlistName].Contains(file))
                {
                    Playlist[playlistName].Add(file);
                    SavePlaylist();
                    return true;
                }
            }
            return false;
        }

        public static bool RemoveFileFromPlaylist(string playlistName, string file)
        {
            if (Playlist.ContainsKey(playlistName))
            {
                if (Playlist[playlistName].Contains(file))
                {
                    Playlist[playlistName].Remove(file);
                    SavePlaylist();
                    return true;
                }
            }
            return false;
        }

    }
}
