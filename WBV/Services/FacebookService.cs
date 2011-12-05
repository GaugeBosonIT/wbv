using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using Facebook;

namespace WBV.Services
{
    public class FacebookService
    {


        public void StreamPublish(string accessToken, string sender, string link, string recipient)
        {
            var client = new FacebookClient(accessToken);
            dynamic parameters = new ExpandoObject();
            parameters.message = sender + " has sent you gift";
            parameters.link = link;
            parameters.picture = "http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg";
            parameters.name = "Gifts delivered by ziftly";
            parameters.caption = "Clink the link to claim your gift";
            parameters.description = "ziftly is a service that allows friends to send each other awesome gifts";
            parameters.actions = new
            {
                name = "View on Ziftly",
                link = link,
            };

            parameters.targeting = new
            {
                countries = "DE",
                regions = "6,53",
                locales = "6",
            };
            dynamic result = client.Post(recipient+"/feed", parameters);
        }
    }
}