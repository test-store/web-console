using FinalSite.Repository;
using FinalSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using FinalSite.DAL;

namespace FinalSite.Controllers
{
    public class AdminController : Controller
    {
        DataBaseTestEntities context = new DataBaseTestEntities();
        public ActionResult Index()
        {
            return Redirect("AllProducts");
        }
        public ActionResult AllUsers()
        {
            var users = context.AspNetUsers.ToList();
            return View(users);
        }
        public ActionResult AllStocks()
        {
            List<StockIndexViewModel> results = (from pr in context.Productoes
                                                 join st in context.Stocks on pr.IdProducto equals st.IdProducto
                                                 join usr in context.AspNetUsers on st.IdUsuario equals usr.IdUsuario
                                                 select new StockIndexViewModel()
                                                 {
                                                     IdStock = st.IdStock,
                                                     detalle_producto = pr.descripcion_producto,
                                                     precio = pr.precio,
                                                     Username = usr.UserName,
                                                     fecha_venta = st.fecha_venta,
                                                     estado = st.estado
                                                 }).ToList();


            return View(results);

        }
        public ActionResult AllProducts()
        {
            var products = context.Productoes.Include(p => p.Brand).Include(p => p.Categoria);
            return View(products.ToList());
        }

        public ActionResult AllBrands()
        {
            var brands = context.Brands.Include(b => b.BusinessPartner);
            return View(brands.ToList());
        }
        public ActionResult AllCategories()
        {
            var categories = context.Categorias;
            return View(categories.ToList());
        }
    }
}