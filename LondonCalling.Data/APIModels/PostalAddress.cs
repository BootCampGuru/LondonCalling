using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels
{
    public class PostalAddress
    {
        public string stateOrRegion { get; set; }
        public string city { get; set; }
        public string countryCode { get; set; }
        public string postalCode { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string districtOrCounty { get; set; }
    }
}
