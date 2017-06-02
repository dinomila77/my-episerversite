using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Search;
using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Models.ViewModels
{
    public class SearchPageViewModel : PageViewModel<SearchPage>
    {
        public SearchPageViewModel(SearchPage currentPage) : base(currentPage)
        {
        }

        public bool SearchServiceDisabled { get; set; }
        public string SearchedQuery { get; set; }
        public int NumberOfHits { get; set; }
        public List<IndexResponseItem> Results { get; set; }
        public List<string> Urls { get; set; }
    }
}