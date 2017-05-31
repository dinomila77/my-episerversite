using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.Search;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Models.ViewModels
{
    public class SearchPageViewModel : PageViewModel<SearchPage>
    {
        private string _searchTerm;
        //public IEnumerable<PageData> SearchResults { get; set; }
        public PageDataCollection PageResults { get; set; }
        public bool HasSearchResult { get; set; }

        public SearchPageViewModel(SearchPage currentPage) : base(currentPage)
        {
        }

        public SearchPageViewModel(SearchPage currentPage, string searchTerm) : this(currentPage)
        {
            _searchTerm = searchTerm;
            
            if (!string.IsNullOrEmpty(_searchTerm))
            {
                PropertyCriteriaCollection criterias = new PropertyCriteriaCollection();

                PropertyCriteria frstCriteria = new PropertyCriteria
                {
                    Condition = CompareCondition.Contained,
                    Name = "MainBody",
                    Type = PropertyDataType.String,
                    Value = _searchTerm,
                    Required = false
                };
                criterias.Add(frstCriteria);

                PropertyCriteria secCriteria = new PropertyCriteria
                {
                    Condition = CompareCondition.StartsWith,
                    Name = "PageName",
                    Type = PropertyDataType.String,
                    Value = _searchTerm,
                    Required = false
                };
                criterias.Add(secCriteria);

                PropertyCriteria thrdCriteria = new PropertyCriteria
                {
                    Condition = CompareCondition.StartsWith,
                    Name = "Introduction",
                    Type = PropertyDataType.String,
                    Value = _searchTerm,
                    Required = false
                };
                criterias.Add(thrdCriteria);

                var repository = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();
                PageResults = repository.FindPagesWithCriteria(ContentReference.StartPage, criterias);

                FilterForVisitor.Filter(PageResults);
                new FilterSort(FilterSortOrder.PublishedDescending).Filter(PageResults);
            }

            HasSearchResult = PageResults != null;
        }  
    }
}