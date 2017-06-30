using MyEpiserverSite.Models.Pages;
using PagedList;

namespace MyEpiserverSite.Models.ViewModels
{
    public class SearchPageViewModel : PageViewModel<SearchPage>
    {
        public SearchPageViewModel(SearchPage currentPage) : base(currentPage)
        {
        }

        public string SearchedQuery { get; set; }
        public IPagedList<SearchHit> PageHits { get; set; }

        public class SearchHit
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Excerpt { get; set; }
        }
    }
}