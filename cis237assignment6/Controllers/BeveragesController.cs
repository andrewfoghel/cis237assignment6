using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;
using System.ComponentModel.DataAnnotations;

namespace cis237assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageAFoghelEntities db = new BeverageAFoghelEntities();

        //Remake Index so that it has a sorting option bar, and when clicked will sort, first by ascending followed by descending 
        //Must also create hyperlinks for given sorting techniques 
        // GET: Beverages
        public ActionResult Index(string sortOrder)
        {

            //SortMethod view bags
            ViewBag.NoSortP = String.IsNullOrEmpty(sortOrder) ? "Original" : "";
            ViewBag.SortByNameP = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.SortByPackP = sortOrder == "Pack" ? "pack_desc" : "Pack";
            ViewBag.SortByPriceP = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.SortByActiveP = sortOrder == "Active" ? "active_desc" : "Active";

            //var to hold list in to sort but not change database
            var drinks = from x in db.Beverages
                         select x;


            string filterName = "";
            string filterPack = "";
            string filterPrice = "";

            decimal cost = 3000;
            //create separarte decimal fields then do parsing in here
            if(Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
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
                decimal number;
                if(Decimal.TryParse(filterPrice,out number)){
                    cost = decimal.Parse(filterPrice);
                }
                else
                {
                    ViewBag.errorMessage = "Invalid Parameter";
                }
                
            }


            drinks = drinks.Where(drink => drink.name.Contains(filterName) &&
                                                                  drink.pack.Contains(filterPack) &&
                                                                 drink.price <= cost);


            //switch statement using sortOrder to check for nulls, and decide which sort does what 
            switch (sortOrder)
            {
                case "Name":
                    drinks = drinks.OrderBy(x => x.name);
                    break;
                case "name_desc":
                    drinks = drinks.OrderByDescending(x => x.name);
                    break;
                case "Pack":
                    drinks = drinks.OrderBy(x => x.pack);
                    break;
                case "pack_desc":
                    drinks = drinks.OrderByDescending(x => x.pack);
                    break;
                case "Price":
                    drinks = drinks.OrderBy(x => x.price);
                    break;
                case "price_desc":
                    drinks = drinks.OrderByDescending(x => x.price);
                    break;
                case "Active":
                    drinks = drinks.OrderBy(x => x.active);
                    break;
                case "active_desc":
                    drinks = drinks.OrderByDescending(x => x.active);
                    break;
                case "Original":
                    drinks = db.Beverages;
                    break;
                default:
                    break;
            }
            //return newly loaded array, now to go set hyperlinks in the BeveragesController index view
            return View(drinks.ToList());
        }

        //HOW TO DO VALIDATION FOR FILTER SINCE IT'S IN THE INDEX PART RATHER THAN THE FILTER PART, WHICH IS NOT EVEN BEING USED AT THE MOMENT. 
        //MOREOVER HOW WOULD I GO ABOUT USING THAT INSTEAD OF THE FILTER?

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
   
            //Server Side Validation check
            //Imparitive logic check using if else statement
                //Note:The problem is that this logic is repeated requiring updates in many places if the logic changes, which it inevitably will
                //But the .NET framework provides a more declaritive approach called DataAnnotationAttributes, which applies ASP.NET MVC with all the
                //nessacary metadata it needs to validate the model in one central place, But I wasn't able to figure it out, Finally got it working with anotation models
      /*      if (string.IsNullOrWhiteSpace(beverage.id))
            {
                //Invalid
                //Keep track of failures and then tell MVC about them adding them to the model state dictionary
                    //Note: you can then refer to this dictionary later to see if any of these errors were registered by using the ModelState.IsValid function
                    //to return the user back to said view so that they can fix their mistakes.
                ModelState.AddModelError("id", "Name is required");
            } else if (beverage.id.Length < 3 || beverage.id.Length > 10)
            {
                //Also Invalid
                ModelState.AddModelError("id", "Id must between 3 and 10 characters");
            }

            if (string.IsNullOrWhiteSpace(beverage.name))
            {
                ModelState.AddModelError("name","Name is requried");
            }

            if (string.IsNullOrWhiteSpace(beverage.pack))
            {
                ModelState.AddModelError("pack", "pack is requried");
            }

            if (string.IsNullOrWhiteSpace(beverage.price.ToString()))
            {
                ModelState.AddModelError("price", "price is requried");
            }
            */
            //Now we can refer to the errors in the view using the HTML.ValidationSummary
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //If not valid then returns you to the original view for editing
            
            return Create();
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {

            if (string.IsNullOrWhiteSpace(beverage.name))
            {
                ModelState.AddModelError("name", "Name is requried");
            }

            if (string.IsNullOrWhiteSpace(beverage.pack))
            {
                ModelState.AddModelError("pack", "pack is requried");
            }

            if (string.IsNullOrWhiteSpace(beverage.price.ToString()))
            {
                ModelState.AddModelError("price", "price is requried");
            }


            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Edit(beverage.id);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Filter Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Get the form data that was sent out of the Request object
            //the string that is used as a key to get the data matches the name
            //propertry of the form control. (for us this is the first parameter)
        String name = Request.Form.Get("name");
        String pack = Request.Form.Get("pack");
        String price = Request.Form.Get("price");
            
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

//Filter validation not working... Having trouble using data annotations, in videos I've watched they have a class in the models folder that 
//contains all fields and getter and setter methods, I can't find that here