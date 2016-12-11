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
        [TestMethod]
        public void Test_Index()
        {
            BeveragesController Drinks = new BeveragesController();
            Beverage drink = new Beverage();
            var drinkTest = Drinks.Index(drink.id) as ViewResult;
            Beverage tester = (Beverage)drinkTest.ViewData.Model;
            Assert.AreEqual(tester.id, drink.id);
        }

        [TestMethod]
        public void Edit()
        {
            BeveragesController ee = new BeveragesController();
            Beverage e = new Beverage();
            var drinkEdit = ee.Edit(e) as ViewResult;
            Beverage tester = (Beverage)drinkEdit.ViewData.Model;
            Assert.AreEqual(tester.name, e.name);
        }

    }
}
//Add a new basic unit test to the Test project
//add refereneces: system.web, system.web.mvc, cis237assigment6.Controller/Model
