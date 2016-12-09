using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    [Authorize]
    public class FilteringController : Controller
    {
        private BeverageAFoghelEntities db = new BeverageAFoghelEntities();

        // GET: Filtering
        public ActionResult Index()
        {
            //FILTER PART
            //setup a variable to hold the Cars dataset
            DbSet<Beverage> BeveragesToSearch = db.Beverages;

            //setup some strings to hold the data that might be in the session 
            //If there is nothing in the session we can still use these variables 
            //as a default value
            string filterName = "";
            string filterPrice = "";
            string filterPack = "";

            int priceF = 0;

            //Check session if it has a value if so assign to string variables
            //the session holds objects so YOU MUST CAST IT TO A STRING
            if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }
            if (Session["pack"] != null && !String.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                filterPack = (string)Session["pack"];
            }
            if (Session["price"] != null && !String.IsNullOrWhiteSpace((string)Session["price"]))
            {
                filterPrice = (string)Session["price"];
                priceF = Int32.Parse(filterPrice);
            }

            //Do the filter on the CarsToSearch dataset. Use the where that we used before
            //when doing EF work, only this time send in more lamda expressions to narrow it 
            //down further. Since we setup default values for each of the filter parameters,
            //min,max and make, we can count on this alway running with no errors.
            IEnumerable<Beverage> filtered = BeveragesToSearch.Where(drink => drink.name.Contains(filterName) &&
                                                                  drink.pack.Contains(filterPack) &&
                                                                  drink.price <= priceF);
            //Place the string representation of the values in the session into the viewBag so that they can be retrieved and displayed
            ViewBag.filterMake = filterName;
            ViewBag.filterMin = filterPack;
            ViewBag.filterMax = filterPrice;

            //return the view with a filter selection of cars
            return View(filtered);
        }
        

        
        //Filter Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Get the form data that was sent out of the Request object
            //the string that is used as a key to get the data matches the name
            //propertry of the form control. (for us this is the first parameter)
            String name = Request.Form.Get("price");
            String pack = Request.Form.Get("pack");
            String price = Request.Form.Get("price");

            //Store the form data into the session so that it can be retrived later
            //on to filter the data
            Session["name"] = name;
            Session["pack"] = pack;
            Session["price"] = price;

            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
