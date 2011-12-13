using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBV.Interfaces;
using WBV.Models;
using NLog;

namespace WBV.Services
{
    public class SessionService : ISession
    {
        public HttpContextBase _context { get; set; }
      
        private static readonly Logger log = LogManager.GetLogger("SessionService");

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
                try
                {
                    if (_context.Session != null)
                    {
                        return _context.Session.GetDataFromSession<User>("user"); ;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception exp)
                { 
                    log.Error(exp.ToString());
                    throw;
                }
            }
            set {
                try
                {
                    _context.Session.SetDataInSession("user", value);
    
                }
                catch (Exception exp)
                {
                    log.Error(exp.ToString());
                }
            
                }
        }

   }
    public static class SessionExtensions
    {
        public static T GetDataFromSession<T>(this HttpSessionStateBase session, string key)
        {
            return (T)session[key];
        }

        public static void SetDataInSession(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = value;
        }
    }
}