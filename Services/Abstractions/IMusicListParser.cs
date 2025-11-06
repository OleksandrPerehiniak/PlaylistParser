using HtmlAgilityPack;
using PlaylistParser.Models;

namespace PlaylistParser.Services.Abstractions
{
    public interface IMusicListParser
    {
        public string ParserType { get; }
        Playlist Parse(HtmlDocument doc, HtmlNode node);
    }
}
