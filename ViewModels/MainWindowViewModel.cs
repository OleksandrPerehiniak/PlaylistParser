using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using HtmlAgilityPack;
using PlaylistParser.Models;

using Microsoft.Playwright;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;




namespace PlaylistParser.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string urlInput = "https://music.amazon.com/playlists/B08BWK8W15";

        [ObservableProperty]
        private Playlist parsedPlaylist;

        [ObservableProperty]
        private string statusMessage = "Enter URL and Click Parse";

        [RelayCommand]
        private async Task ParseUrl()
        {
            StatusMessage = "Parsing...";
            ParsedPlaylist = null;


            try
            {
                
                var html = await FetchRenderedHtmlAsync(UrlInput);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var playlists = new List<Playlist>();

                //File.WriteAllText("rendered.html", html); // inspect this file
                //var header = doc.DocumentNode.SelectSingleNode("//music-detail-header");

                if (html.Contains("music-detail-header") == true) {
                    StatusMessage = true.ToString();
                }
                
                
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//music-detail-header");
                foreach (var node in nodes) 
                {
                
                    var test = node.GetAttributeValue("headline", "Unknown Playlist") ?? "Unknown Playlist";
                }

                //foreach(var node in nodes) 
                //{
                //    var playlist = new Playlist
                //    {
                //        Name = node.GetAttributeValue("headline", "Unknown Playlist") ?? "Unknown Playlist",

                //        AvatarUrl = node.GetAttributeValue("image-src", "default_avatar.png") ?? "default_avatar.png",

                //        Description = node.GetAttributeValue("primary-text", "No Description Found") ?? "No Description Found",
                //    };


                //}


                //// Parse song list
                //var songNodes = doc.DocumentNode.SelectNodes("//div[@data-test='track-row']");
                //if (songNodes != null)
                //{
                //    foreach (var node in songNodes)
                //    {
                //        var songName = node.SelectSingleNode(".//span[@data-test='track-title']")?.InnerText.Trim();
                //        var artistName = node.SelectSingleNode(".//a[@data-test='track-artist']")?.InnerText.Trim();
                //        var albumName = node.SelectSingleNode(".//a[@data-test='track-album']")?.InnerText.Trim();
                //        var duration = node.SelectSingleNode(".//span[@data-test='track-duration']")?.InnerText.Trim();

                //        if (!string.IsNullOrEmpty(songName))
                //        {
                //            playlist.Songs.Add(new Song
                //            {
                //                SongName = songName,
                //                ArtistName = artistName ?? "N/A",
                //                AlbumName = albumName ?? "N/A",
                //                Duration = duration ?? "N/A"
                //            });
                //        }
                //    }
                //}

                foreach (var playlist in playlists) 
                { 
                    ParsedPlaylist = playlist;
                    StatusMessage = $"✅ Successfully parsed: {playlist.Name} with {playlist.Songs.Count} songs.";
                
                }
            }
            catch (HttpRequestException)
            {
                StatusMessage = "❌ Error: Could not fetch URL. Check the address.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"⚠️ An error occurred during parsing: {ex.Message}";
            }
        }

        private async Task<string> FetchRenderedHtmlAsync(string url)
        {
            

            using var playwright = await Playwright.CreateAsync(); 
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false, }); 
            var context = await browser.NewContextAsync(); 
            var page = await context.NewPageAsync(); 
            await page.GotoAsync("https://music.amazon.com/playlists/B01M11SBC8");

            await Task.Delay(5000);
            var html = await page.ContentAsync();

            return html;

        }
    }
}