using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models.Home
{
    public class Card
    {
        public string cardNumber { get; set; }
        public string cvv { get; set; }
        public int expiration_month { get; set; }
        public int expiration_year { get; set; }

        public string email { get; set; }

        public decimal IdProducto { get; set; }


    }
}