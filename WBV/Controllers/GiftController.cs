﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Web;
using WBV.Models;
using WBV.Services;
using WBV.Interfaces;
using NLog; 
using WBV.DataMapper;
using Newtonsoft.Json;
using System.ServiceModel;


namespace WBV.Controllers
{

    public class GiftController : Controller
    {
        private static readonly Logger log = LogManager.GetLogger("GiftController");
        public IData _data;
        public ISession _session;
        public GiftController(ISession session, IData data)
        {
            _data = data;
            _session = session;

        }
       
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "")]
        public JsonResult Send(GiftWrapper gift)
        {
            try
            {
                var sender = new User();
                sender = _session.user;
                sender.role = "SENDER";
                var recipient = new User();
                recipient.facebook_id = gift.recipient.uid;
                recipient.role = "RECIPIENT";
                recipient.name = gift.recipient.name;
                var myGift = new Gift();
                myGift.product_id = "1";
                myGift.User = new User[2];
                myGift.User[0] = sender;
                myGift.User[1] = recipient;
                var o = new orm(_data);
                var returned_gift = o.SetObject(myGift).o as Gift;
                var fb = new FacebookService();
                var port  = (_session._context.Request.Url.Port == 80) ? "" : ":"+_session._context.Request.Url.Port.ToString();
                var hostname = _session._context.Request.Url.Host + port;
                var link = "http://" + hostname + "/gift/claim/?id=" + returned_gift.token;
                fb.StreamPublish(_session.user.access_token, _session.user.name, link, recipient.facebook_id);
                var result = new Result();
                result.status = 0;
                return Json(result);
            }
            catch (Exception exp)
            {
                log.Error(exp.ToString());
                throw;
            }

       }
        [HttpGet]
        public ActionResult Claim(string id)
        {
            var gift = new Gift();
            gift.token = id;
            var o = new orm(_data);
            var return_gift = o.GetObject(gift) as Gift;
            var product = new Product();
            product.id = return_gift.product_id;
            product.name = return_gift.name;
            product.picture = return_gift.picture;
            product.token = return_gift.token;
            var dict = new Dictionary<string, object>();
            dict.Add("product", product);
            dict.Add("sender", return_gift.User[0]);
            dict.Add("recipient", return_gift.User[1]);

            TempData["GiftJson"] = JsonConvert.SerializeObject(dict);
            return RedirectToRoute("Home");
            
        }


       [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "")]
        public JsonResult Redeem(Gift gift)
        {
            var o = new orm(_data);
            var return_gift = o.GetObject(gift) as Gift;
            var ret_gift = new Gift();
            ret_gift.redeem_token = return_gift.redeem_token;
            var dict = new Dictionary<string, object>();
            dict.Add("gift", ret_gift);
            SendThankYouMail(return_gift);
            return Json(dict);

        }

       public void SendThankYouMail(Gift gift)
       {
           
           var subject = gift.User[1].name  + " says thanks for the gift";
           var body = "Dear " + gift.User[0].name + " " + gift.User[1].name + " has now redeemed your gift and is very happy. Best wishes, the Ziftly team";
           var toAddress = gift.User[0].email;

           var mail = new Mail(subject,body, toAddress);
           mail.SendMail();
       }

      
    }
}
