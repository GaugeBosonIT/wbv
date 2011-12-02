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
    public class HomeControllerTest
    {
         [Test]
        public void index_returns_view()
        {
             //arrange
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockContext = new Mock<ISession>();
            var mockdata = new Mock<IData>();
            var c = new HomeController(mockContext.Object, mockdata.Object);
            //act
            var v = c.Index() as ViewResult;
            //assert
            Assert.AreEqual(v.ViewName, "Index", "Index View name incorrect");
         }

         [Test]
         public void index_returns_token()
         {
             //arrange
             var mockHttpContext = new Mock<HttpContextBase>();
             var mockContext = new Mock<ISession>();
             var mockdata = new Mock<IData>();
             var c = new HomeController(mockContext.Object, mockdata.Object);
             //act
             var v = c.Index() as ViewResult;
             //assert
             Assert.IsNotNullOrEmpty(v.ViewData["AccessToken"].ToString());
         }


         [Test]
         public void return_access_token_from_user_in_session()
         {
             //arrange
             var mockContext = new Mock<ISession>();
             mockContext.SetupGet(m => m.user.access_token).Returns("somelovelyrealaccesstoken");
             var mockdata = new Mock<IData>();
             //act
             string accessToken = LoginStatus.accessToken(mockContext.Object, mockdata.Object);
             //assert
             Assert.AreNotEqual(accessToken, "NOTOKEN", "access_token not retrieved from session");
         }
    }
}
