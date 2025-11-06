using HtmlAgilityPack;
using PlaylistParser.Services.Abstractions;

namespace PlaylistParser.Services.PlaylistsParsers
{
    public class PlaylistsParser : ParserBase, IMusicListParser
    {
        private const string PlaylistParserTypeValue = "playlist";
        private const string SongNodeXPath = "/html/body/div/div[3]/div/music-app/div/div/div/div/music-container/music-container/div/div/div[2]/div/div/music-image-row";
        private const string AlbumNameAttribute = "secondary-text-2";
        private const string DefaultAlbumName = "Unknown Album";
        private const string ArtistNameAttribute = "secondary-text-1";
        private const string DefaultArtistName = "Unknown Artist";

        public string ParserType => PlaylistParserTypeValue;

        protected override string SongNodeString => SongNodeXPath;

        protected override string GetAlbumName(HtmlNode songNode, HtmlNode titleNode)
        {
            return songNode.GetAttributeValue(AlbumNameAttribute, DefaultAlbumName);
        }

        protected override string GetArtistName(HtmlNode songNode, HtmlNode titleNode)
        {
           return songNode.GetAttributeValue(ArtistNameAttribute, DefaultArtistName);
        }
    }
}
