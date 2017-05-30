using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Search;
using EPiServer.ServiceLocation;
using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Models.ViewModels
{
    public class SearchPageViewModel : PageViewModel<SearchPage>
    {
        private string _searchTerm;
        public IEnumerable<PageData> SearchResults { get; set; }
        public bool HasSearchResult { get; set; }

        public SearchPageViewModel(SearchPage currentPage) : base(currentPage)
        {
        }

        public SearchPageViewModel(SearchPage currentPage, string searchTerm) : this(currentPage)
        {
            _searchTerm = searchTerm;

            if (!string.IsNullOrEmpty(_searchTerm))
            {
                PropertyCriteriaCollection criterias = new PropertyCriteriaCollection
                {
                    new PropertyCriteria()
                    {
                        Name = "PageName",
                        Type = PropertyDataType.String,
                        Condition = EPiServer.Filters.CompareCondition.Equal,
                        Value = _searchTerm
                    }
                };

                var repository = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();

                SearchResults = repository.FindPagesWithCriteria(ContentReference.StartPage, criterias);
            }

            HasSearchResult = SearchResults != null;
        }

        
    }
}