using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class AddressBookVM
    {
        public int Id { get; set; }
        
        public string AddressLine1 { get; set; }

        public string LocalArea { get; set; }

        public string City { get; set; }

        public DateTime DateCreated { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string FullAddress() => $"{AddressLine1 ?? ""}, {LocalArea ?? ""}, {City ?? ""}";

        //public string CityName() => ((Common.Enums.City) cityId).Name();
    }
}
