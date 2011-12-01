using System;
using System.Xml;
namespace WBV.Interfaces
{
    public interface IData
    {
         XmlDocument execStoredProc(string strProcName, XmlDocument strParameters);
    }
}
