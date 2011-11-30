﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBV.Models;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.IO;
using System.Xml;
using System.Text;
using WBV.DataAccess;

namespace WBV.DataAccess
{
    public class orm
    {


        private IDataConnector _dataConnector;

        private orm(IDataConnector dc) 
        {
            _dataConnector = dc;
        }
        
        //set object return bool. get take object with only param attribute and returns full object
        public  Object GetObject(Object o)
        {
            Object r;
            r = SerialiseR(GetXML(o));
            return r;
        }

        public  ResultObject SetObject(Object o)
        {
            ResultObject ro = new ResultObject();
            ro.o = SerialiseR(SetXML(o));
            ro.r = true;
            return ro;
        }

        private  XmlDocument SetXML(Object o)
        {
            
            XmlDocument r = new XmlDocument();
            r = _dataConnector.execStoredProc("set_" + o.GetType().Name, DeserialiseP(o));
            return r;
        }

        private  XmlDocument GetXML(Object o)
        {
           XmlDocument r = new XmlDocument();
            string proc = "get_" + o.GetType().Name;
            r = _dataConnector.execStoredProc(proc, DeserialiseP(o));
            return r;
        }

        private static XmlDocument DeserialiseP(Object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream myStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlWriter xmlWriter = XmlWriter.Create(myStream, settings);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            mySerializer.Serialize(xmlWriter, o, ns);
            string xml = Encoding.UTF8.GetString(myStream.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            XmlDocument p = new XmlDocument();
            p.LoadXml(xml);
            return p;
        }
        private static Object SerialiseR(XmlDocument r)
        {

            Object o;
            XmlDocument innerobject = new XmlDocument();
            innerobject.LoadXml(r.FirstChild.InnerXml);
            string oname = "Schapp.Models." + innerobject.DocumentElement.Name;
            Type t = Type.GetType(oname);
            XmlSerializer mySerializer = new XmlSerializer(t);
            MemoryStream myStream = new MemoryStream();
            innerobject.Save(myStream);
            myStream.Position = 0;
            o = mySerializer.Deserialize(myStream);
            return o;

        }

        public struct ResultObject
        {
            public Object o { get; set; }
            public bool r { get; set; }

        }


    }
}