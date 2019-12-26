using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models
{
    public class StockIndexViewModel
    {
        public int IdStock { get; set; }
        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }
        public string estado { get; set; }
        public string descripcion_producto { get; set; }
        public Nullable<decimal> precio { get; set; }

        public string monto_pagado { get; set; }
        public  string  Username { get; set; }
        public  DateTime?  fecha_venta{ get; set; }
        public string codigo_pago { get; set; }
        public string codigoBP { get; set; }
    }
}
