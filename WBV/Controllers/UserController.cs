using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using WBV.Models;
using WBV.Interfaces;
using WBV.DataMapper;

namespace WBV.Controllers
{
    public class UserController : Controller
    {
        public IData _data; 
        public UserController(IData data)
        {
            _data = data;

        }
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, UriTemplate = "")]
        public ActionResult Index(User user)
        {
            var o = new orm(_data);
            return o.SetObject(user).o as ActionResult;
      
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();

        }

      
    }
}
