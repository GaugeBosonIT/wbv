using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBV.Interfaces;

namespace WBV.Services
{
    public class SessionService : ISession
    {
        public HttpContext Context { get; set; }

        public SessionService(HttpContext context)
        {
            this.Context = context;
        }


    }
}