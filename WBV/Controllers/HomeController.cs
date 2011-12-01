using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBV.Interfaces;
using WBV.DataMapper;
using WBV.Models;

namespace WBV.Controllers
{
    public class HomeController : Controller
    {
        
        public ISession _session;
        public IData _data;
        public HomeController(ISession session, IData data)
        {
            _session = session;
            _data = data;
        }
        public ActionResult Index()
        {

            ViewBag.AccessToken = accessToken();

            return View("Index");
        }

        public string accessToken()
        {
            string accessToken;
            if (_session.user == null)
            {
                if (_session.userToken == "")
                {
                    //this bloke needs to login before we are going to set any cookie.
                    accessToken = "NOTOKEN";
                }
                else
                {
                    //get the  user into the session from their cookie
                    var user = new User();
                    user.user_token = _session.userToken;
                    var o = new orm(_data);
                    _session.user = (User)o.GetObject(user);
                    accessToken = _session.user.access_token;

                }

            }
            else
            {
                accessToken = _session.userToken;
            }

            return accessToken;
        }

    }
}
