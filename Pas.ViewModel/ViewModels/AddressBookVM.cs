using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class AddressBookVM
    {
        public int Id { get; set; }
        
        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public string LocalArea { get; set; }
        public string PostCode { get; set; }
        
        public string City { get; set; }
        
        [Required]
        public int CityId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string FullAddress() => $"{AddressLine1 ?? ""}, {LocalArea ?? ""}, {City ?? ""}";

        public DateTime LastUpdated { get; set; }

        //public string CityName() => ((Common.Enums.City) cityId).Name();
    }
}
