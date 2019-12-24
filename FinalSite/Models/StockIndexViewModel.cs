using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models
{
    public class StockIndexViewModel
    {
        public int IdStock { get; set; }
        public string detalle_producto { get; set; }
        public Nullable<decimal> precio { get; set; }
        
        public  string  Username { get; set; }
        public  DateTime?  fecha_venta{ get; set; }
        public string estado { get; set; }
    }
}
