﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBV.Interfaces;
using NLog;
using WBV.Models;
using WBV.DataMapper;

namespace WBV.Controllers.Helpers
{
    public static class LoginStatus
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        public static string accessToken(ISession _session, IData _data)
        {
            string _accessToken;
            try
            {
               

                if (_session.user == null)
                {
                    if (_session.userToken == "" || _session.userToken == null)
                    {
                        //this bloke needs to login before we are going to set any cookie.
                       _accessToken = "NOTOKEN";
                        log.Info("Some user with no token");
                    }
                    else
                    {
                        //get the  user into the session from their cookie
                        var user = new User();
                        user.user_token = _session.userToken;
                        var o = new orm(_data);
                        _session.user = (User)o.GetObject(user);
                        _accessToken = _session.user.access_token;
                    }
                }
                else
                {
                    _accessToken = _session.user.access_token;
                }
            }
            catch (Exception exp)
            {
                log.Error(exp);
                throw;
            }
            return _accessToken;
        }
    }
}