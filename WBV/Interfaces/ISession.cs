using System;
using System.Web;
using WBV.Models;
namespace WBV.Interfaces
{
    public interface ISession
    {
        HttpContext Context { get; set; }

        string userToken { get; set; }

        User user { get; set; }

    }
}
