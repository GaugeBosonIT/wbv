using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using WBV.Interfaces;

namespace WBV.Models.Facebook
{
    [DataContract]
    public class FacebookUser : IFacebookUser
    {

        [DataMember]
        public Session session{ get; set; }

        [DataMember]
        public Profile profile { get; set; }
    }

    
    public class Session
    {
        [DataMember]
        public string session_key { get; set; }
        [DataMember]
        public string secret { get; set; }
        [DataMember]
        public string expires { get; set; }
        [DataMember]
        public string base_domain { get; set; }
        [DataMember]
        public string access_token { get; set; }
     }



    public class Profile
    {
        [DataMember]
        public string uid { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string gender { get; set; }
        [DataMember]
        public string timezone { get; set; }
        [DataMember]
        public string locale { get; set; }
        [DataMember]
        public string verified { get; set; }
        [DataMember]
        public string updated_time { get; set; }
        [DataMember]
        public string email { get; set; }

    }

}
