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
using Rotativa.MVC;
using FinalSite.Helper;
using Newtonsoft.Json.Linq;
using FinalSite.Models;
using FinalSite.Infraestructure;
using Microsoft.AspNet.Identity;

namespace FinalSite.Controllers
{
    public class ProductosController : Controller
    {
        private DataBaseTestEntities db = new DataBaseTestEntities();


        //public ActionResult Index()
        //{
        //    IQueryable<Producto> productos = db.Productoes.Include(p => p.Brand).Include(p => p.Categoria);

        //    var lista = productos.GroupBy(t => t.precio).Select(t => t.FirstOrDefault()).ToList();

        //    return View(lista);

        //}
        [Authorize]
        public ActionResult Index()
        {

            IQueryable<ProductStock> productos_with_S = (from pr in db.Productoes
                                                         join st in db.Stocks on pr.IdProducto equals st.IdProducto
                                                         where st.estado == "disponible"
                                                         select new ProductStock()
                                                         {
                                                             IdProducto = pr.IdProducto,
                                                             nombre_producto = pr.descripcion_producto,
                                                             Imagen = pr.Imagen,
                                                             precio = (decimal)pr.precio,
                                                             estado = st.estado
                                                         });
            var lista = productos_with_S.GroupBy(t => t.precio).Select(t => t.FirstOrDefault()).ToList();

            return View(lista);

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
        public ActionResult GenerarPDF()
        {
            return new ActionAsPdf("CheckoutDetails", new { nombre = "Danny" }) { FileName = "a.pdf"};
        }
        public string generarToken(Card card) {
            Dictionary<string, object> map = new Dictionary<string, object>
            {
                    {"card_number", card.cardNumber},
                    {"cvv", card.cvv},
                    {"expiration_month", card.expiration_month},
                    {"expiration_year", card.expiration_year},
                    {"email", card.email}
            };
            string data = new Token(security()).Create(map);
            return data;
        }
        [HttpPost]
        public ActionResult RealizarPagoList(Card card)
        {
            string data = generarToken(card);
            List<Item> lista = (List<Item>)Session["cart"];
            string orderList="";
            List<int> idProductos = new List<int>();

            var suma = 0;
            foreach (var item in lista)
            {
                suma = (int)(suma + (item.producto.precio*item.cantidad));
                orderList = orderList+"-"+item.producto.IdProducto.ToString();
                idProductos.Add(item.producto.IdProducto);
            }
            ViewBag.listaIdProductos = idProductos;

           // var producto = db.Productoes.Find(IdProducto);
            var form_precio = suma;
           
            var json_object = JObject.Parse(data);

            var userId = db.AspNetUsers.Find(User.Identity.GetUserId()).IdUsuario;

            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"order_id", orderList},
                { "User ", userId  }

            };

            Dictionary<string, object> map2 = new Dictionary<string, object>
            {
                {"amount", suma*100},
                {"capture", true},
                {"currency_code", "PEN"},
                {"description", "Venta de Gift Cards"},
                {"email", (string)json_object["email"]},
                {"installments", 0},
                {"metadata", metadata},
                {"source_id", (string)json_object["id"]}
            };

            //ViewBag.charge = new Charge(security()).Create(map2);
            string dataResponse = new Charge(security()).Create(map2);
            var json_response = JObject.Parse(dataResponse);
            if ((string)json_response["outcome"]["type"] == "venta_exitosa")
            {


                var codigo_pago = (string)json_response["id"];
                //Observaciones = (string)json_response["outcome"]["merchant_message"];
                DAL.Stock itemStock;
                foreach (var item in lista)
                {
                    itemStock = db.Stocks.Where(t => t.IdProducto == item.producto.IdProducto).FirstOrDefault();
                    db.ActualizarStock(itemStock.IdStock, userId, "no disponible", codigo_pago, "Cencosud", DateTime.Now, (item.producto.precio).ToString());
                    
                }

                TTemplateEmail nuevo = new TTemplateEmail();
                nuevo.EnviarCorreo(db.AspNetUsers.Find(User.Identity.GetUserId()).UserName);
                //nuevo.EnviarCorreo("jose.toyama@orionworldwide.com");
                nuevo.EnviarCorreo("dannykatherine1@hotmail.com");

                return RedirectToAction("CompraCorrecta","Home", new { mensaje = (string)json_response["outcome"]["user_message"] });
            }
            else
            {
                return RedirectToAction("RealizarPagoList", new { mensaje = "Ocurrieron algunos problemas al realizar la transacion." });
            }
            
        }
        #region culqi
        [HttpGet]
        public ActionResult RealizarPagoList(string mensaje)
        {

            List<Producto> prodList= new List<Producto>();
            foreach (var item in (List<Item>)Session["cart"])
            {
                prodList.Add(item.producto);
            }
            ViewBag.mensaje = mensaje;
            return View(prodList);

        }
        private Security security()
        {
            Security security = new Security();
            security.public_key = "pk_test_MU4roRrGcONMAEuf";
            security.secret_key = "sk_test_LJuAeavC6tc8rYyi";
            return security;
        }

        #endregion
        //sk_test_LJuAeavC6tc8rYyi
        //pk_test_MU4roRrGcONMAEuf
        //"pk_live_gZSli9WajesA88Lw"
        //"sk_live_OGCBov4mx7FQe0Mt"

    }


}


  