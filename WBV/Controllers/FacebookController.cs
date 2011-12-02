﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using WBV.Models;
using WBV.Models.Facebook;
using WBV.Interfaces;
using WBV.DataMapper;
using log4net;
using log4net.Config;

namespace WBV.Controllers
{
    public class FacebookController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger("UserController");
        public IData _data;
        public ISession _session; 
        public FacebookController(ISession session, IData data )
        {
            _data = data;
            _session = session;

        }
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, UriTemplate = "")]
        public string User(FacebookUser facebookuser)
        {
            
            try
            {
                var user = new User();
                user.access_token = facebookuser.session.access_token;
                user.name = facebookuser.profile.name;
                user.facebookId = facebookuser.profile.id;
                var o = new orm(_data);
                var returned_user = o.SetObject(user).o as User;
                _session.user = returned_user;
                _session.userToken = returned_user.user_token;

                return "\"Result\":{\"status\":0";
            }
            catch (Exception exp)
            {
                log.Error(exp);
                throw;
            }
        }
       }
}