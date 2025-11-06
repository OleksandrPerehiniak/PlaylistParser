using System.Collections.ObjectModel;

namespace PlaylistParser.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
        public ObservableCollection<Song> Songs { get; set; } = new ObservableCollection<Song>();
    }
}
