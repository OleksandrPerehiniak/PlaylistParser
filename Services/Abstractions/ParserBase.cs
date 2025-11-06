using HtmlAgilityPack;
using PlaylistParser.Models;

namespace PlaylistParser.Services.Abstractions
{
    public abstract class ParserBase
    {
        private const string PlaylistNameAttribute = "headline";
        private const string DefaultPlaylistName = "Unknown Playlist";
        private const string AvatarUrlAttribute = "image-src";
        private const string DefaultAvatarUrl = "default_avatar.png";
        private const string DescriptionAttribute = "secondary-text";
        private const string DefaultDescription = "No Description Found";
        private const string SongNameAttribute = "primary-text";
        private const string DefaultSongName = "Unknown Song";
        private const string DurationNodeXPath = ".//music-link[@kind='secondary']/span";
        private const string DefaultDuration = "00:00";

        protected abstract string SongNodeString { get; }
        protected abstract string GetAlbumName(HtmlNode songNode, HtmlNode titleNode);
        protected abstract string GetArtistName(HtmlNode songNode, HtmlNode titleNode);
        public Playlist Parse(HtmlDocument doc, HtmlNode node)
        {
            var playlist = new Playlist
            {
                Name = node.GetAttributeValue(PlaylistNameAttribute, DefaultPlaylistName),

                AvatarUrl = node.GetAttributeValue(AvatarUrlAttribute, DefaultAvatarUrl),

                Description = node.GetAttributeValue(DescriptionAttribute, DefaultDescription),
            };

            var songNodes = doc.DocumentNode.SelectNodes(SongNodeString);
            if (songNodes != null)
            {
                foreach (var songNode in songNodes)
                {
                    var songName = songNode.GetAttributeValue(SongNameAttribute, DefaultSongName);
                    var duration = songNode.SelectSingleNode(DurationNodeXPath)?.GetDirectInnerText()?.Trim();

                    if (!string.IsNullOrEmpty(songName))
                    {
                        playlist.Songs.Add(new Song
                        {
                            SongName = songName,
                            Duration = duration ?? DefaultDuration,
                            AlbumName = GetAlbumName(songNode, node),
                            ArtistName = GetArtistName(songNode, node)
                        });
                    }
                }
            }

            return playlist;
        }
    }
}
