using FinalSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSite.Models
{
    public class TemplateEmail
    {
        public string userName { get; set; }
        public Producto producto { get; set; }
        public string content_message { get; set; }


        public TemplateEmail() { 
        
        }
        public TemplateEmail(string userName, Producto producto, string content_message)
        {
            this.userName = userName;
            this.producto = producto;
            this.content_message = content_message;
        }
    }
}