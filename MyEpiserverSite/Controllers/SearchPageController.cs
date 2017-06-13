using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery.Internal;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Search;
using EPiServer.Search.Queries;
using EPiServer.Search.Queries.Lucene;
using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyEpiserverSite.Business;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;
using PagedList;

namespace MyEpiserverSite.Controllers
{
    public class SearchPageController : PageControllerBase<SearchPage>
    {

        //#region'orgcode'
        //private const int MaxResults = 20;

        //public ActionResult Index(SearchPage currentPage, string q, int page = 1)
        //{
        //    var model = new SearchPageViewModel(currentPage)
        //    {
        //        SearchedQuery = q
        //    };

        //    if (!string.IsNullOrWhiteSpace(q))
        //    {
        //        var hits = Search(q.Trim(),
        //            new[] { SiteDefinition.Current.StartPage, SiteDefinition.Current.GlobalAssetsRoot, SiteDefinition.Current.SiteAssetsRoot },
        //            ControllerContext.HttpContext,
        //            currentPage.LanguageID, 
        //            MaxResults);

        //        model.PageHits = hits.ToPagedList(page,2);
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_SearchResults", model);
        //    }
        //    return View(model);
        //}

        //private IEnumerable<SearchPageViewModel.SearchHit>Search(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch, int pages)
        //{
        //    var query = CreateQuery(searchText, searchRoots, context, languageBranch);
        //    var results = SearchHandler.Instance.GetSearchResults(query, 1, pages);

        //    return results.IndexResponseItems.SelectMany(CreateUrl);
        //}

        //private IEnumerable<SearchPageViewModel.SearchHit> CreateUrl(IndexResponseItem responseItem)
        //{
        //    UrlBuilder urlbuilder = new UrlBuilder(responseItem.Uri);
        //    Global.UrlRewriteProvider.ConvertToExternal(urlbuilder, responseItem, System.Text.Encoding.UTF8);
        //    var url = urlbuilder.Path;
        //    yield return CreatePageHit(responseItem,url);
        //}

        //private SearchPageViewModel.SearchHit CreatePageHit(IndexResponseItem item,string url)
        //{
        //    return new SearchPageViewModel.SearchHit
        //    {
        //        Title = item.Title,
        //        Url = url              
        //    };
        //}

        //private IQueryExpression CreateQuery(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch)
        //{
        //    var query = new GroupQuery(LuceneOperator.AND);
        //    query.QueryExpressions.Add(new FieldQuery(searchText));

        //    //Search for pages using the provided language
        //    var pageTypeQuery = new GroupQuery(LuceneOperator.AND);
        //    pageTypeQuery.QueryExpressions.Add(new ContentQuery<PageData>());
        //    pageTypeQuery.QueryExpressions.Add(new FieldQuery(languageBranch, Field.Culture));

        //    //Search for media without languages
        //    var contentTypeQuery = new GroupQuery(LuceneOperator.OR);
        //    contentTypeQuery.QueryExpressions.Add(new ContentQuery<MediaData>());
        //    contentTypeQuery.QueryExpressions.Add(pageTypeQuery);

        //    query.QueryExpressions.Add(contentTypeQuery);

        //    var typeQueries = new GroupQuery(LuceneOperator.OR);
        //    query.QueryExpressions.Add(typeQueries);

        //    foreach (var root in searchRoots)
        //    {
        //        var contentRootQuery = new VirtualPathQuery();
        //        contentRootQuery.AddContentNodes(root);
        //        typeQueries.QueryExpressions.Add(contentRootQuery);
        //    }

        //    var accessRightsQuery = new AccessControlListQuery();
        //    accessRightsQuery.AddAclForUser(PrincipalInfo.Current, context);
        //    query.QueryExpressions.Add(accessRightsQuery);

        //    return query;

        //}
        //#endregion

        private const int MaxResults = 40;
        private readonly SearchService _searchService;
        private readonly ContentSearchHandler _contentSearchHandler;
        private readonly UrlResolver _urlResolver;
        private readonly TemplateResolver _templateResolver;

        public SearchPageController(
            SearchService searchService,
            ContentSearchHandler contentSearchHandler,
            TemplateResolver templateResolver,
            UrlResolver urlResolver)
        {
            _searchService = searchService;
            _contentSearchHandler = contentSearchHandler;
            _templateResolver = templateResolver;
            _urlResolver = urlResolver;
        }

        [ValidateInput(false)]  
        public ViewResult Index(SearchPage currentPage, string q, int page = 1)
        {
            var model = new SearchPageViewModel(currentPage)
            {
                SearchServiceDisabled = !_searchService.IsActive,
                SearchedQuery = q
            };

            if (!string.IsNullOrWhiteSpace(q) && _searchService.IsActive)
            {
                var hits = Search(q.Trim(),
                    new[] { SiteDefinition.Current.StartPage, SiteDefinition.Current.GlobalAssetsRoot, SiteDefinition.Current.SiteAssetsRoot },
                    ControllerContext.HttpContext,
                    currentPage.LanguageID).ToList();
                model.Hits = hits.ToPagedList(page,2);
                model.NumberOfHits = hits.Count();
            }

            return View(model);
        }

        /// <summary>
        /// Performs a search for pages and media and maps each result to the view model class SearchHit.
        /// </summary>
        /// <remarks>
        /// The search functionality is handled by the injected SearchService in order to keep the controller simple.
        /// Uses EPiServer Search. For more advanced search functionality such as keyword highlighting,
        /// facets and search statistics consider using EPiServer Find.
        /// </remarks>
        private IEnumerable<SearchPageViewModel.SearchHit> Search(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch)
        {
            var searchResults = _searchService.Search(searchText, searchRoots, context, languageBranch, MaxResults);

            return searchResults.IndexResponseItems.SelectMany(CreateHitModel);
        }

        private IEnumerable<SearchPageViewModel.SearchHit> CreateHitModel(IndexResponseItem responseItem)
        {
            var content = _contentSearchHandler.GetContent<IContent>(responseItem);
            if (content != null && HasTemplate(content) && IsPublished(content as IVersionable) && IsVisible(content))
            {
                yield return CreatePageHit(content, responseItem);
            }
        }

        private bool IsVisible(IContent content)
        {
            var visible = content.Property.Get("PageVisibleInMenu").Value;
            return visible.Equals(true);
        }

        private bool HasTemplate(IContent content)
        {
            return _templateResolver.HasTemplate(content, TemplateTypeCategories.Page);
        }

        private bool IsPublished(IVersionable content)
        {
            if (content == null)
                return true;
            return content.Status.HasFlag(VersionStatus.Published);
        }

        private SearchPageViewModel.SearchHit CreatePageHit(IContent content, IndexResponseItem responseItem)
        {
            return new SearchPageViewModel.SearchHit
            {
                Title = content.Name,
                Url = _urlResolver.GetUrl(content.ContentLink),
                Excerpt = responseItem.DisplayText
                //Excerpt = content is SitePageData ? ((SitePageData)content).TeaserText : string.Empty
            };
        }
    }
}