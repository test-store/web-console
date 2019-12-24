using FinalSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models
{
    public class ProductDetail
    {
        public int IdProducto { get; set; }
        public string descripcion_producto { get; set; }
        public string condiciones_uso { get; set; }
        public Nullable<int> IdCategoria { get; set; }
        public Nullable<int> IdBrand { get; set; }
        public Nullable<int> precio_producto { get; set; }
        public string Imagen { get; set; }
        public string Lote { get; set; }
    }
    public class BrandDetail
    {   
        public int IdBrand { get; set; }
        public string Descripcion_brand { get; set; }
        public int IdBusinessPartner { get; set; }
    }

    public class BusinnesPartnerDetail {
        public int IdBusinessPartner { get; set; }
        public string RUC { get; set; }
        public string razon_social { get; set; }
        public System.DateTime fecha_alta { get; set; }

    }

    public class CategoryDetail 
    {   
        public int IdCategoria { get; set; }
        public string descripcion_categoria { get; set; }
        public string grupo { get; set; }
    }

    public class UserDetail
    {
        public string Id { get; set; }
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string estado { get; set; }


    }

    public class StockDetail
    {
        public int IdStock { get; set; }
        public Nullable<int> IdProducto { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<int> IdPago { get; set; }
        public string estado { get; set; }
        public string CodigoBP { get; set; }
        public Nullable<System.DateTime> fecha_venta { get; set; }
        public decimal monto_pagado { get; set; }
    }



}