using System.Threading.Tasks;

namespace PlaylistParser.Services.Abstractions
{
    public interface IHtmlLoader
    {
        Task<string> FetchRenderedHtmlAsync(string url);
    }
}
