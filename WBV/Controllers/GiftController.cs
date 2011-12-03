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
        public string Send(GiftRaw giftRaw)
        {
            try
            {
                var sender = new User();
                sender = _session.user;
                var recipient = new User();
                recipient.facebook_id = giftRaw.recipient.profile.id;
                recipient.name = giftRaw.recipient.profile.name;
                var gift = new Gift();
                gift.product_id = giftRaw.product.product_id;
                gift.User[0] = sender;
                gift.User[1] = recipient;
                var o = new orm(_data);
                var returned_user = o.SetObject(gift).o as Gift;
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
