using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WBV.Models.Facebook;

namespace WBV.Models
{
   
    [Serializable]
    public class Gift
    {
        
        [XmlAttribute]
        public virtual string product_id { get; set; }
        [XmlElement]
        public User[] User { get; set; }
     }

    [DataContract]
    public class GiftRaw
    {
        [DataMember]
        public Product product { get; set; }
        [DataMember]
        public FacebookUser profile { get; set; }
    }

    public class Product
    {
        [DataMember]
        public string product_id { get; set; }
    }

}