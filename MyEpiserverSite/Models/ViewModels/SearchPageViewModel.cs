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

                //PropertyCriteria secCriteria = new PropertyCriteria();
                //secCriteria.Condition = CompareCondition.StartsWith;
                //secCriteria.Name = "PageName";
                //secCriteria.Type = PropertyDataType.String;
                //secCriteria.Value = _searchTerm;
                //secCriteria.Required = true;
                //criterias.Add(secCriteria);

                PropertyCriteria frstCriteria = new PropertyCriteria();
                frstCriteria.Condition = CompareCondition.StartsWith;
                frstCriteria.Name = "PageName";
                frstCriteria.Type = PropertyDataType.String;
                frstCriteria.Value = _searchTerm;
                frstCriteria.Required = true;
                criterias.Add(frstCriteria);

                var repository = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();
                PageResults = repository.FindPagesWithCriteria(ContentReference.StartPage, criterias);
                FilterForVisitor.Filter(PageResults);
                new FilterSort(FilterSortOrder.PublishedDescending).Filter(PageResults);
            }

            HasSearchResult = PageResults != null;
        }

        
    }
}