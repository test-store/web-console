using FinalSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models
{
    public class ProductStock
    {
        
        public int IdProducto { get; set; }
        public string nombre_producto { get; set; }
        public string  Imagen { get; set; }
        public decimal precio { get; set; }
        public string estado { get; set; }
        
    }
}