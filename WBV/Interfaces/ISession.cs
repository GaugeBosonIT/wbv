using System;
using System.Web;
namespace WBV.Interfaces
{
    interface ISession
    {
        HttpContextBase Context { get; set; }
    }
}
