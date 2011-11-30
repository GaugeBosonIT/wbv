using System;
using System.Xml;
namespace WBV.Interfaces
{
    interface IData
    {
         XmlDocument execStoredProc(string strProcName, XmlDocument strParameters);
    }
}
