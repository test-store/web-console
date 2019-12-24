using FinalSite.DAL;
using FinalSite.Helper;
using FinalSite.Infraestructure;
using FinalSite.Models;
using FinalSite.Models.Home;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalSite.Controllers
{
    //User.Identity.GetUserName()
    public class HomeController : Controller
    {
        DataBaseTestEntities db = new DataBaseTestEntities();
        public ActionResult Index()
        {
            return View();
        }



        #region culqi
        [HttpGet]
        public ActionResult RealizarPago(int IdProducto, string mensaje)
        {
            var producto = db.Productoes.Find(IdProducto);
            ViewBag.mensaje = mensaje;
            return View(producto);

        }
        [HttpPost]
        public ActionResult RealizarPagoList(Card c)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            if (cart != null)
            {

            }
            return View();
        }
        [HttpGet]
        public ActionResult RealizarPagoList(List<Producto> lista)
        {
            return View(lista);
        }

        [HttpPost]
        public ActionResult RealizarPago(Card card, int IdProducto)
        {
            var producto = db.Productoes.Find(IdProducto);

            Dictionary<string, object> map = new Dictionary<string, object>
                  {
                    {"card_number", card.cardNumber},
                    {"cvv", card.cvv},
                    {"expiration_month", card.expiration_month},
                    {"expiration_year", card.expiration_year},
                    {"email", card.email}
                 };
            String data = new Token(security()).Create(map);
            var json_object = JObject.Parse(data);

            var userId = db.AspNetUsers.Find(User.Identity.GetUserId()).IdUsuario;

            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"order_id", Convert.ToString(producto.IdProducto)},
                { "User ", userId  }

            };

            Dictionary<string, object> map2 = new Dictionary<string, object>
            {
                {"amount", (int)(producto.precio??0)*100},
                {"capture", true},
                {"currency_code", "PEN"},
                {"description", "Venta de Gift Cards"},
                {"email", card.email},
                {"installments", 0},
                {"metadata", metadata},
                {"source_id", (string)json_object["id"]}
            };

            //ViewBag.charge = new Charge(security()).Create(map2);
            string dataResponse = new Charge(security()).Create(map2);
            var json_response = JObject.Parse(dataResponse);
            if ((string)json_response["outcome"]["type"] == "venta_exitosa")
            {


                Pago oPago = db.Pagoes.Add(new Pago()
                {
                    descripción_pago = (string)json_response["id"],
                    Observaciones = (string)json_response["outcome"]["merchant_message"]
                });

                var oStock = db.Stocks.Where(t => t.IdProducto == IdProducto).FirstOrDefault();
                db.ActualizarStock(oStock.IdStock, userId, oPago.IdPago, "no disponible", "", DateTime.Now, Convert.ToString(producto.precio ?? 0));

                TTemplateEmail nuevo = new TTemplateEmail();
                nuevo.EnviarCorreo(db.AspNetUsers.Find(User.Identity.GetUserId()).UserName);
                nuevo.EnviarCorreo("jose.toyama@orionworldwide.com");
                nuevo.EnviarCorreo("dannykatherine1@hotmail.com");

                return RedirectToAction("CompraCorrecta", new { mensaje = (string)json_response["outcome"]["user_message"] });
            }
            else
            {
                return RedirectToAction("RealizarPago", new { IdProducto = IdProducto, mensaje = "Ocurrieron algunos probemas al realizar la transacion." });
            }
        }
        public ActionResult CompraCorrecta(string mensaje)
        {
            return View();
        }
        private Security security()
        {
            Security security = new Security();
            security.public_key = "pk_test_MU4roRrGcONMAEuf";
            security.secret_key = "sk_test_LJuAeavC6tc8rYyi";
            return security;
        }

        #endregion
    }
}