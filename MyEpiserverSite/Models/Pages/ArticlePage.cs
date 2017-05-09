using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "ArticlePage", GUID = "c9f58b29-caa7-4749-b571-3c9a6da2d758", Description = "Basic page type for creating articles.")]
    public class ArticlePage : StandardPage
    {
        public virtual string Heading { get; set; }

        [UIHint(UIHint.Textarea)]
        public virtual string Introduction { get; set; }
    }
}