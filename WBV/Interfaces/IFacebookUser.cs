using System;
namespace WBV.Interfaces
{
   public interface IFacebookUser
    {
        WBV.Models.Facebook.Profile profile { get; set; }
        WBV.Models.Facebook.Session session { get; set; }
    }
}
