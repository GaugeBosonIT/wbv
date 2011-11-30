using System;
namespace WBV.Interfaces
{
    interface IUser
    {

        int id { get; set; }
        string facebookId { get; set; }
        string name { get; set; }
        string accessToken { get; set; }
    }
}
