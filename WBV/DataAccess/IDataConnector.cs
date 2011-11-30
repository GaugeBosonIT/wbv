using System;
using System.Xml;
namespace WBV.DataAccess
{
    interface IDataConnector
    {
         XmlDocument execStoredProc(string strProcName, XmlDocument strParameters);
    }
}
