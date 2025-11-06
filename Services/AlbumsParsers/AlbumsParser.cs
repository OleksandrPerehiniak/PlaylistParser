using HtmlAgilityPack;
using PlaylistParser.Services.Abstractions;

namespace PlaylistParser.Services.AlbumsParsers
{
    public class AlbumsParser : ParserBase, IMusicListParser
    {
        private const string AlbumParserTypeValue = "Album";
        private const string SongNodeXPath = "/html/body/div/div[3]/div/music-app/div/div/div/div/music-container/music-container/div/div/div[2]/div/div/music-text-row";
        private const string AlbumNameAttribute = "headline";
        private const string DefaultAlbumName = "Unknown Playlist";
        private const string ArtistNameAttribute = "primary-text";
        private const string DefaultArtistName = "Unknown Artist";

        public string ParserType => AlbumParserTypeValue;
        protected override string SongNodeString => SongNodeXPath;

        protected override string GetAlbumName(HtmlNode songNode, HtmlNode titleNode)
        {
           return titleNode.GetAttributeValue(AlbumNameAttribute, DefaultAlbumName);
        }

        protected override string GetArtistName(HtmlNode songNode, HtmlNode titleNode)
        {
            return titleNode.GetAttributeValue(ArtistNameAttribute, DefaultArtistName);
        }
    }
}
