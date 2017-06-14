using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery.Internal;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Search;
using EPiServer.Search.Queries;
using EPiServer.Search.Queries.Lucene;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;
using PagedList;

namespace MyEpiserverSite.Controllers
{
    public class SearchPageController : PageControllerBase<SearchPage>
    {
        private const int MaxResults = 100;

        public ActionResult Index(SearchPage currentPage, string q, int page = 1)
        {
            var model = new SearchPageViewModel(currentPage)
            {
                SearchedQuery = q
            };

            var watch = new Stopwatch();

            if (!string.IsNullOrWhiteSpace(q))
            {
                IEnumerable<SearchPageViewModel.SearchHit> hits = new List<SearchPageViewModel.SearchHit>();
                watch.Start();
                for (int i = 0; i < 100; i++)
                {
                    hits = Search(q.Trim(),
                    new[] { SiteDefinition.Current.StartPage, SiteDefinition.Current.GlobalAssetsRoot, SiteDefinition.Current.SiteAssetsRoot },
                    ControllerContext.HttpContext,
                    currentPage.LanguageID,
                    MaxResults);
                }
                model.PageHits = hits.ToPagedList(page, 10);
                watch.Stop();
            }
            if (Request.IsAjaxRequest())
            {
                var ms = watch.ElapsedMilliseconds.ToString();
                TempData["time"] = $"{ms}";
                return PartialView("_SearchResults", model);
            }
            return View(model);
        }

        private IEnumerable<SearchPageViewModel.SearchHit>Search(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch, int pages)
        {
            var query = CreateQuery(searchText, searchRoots, context, languageBranch);
            var results = SearchHandler.Instance.GetSearchResults(query, 1, pages);
            
            return results.IndexResponseItems.SelectMany(CreateUrl);
        }

        private IEnumerable<SearchPageViewModel.SearchHit> CreateUrl(IndexResponseItem responseItem)
        {
            #region 'Code1'

            //UrlBuilder urlbuilder = new UrlBuilder(responseItem.Uri);
            //Global.UrlRewriteProvider.ConvertToExternal(urlbuilder, responseItem, System.Text.Encoding.UTF8);
            //var url = urlbuilder.Path;
            //
            //yield return CreatePageHit(responseItem, url);

            #endregion

            #region 'Code2'

            //var contentSearchHandler = ServiceLocator.Current.GetInstance<ContentSearchHandler>();
            //var content = contentSearchHandler.GetContent<IContent>(responseItem);
            //if (IsVisible(content))
            //{
            //    UrlResolver resolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            //    var url = resolver.GetUrl(content.ContentLink);

            //    yield return CreatePageHit(responseItem, url);
            //}

            #endregion

            #region 'Code3'

            var contentSearchHandler = ServiceLocator.Current.GetInstance<ContentSearchHandler>();
            var content = contentSearchHandler.GetContent<PageData>(responseItem);

            if (content.VisibleInMenu)
            {
                var url = UrlResolver.Current.GetUrl(content.ContentLink);
                yield return CreatePageHit(responseItem, url);
            }
            #endregion
        }

        private bool IsVisible(IContent content)
        {
            var visible = content.Property.Get("PageVisibleInMenu").Value;
            return visible.Equals(true);
        }

        private SearchPageViewModel.SearchHit CreatePageHit(IndexResponseItem item,string url)
        {
            return new SearchPageViewModel.SearchHit
            {
                Title = item.Title,
                Url = url              
            };
        }

        private IQueryExpression CreateQuery(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch)
        {
            var query = new GroupQuery(LuceneOperator.AND);
            query.QueryExpressions.Add(new FieldQuery(searchText));

            //Search for pages using the provided language
            var pageTypeQuery = new GroupQuery(LuceneOperator.AND);
            pageTypeQuery.QueryExpressions.Add(new ContentQuery<PageData>());
            pageTypeQuery.QueryExpressions.Add(new FieldQuery(languageBranch, Field.Culture));

            //Search for media without languages
            var contentTypeQuery = new GroupQuery(LuceneOperator.OR);
            contentTypeQuery.QueryExpressions.Add(new ContentQuery<MediaData>());
            contentTypeQuery.QueryExpressions.Add(pageTypeQuery);

            query.QueryExpressions.Add(contentTypeQuery);

            var typeQueries = new GroupQuery(LuceneOperator.OR);
            query.QueryExpressions.Add(typeQueries);

            foreach (var root in searchRoots)
            {
                var contentRootQuery = new VirtualPathQuery();
                contentRootQuery.AddContentNodes(root);
                typeQueries.QueryExpressions.Add(contentRootQuery);
            }

            var accessRightsQuery = new AccessControlListQuery();
            accessRightsQuery.AddAclForUser(PrincipalInfo.Current, context);
            query.QueryExpressions.Add(accessRightsQuery);

            return query;
            
        }
    }
}