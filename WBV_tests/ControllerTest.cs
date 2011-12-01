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


namespace WBV_tests
{
    [TestFixture]
    public class ControllerTest
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
    }
}
