using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBV.Interfaces;

namespace WBV.Controllers
{
    public class HomeController : Controller
    {

        private ISession _session;

        public HomeController(ISession session)
        {
            _session = session;
        }       
        public ActionResult Index()
        {
            if (_session.Context.Session["user"] == null)
            {
                var userToken = _session.Context.Request.Cookies["userToken"] ;
                if (userToken == null)
                {
                    //this bloke needs to login before we are going to set any cookie.

                }
                else
                { 
                    //get the  user into the session from their cookie

                }




            }
            
            ViewBag.AccessToken = "NOTOKEN";
            return View("Index");
        }

      
    }
}
