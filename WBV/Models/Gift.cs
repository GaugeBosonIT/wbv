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
        [XmlAttribute]
        public virtual string token { get; set; }
        [XmlElement]
        public User[] User { get; set; }
     }

    [DataContract]
    public class GiftRaw
    {
        
        [DataMember]
        public GiftWrapper gift {get; set;}

    }
    [DataContract]
    public class GiftWrapper
    {
        [DataMember]
        public Product product { get; set; }
        [DataMember]
        public Profile recipient { get; set; }
    
    }

    public class Product
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string name { get; set; }
    }

}