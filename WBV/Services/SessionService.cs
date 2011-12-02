using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBV.Interfaces;
using WBV.Models;
using log4net;
using log4net.Config;

namespace WBV.Services
{
    public class SessionService : ISession
    {
        public HttpContextBase _context { get; set; }
        private static readonly ILog log = LogManager.GetLogger("SessionService");
        public SessionService(HttpContextBase context)
        {
            _context = context;
   
        }

        public string userToken
        { 
           get {
               try
               {
                   var cookie = _context.Request.Cookies["userToken"];
                   if (cookie != null)
                       return cookie.Value.ToString();
                   else
                       return "";
               }
               catch(Exception exp) 
               {
                   log.Error(exp);
                   throw;
               }
                }
            set 
                {
                    try
                    {
                        HttpCookie cookie = new HttpCookie("userToken");
                        cookie.Value = value;
                        cookie.Expires = DateTime.Now.AddYears(1);
                        _context.Response.Cookies.Add(cookie);
                    }
                    catch(Exception exp) 
                    {
                        log.Error(exp);
                        throw;
                    }
                }
        
        }
        public User user
        {
            get
            {
                if (_context.Session != null)
                {
                    return _context.Session["user"] as User;
                }
                else
                {
                    return null;
                }
            }
            set { _context.Session["user"] = value; }
        }


    }
}