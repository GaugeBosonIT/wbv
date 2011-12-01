using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBV.Interfaces;
using WBV.Models;

namespace WBV.Services
{
    public class SessionService : ISession
    {
        public HttpContextBase Context { get; set; }

        public SessionService(HttpContextBase context)
        {
            this.Context = context;
        }

        public string userToken
        { 
           get {

               var cookie = Context.Request.Cookies["userToken"];
               if (cookie != null)
                   return cookie.Value.ToString();
               else
                   return "";
           
                }
            set 
                { 
                HttpCookie cookie = new HttpCookie("userToken");
                cookie.Value = value;
                cookie.Expires = DateTime.Now.AddYears(1);
                Context.Response.Cookies.Add(cookie);
                }
        
        }
        public User user
        {
            get { return (User)Context.Session["user"]; }
            set { Context.Session["user"] = value; }
        }


    }
}