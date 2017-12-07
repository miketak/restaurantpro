using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Address
    {
        public int UserAddressId { get; set; }

        public List<String> AddressLines { get; set; }

        public string City { get; set; }

        public int StateID { get; set; }

        public string Zip { get; set; }

        public int CountryID { get; set; }

        public int AddressTypeId { get; set; }


    }
}
