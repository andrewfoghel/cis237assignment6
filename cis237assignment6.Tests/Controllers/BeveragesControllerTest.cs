using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cis237assignment6;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Controllers;
using cis237assignment6.Models;




namespace cis237assignment6.Tests.Controllers
{
    [TestClass]
    public class BeveragesControllerTest
    {
        //Testing the view returned by the controller
        [TestMethod]
        public void Test_Index()
        {
            //Arrange 
            BeveragesController x = new BeveragesController();

            //Act
            ViewResult result = x.Index(null) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit()
        {
            BeveragesController ee = new BeveragesController();
            Beverage e = new Beverage();
            var drinkEdit = ee.Edit(e) as ViewResult;
     //       Beverage tester = (Beverage)drinkEdit.ViewData.Model;
            Assert.AreEqual("Edit",drinkEdit.ViewName);
            
        }

    }
}
//Add a new basic unit test to the Test project
//add refereneces: system.web, system.web.mvc, cis237assigment6.Controller/Model
