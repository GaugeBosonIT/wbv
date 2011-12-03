using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Web;
using WBV.Models;
using WBV.Services;
using WBV.Interfaces;
using log4net;
using log4net.Config;
using WBV.DataMapper;


namespace WBV.Controllers
{
    public class GiftController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger("GiftController");
        public IData _data;
        public ISession _session;
        public GiftController(ISession session, IData data)
        {
            _data = data;
            _session = session;

        }
        
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, UriTemplate = "")]
        public string Send(GiftWrapper gift)
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
                myGift.product_id = gift.product.id;
                myGift.User = new User[2];
                myGift.User[0] = sender;
                myGift.User[1] = recipient;
                var o = new orm(_data);
                var returned_user = o.SetObject(myGift).o as Gift;
                return "\"Result\":{\"status\":0";
            }
            catch (Exception exp)
            {
                log.Error(exp.ToString());
                throw;
            }

       }

      
    }
}
