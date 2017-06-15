using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const int MaxResults = 20;

        public ActionResult Index(SearchPage currentPage, string q, int page = 1)
        {
            var model = new SearchPageViewModel(currentPage)
            {
                SearchedQuery = q
            };
            
            if (!string.IsNullOrWhiteSpace(q))
            {
                var hits = Search(q.Trim(),
                    new[]
                    {
                        SiteDefinition.Current.StartPage, SiteDefinition.Current.GlobalAssetsRoot, SiteDefinition.Current.SiteAssetsRoot
                    },
                    ControllerContext.HttpContext,
                    currentPage.LanguageID, 
                    MaxResults).ToPagedList(page, 2);

                model.PageHits = hits;
            }
            if (Request.IsAjaxRequest())
            {
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
            #region 'Urlbuilder'
            //UrlBuilder urlbuilder = new UrlBuilder(responseItem.Uri);
            //Global.UrlRewriteProvider.ConvertToExternal(urlbuilder, responseItem, System.Text.Encoding.UTF8);
            //var url = urlbuilder.Path;
            #endregion

            var contentSearchHandler = ServiceLocator.Current.GetInstance<ContentSearchHandler>();
            var content = contentSearchHandler.GetContent<PageData>(responseItem);
            //if (content.VisibleInMenu)
            {
                var url = UrlResolver.Current.GetUrl(content.ContentLink);
                yield return CreatePageHit(responseItem, url);
            }
        }

        private SearchPageViewModel.SearchHit CreatePageHit(IndexResponseItem item,string url)
        {
            string excerpt = string.Empty;
            if (item.DisplayText.Length > 0)
            {
                excerpt = Truncate(item);
            }
            
            return new SearchPageViewModel.SearchHit
            {
                Title = item.Title,
                Url = url,
                Excerpt = excerpt
            };
        }

        private string Truncate(IndexResponseItem item)
        {
            
            string res = string.Empty;
            string[] words = item.DisplayText.Split(' ');

            if (words.Length < 25)
            {
                return item.DisplayText;
            }
            for (int i = 0; i < 25; i++)
            {
                res += words[i] + " ";
            }
            return res;
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