using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class PaymentDataModel
    {
        public string CardNumber { get; set; }

        public string CardExpiryDate { get; set; }

        public string CCV { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZIP { get; set; }

        public string Country { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string Phone { get; set; }
    }
}