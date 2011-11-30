using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace WBV.Models
{
    [Serializable]
    public class Result
    {

        [XmlAttribute("proc_name")]
        public string proc_name { get; set; }

        [XmlAttribute("status")]
        public int status { get; set; }

    }
}