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
    //[Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        DataBaseTestEntities context = new DataBaseTestEntities();
        public ActionResult AllBP()
        {
            var BP = context.BusinessPartners.ToList();
            return View(BP);
        }
        public ActionResult Index()
        {
            return RedirectToAction("AllProducts");
        }
        public ActionResult Dashboard()
        {
            return RedirectToAction("Index");
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
                                                 where st.estado == "no disponible"
                                                 select new StockIndexViewModel()
                                                 {
                                                     IdStock = st.IdStock,
                                                     estado = st.estado,
                                                     descripcion_producto= pr.descripcion_producto,
                                                     Username = usr.UserName,
                                                     fecha_venta = st.fecha_venta,
                                                     codigo_pago = st.codigo_pago,
                                                     codigoBP = st.CodigoBP,
                                                     precio = pr.precio 
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