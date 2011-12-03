using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WBV.Models
{
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
        public string facebook_id { get; set; }
        [XmlAttribute]
        public string email { get; set; }
    }
}