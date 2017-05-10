using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEpiserverSite.Models
{
    public class ShippingAddress
    {
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string AddressLine2 { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Postcode { get; set; }
    }
}