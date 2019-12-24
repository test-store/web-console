using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalSite.DAL;

namespace FinalSite.Models.Home
{
    public class Item
    {
        
        public Item() { 
        }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
    }
}