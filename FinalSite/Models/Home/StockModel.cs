using FinalSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models.Home
{
    public class StockModel
    {
        DataBaseTestEntities db = new DataBaseTestEntities();

        public int IdProducto { get; set; }
        public Producto getProduct(int IdProducto)
        {
            Producto prod=db.Productoes.Where(t => t.IdProducto == IdProducto).FirstOrDefault();
            return prod;
        }
    }
}