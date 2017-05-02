using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using db_saber.Model;
using db_saber.BLL;

namespace Web.Controllers
{
    public class IndexController : Controller
    {
        // 
        public ActionResult Index(int? page)
        {
            return View();
        }
       
    }
}
