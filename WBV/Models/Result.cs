using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Web.Mvc;
using WBV.Interfaces;


namespace WBV.Models
{
    [Serializable]
    public class Result
    {

        [XmlAttribute("proc_name")]
        public string proc_name { get; set; }
        [XmlAttribute("status")]
        public int status { get; set; }
        [XmlElement("User")]
        public User user{get; set; }
    }
    

    [Serializable]
    public class User
    {
        [XmlAttribute]
        public virtual string access_token { get; set; }
        [XmlAttribute]
        public string user_token { get; set; }
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string facebookId { get; set; }
        [XmlAttribute]
        public string email { get; set; }
    }

}