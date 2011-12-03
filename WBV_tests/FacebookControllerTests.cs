using System;
using NUnit.Framework;
using WBV.Controllers;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WBV.Interfaces;
using WBV.Models;
using WBV.DataAccess;
using WBV.Controllers.Helpers;


namespace WBV_tests
{
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public void user_returns_status_string()
        {
            //arrange
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockContext = new Mock<ISession>();
            var mockdata = new Mock<IData>();
            var mockFacebookUser = new Mock<IFacebookUser>();
            var c = new FacebookController(mockContext.Object, mockdata.Object);
            mockFacebookUser.Object.session.access_token = "somedummytoken";
            mockFacebookUser.Object.profile.name = "somedummy";
            mockFacebookUser.Object.profile.uid = "1";
            //act
            var v = c.User(mockFacebookUser.Object) as string;
            //assert
            Assert.IsNotNullOrEmpty(v);
        }

    }
}
