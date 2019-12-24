using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalSite.Models.Home;
using FinalSite.DAL;
using System.Data.Entity.Core.Objects;

namespace FinalSite.Controllers
{
    public class ProductosController : Controller
    {
        private DataBaseTestEntities db = new DataBaseTestEntities();

        [Authorize]
        public ActionResult Index()
        {
            IQueryable<Producto> productos = db.Productoes.Include(p => p.Brand).Include(p => p.Categoria);

            var lista = productos.GroupBy(t => t.precio).Select(t => t.FirstOrDefault()).ToList();

            return View(lista);

        }
        private int selectProducts(decimal precio)
        {
            DbSet<Producto> prod = db.Productoes;
            Producto query = (
                        from Producto in prod
                        where Producto.precio == precio
                        select Producto)
                        .First();
            Console.WriteLine("ProductoID: " + query.IdProducto);
            return query.IdProducto;

        }
        public ActionResult Test()
        {
            ViewBag.idProducto = selectProducts(50);
            return View();
        }

        
        // GET: Productos/Details/5
        public ActionResult Details(int? IdProducto)
        {
            if (IdProducto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(IdProducto);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "Descripcion_brand");
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "descripcion_categoria");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProducto,descripcion_producto,condiciones_uso,IdCategoria,IdBrand,precio_producto,Imagen,Lote")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productoes.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "Descripcion_brand", producto.IdBrand);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "descripcion_categoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "Descripcion_brand", producto.IdBrand);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "descripcion_categoria", producto.IdCategoria);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProducto,descripcion_producto,condiciones_uso,IdCategoria,IdBrand,precio_producto,Imagen,Lote")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "Descripcion_brand", producto.IdBrand);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "descripcion_categoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }
        [Authorize(Roles = "Admin")]
        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productoes.Find(id);
            db.Productoes.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddToCart(int IdProducto)
        {
            var product = db.Productoes.Find(IdProducto);
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item()
                {
                    producto = product,
                    cantidad = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int Index = getIndex(IdProducto);
                if (Index == -1)
                {
                    cart.Add(new Item()
                    {
                        producto = product,
                        cantidad =1

                    });
                }
                else
                {
                    cart[Index].cantidad += 1;
                    Session["cart"] = cart;
                }
            }
            return Redirect("Index");
        }
            
        private int getIndex(int IdProducto)
        {
            List<Item> lista = (List<Item>)Session["cart"];
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].producto.IdProducto == IdProducto)
                    return i;
            }

            return -1;
        }
        public ActionResult RemoveFromCart(int IdProducto)
        {
            List<Item> cart = (List<Item>)Session["Cart"];

            foreach (var item in cart)
            {
                if (item.producto.IdProducto == IdProducto)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session["Cart"] = cart;
            return Redirect("Index");
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult CheckoutDetails()
        {
            return View();
        }

    }
}
  