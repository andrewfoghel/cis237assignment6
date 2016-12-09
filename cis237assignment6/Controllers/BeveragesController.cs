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
    public class BeveragesController : Controller
    {
        private BeverageAFoghelEntities db = new BeverageAFoghelEntities();

        //Remake Index so that it has a sorting option bar, and when clicked will sort, first by ascending followed by descending 
        //Must also create hyperlinks for given sorting techniques 
        // GET: Beverages
        public ActionResult Index(string sortOrder, string searchStringName, string searchStringPack, string searchStringPrice)
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
            if (!String.IsNullOrEmpty(searchStringName))
            {
                drinks = drinks.Where(s => s.name.ToUpper().Contains(searchStringName.ToUpper()));
                                    
            }
            if (!String.IsNullOrEmpty(searchStringPack))
            {
                drinks = drinks.Where(s => s.pack.ToUpper().Contains(searchStringPack.ToUpper()));
            }
            if (!String.IsNullOrEmpty(searchStringPack))
            {
                drinks = drinks.Where(s => s.price <= int.Parse(searchStringPrice));
            }




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
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
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
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
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

            //Store the form data into the session so that it can be retrived later
            //on to filter the data
            Session["name"] = name;
            Session["pack"] = pack;
            Session["price"] = price;



            //Redirect the user to the index page. We will do the work of actually filtering the list in the index method
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
