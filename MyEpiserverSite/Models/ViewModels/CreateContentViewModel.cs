using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Models.ViewModels
{
    public class CreateContentViewModel
    {
        public string Description { get; set; }

        [Required]
        public string Text { get; set; }
        
        [Required]
        public ContentReference ParentId { get; set; }

        public IEnumerable<ContentReference> ContentReferences { get; set; }
       
        public IEnumerable<PageData> Pages { get; set; }

        public IEnumerable<LanguageBranch> Languages { get; set; }

        public CultureInfo Language { get; set; }
    }
}