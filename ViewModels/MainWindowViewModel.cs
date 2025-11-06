using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using PlaylistParser.Models;
using PlaylistParser.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlaylistParser.ViewModels
{
    public partial class MainWindowViewModel(IEnumerable<IMusicListParser> musicListParsers, IHtmlLoader htmlLoader) : ViewModelBase
    {
        private const string DefaultUrl = "https://music.amazon.com/playlists/B08BWK8W15";
        private const string InitialStatusMessage = "Enter URL and Click Parse";
        private const string ParsingStatusMessage = "Parsing...";
        private const string DetailHeaderXPath = "/html/body/div/div[3]/div/music-app/div/div/div/div/div[2]/music-detail-header";
        private const string LabelAttribute = "label";
        private const string DefaultLabel = "Unknown Playlist";
        private const string NoParserFoundErrorMessage = "❌ Error: No suitable parser found for this URL.";
        private const string ParsingFailedErrorMessage = "❌ Error: Parsing failed.";
        private const string SuccessMessageFormat = "✅ Successfully parsed: {0} with {1} songs.";
        private const string FetchErrorMessage = "❌ Error: Could not fetch URL. Check the address.";
        private const string GenericErrorMessageFormat = "⚠️ An error occurred during parsing: {0}";

        [ObservableProperty]
        private string urlInput = DefaultUrl;

        [ObservableProperty]
        private Playlist parsedPlaylist;

        [ObservableProperty]
        private string statusMessage = InitialStatusMessage;

        [RelayCommand]
        private async Task ParseUrl()
        {
            StatusMessage = ParsingStatusMessage;
            ParsedPlaylist = null;

            try
            {
                var html = await htmlLoader.FetchRenderedHtmlAsync(UrlInput);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var node = doc.DocumentNode.SelectSingleNode(DetailHeaderXPath);
                var label = node.GetAttributeValue(LabelAttribute, DefaultLabel);

                var playlistParser = musicListParsers.FirstOrDefault(x => x.ParserType == label);
                if (playlistParser is null)
                {
                    StatusMessage = NoParserFoundErrorMessage;
                    return;
                }
                var playlist = playlistParser?.Parse(doc, node);
                if (playlist is null)
                {
                    StatusMessage = ParsingFailedErrorMessage;
                    return;
                }
                ParsedPlaylist = playlist;
                StatusMessage = string.Format(SuccessMessageFormat, playlist.Name, playlist.Songs.Count);
            }
            catch (HttpRequestException)
            {
                StatusMessage = FetchErrorMessage;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format(GenericErrorMessageFormat, ex.Message);
            }
        }
    }
}