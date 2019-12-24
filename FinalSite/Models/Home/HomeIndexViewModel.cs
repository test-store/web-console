using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalSite.Models;
using FinalSite.DAL;
using FinalSite.Repository;
using System.Data.SqlClient;
using PagedList;
using System.Web.Mvc;
using PagedList.Mvc;
using FinalSite.Models.Home;

namespace FinalSite.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        DataBaseTestEntities context = new DataBaseTestEntities();
        public IPagedList<Producto> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",search??(object)DBNull.Value)
            };
            IPagedList<Producto> data = context.Database.SqlQuery<Producto>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {
                ListOfProducts = data
            };
        }
    }
}

        