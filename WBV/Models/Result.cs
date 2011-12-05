using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Web.Mvc;
using WBV.Interfaces;
using System.ServiceModel;


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
    



}