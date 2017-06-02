using System;
using System.Collections.Generic;
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
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    public class SearchPageController : PageControllerBase<SearchPage>
    {
        private const int MaxResults = 40;

        public ActionResult Index(SearchPage currentPage, string q)
        {
            var model = new SearchPageViewModel(currentPage)
            {
                SearchedQuery = q
            };

            if (!string.IsNullOrWhiteSpace(q))
            {
                var hits = Search(q.Trim(),
                    new[] { SiteDefinition.Current.StartPage, SiteDefinition.Current.GlobalAssetsRoot, SiteDefinition.Current.SiteAssetsRoot },
                    ControllerContext.HttpContext,
                    currentPage.LanguageID).IndexResponseItems;

                model.Urls = new List<string>();
                model.Results = new List<IndexResponseItem>();
                foreach (var indexResponseItem in hits)
                {
                    model.Results.Add(indexResponseItem);
                    var uri = CreateHits(indexResponseItem);                    
                    model.Urls.Add(uri.ToString());
                }
                model.NumberOfHits = hits.Count;
            }
            return View(model);
        }

        private SearchResults Search(string searchText, IEnumerable<ContentReference> searchRoots, HttpContextBase context, string languageBranch)
        {
            var query = CreateQuery(searchText, searchRoots, context, languageBranch);
            var results = SearchHandler.Instance.GetSearchResults(query, 1, 10);
            
            return results;
        }

        private Uri CreateHits(IndexResponseItem responseItem)
        {
            UrlBuilder url = new UrlBuilder(responseItem.Uri);
            Global.UrlRewriteProvider.ConvertToExternal(url, responseItem, System.Text.Encoding.UTF8);
            return url.Uri;
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