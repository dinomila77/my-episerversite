using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Models.ViewModels
{
    public interface IPageViewModel<out T> where T : SitePageData
    {
        T CurrentPage { get; }
    }
}
