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
    public class BeveragesController : Controller
    {
        private BeverageAFoghelEntities db = new BeverageAFoghelEntities();

        //Remake Index so that it has a sorting option bar, and when clicked will sort, first by ascending followed by descending 
            //Must also create hyperlinks for given sorting techniques 
        // GET: Beverages
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NoSortP = String.IsNullOrEmpty(sortOrder) ? "Original" : "";
            ViewBag.SortByNameP = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.SortByPackP = sortOrder == "Pack" ? "pack_desc" : "Pack";
            ViewBag.SortByPriceP = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.SortByActiveP = sortOrder == "Active" ? "active_desc" : "Active";

            //var to hold list in to sort but not change database
            var drinks = from x in db.Beverages
                         select x;

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
