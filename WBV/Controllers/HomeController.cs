﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBV.Interfaces;
using WBV.DataMapper;
using WBV.Models;
using NLog;
using WBV.Controllers.Helpers;


namespace WBV.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger log = LogManager.GetLogger("HomeController");
        public ISession _session;
        public IData _data;

        public HomeController(ISession session, IData data)
        {
            _session = session;  
            _data = data;
        }
        public ActionResult Index()
        {
            try
            {
                ViewBag.GiftJson = TempData["GiftJson"];
                ViewBag.AccessToken = LoginStatus.accessToken(_session, _data);
            }
            catch (Exception exp)
            { 
                log.Error(exp);   
                throw;  
            }
            return View("Index");
        }
    }
}
