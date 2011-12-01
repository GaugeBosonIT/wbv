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
        public HttpContextBase Context { get; set; }
        private static readonly ILog log = LogManager.GetLogger("SessionService");
        public SessionService(HttpContextBase context)
        {
            this.Context = context;
        }

        public string userToken
        { 
           get {
               try
               {
                   var cookie = Context.Request.Cookies["userToken"];
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
                        Context.Response.Cookies.Add(cookie);
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
            get { return (User)Context.Session["user"]; }
            set { Context.Session["user"] = value; }
        }


    }
}