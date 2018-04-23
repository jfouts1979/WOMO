using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WineOnMyOwn.Models;

namespace WineOnMyOwn.Controllers
{
    public class TTBWinePermitsController : Controller
    {
        private WOMOEntities db = new WOMOEntities();

        // GET: TTBWinePermits
        public ActionResult Index()
        {
            //return View(db.TTBWinePermits.Take(3));
            return View(db.TTBWinePermits);
        }




        // GET: TTBWinePermits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTBWinePermit tTBWinePermit = db.TTBWinePermits.Find(id);
            if (tTBWinePermit == null)
            {
                return HttpNotFound();
            }
            return View(tTBWinePermit);
        }

        // GET: TTBWinePermits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TTBWinePermits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PERMIT_NUMBER,OWNER_NAME,OPERATING_NAME,STREET,CITY,STATE,ZIP,ZIP4,COUNTY,WINEPERMITID")] TTBWinePermit tTBWinePermit)
        {
            if (ModelState.IsValid)
            {
                db.TTBWinePermits.Add(tTBWinePermit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tTBWinePermit);
        }

        // GET: TTBWinePermits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTBWinePermit tTBWinePermit = db.TTBWinePermits.Find(id);
            if (tTBWinePermit == null)
            {
                return HttpNotFound();
            }
            return View(tTBWinePermit);
        }

        // POST: TTBWinePermits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PERMIT_NUMBER,OWNER_NAME,OPERATING_NAME,STREET,CITY,STATE,ZIP,ZIP4,COUNTY,WINEPERMITID")] TTBWinePermit tTBWinePermit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tTBWinePermit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tTBWinePermit);
        }

        // GET: TTBWinePermits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTBWinePermit tTBWinePermit = db.TTBWinePermits.Find(id);
            if (tTBWinePermit == null)
            {
                return HttpNotFound();
            }
            return View(tTBWinePermit);
        }

        // POST: TTBWinePermits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TTBWinePermit tTBWinePermit = db.TTBWinePermits.Find(id);
            db.TTBWinePermits.Remove(tTBWinePermit);
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
