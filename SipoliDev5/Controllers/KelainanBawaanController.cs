using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SipoliDev5.Models;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class KelainanBawaansController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: KelainanBawaans
        public ActionResult Index()
        {
            var kelainanBawaans = db.KelainanBawaan.Include(k => k.KelainanBawaan1).Include(k => k.Orang);
            return View(kelainanBawaans.ToList());
        }

        // GET: KelainanBawaans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan kelainanBawaan = db.KelainanBawaan.Find(id);
            if (kelainanBawaan == null)
            {
                return HttpNotFound();
            }
            return View(kelainanBawaan);
        }

        // GET: KelainanBawaans/Create
        public ActionResult Create()
        {
            ViewBag.KelainanBawaanID = new SelectList(db.KelainanBawaan, "ID", "Nama");
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama");
            return View();
        }

        // POST: KelainanBawaans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,KelainanBawaanID")] KelainanBawaan kelainanBawaan)
        {
            if (ModelState.IsValid)
            {
                db.KelainanBawaan.Add(kelainanBawaan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KelainanBawaanID = new SelectList(db.KelainanBawaan, "ID", "Nama", kelainanBawaan.KelainanBawaanID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", kelainanBawaan.PasienID);
            return View(kelainanBawaan);
        }

        // GET: KelainanBawaans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan kelainanBawaan = db.KelainanBawaan.Find(id);
            if (kelainanBawaan == null)
            {
                return HttpNotFound();
            }
            ViewBag.KelainanBawaanID = new SelectList(db.KelainanBawaan, "ID", "Nama", kelainanBawaan.KelainanBawaanID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", kelainanBawaan.PasienID);
            return View(kelainanBawaan);
        }

        // POST: KelainanBawaans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,KelainanBawaanID")] KelainanBawaan kelainanBawaan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kelainanBawaan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KelainanBawaanID = new SelectList(db.KelainanBawaan, "ID", "Nama", kelainanBawaan.KelainanBawaanID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", kelainanBawaan.PasienID);
            return View(kelainanBawaan);
        }

        // GET: KelainanBawaans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan kelainanBawaan = db.KelainanBawaan.Find(id);
            if (kelainanBawaan == null)
            {
                return HttpNotFound();
            }
            return View(kelainanBawaan);
        }

        // POST: KelainanBawaans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KelainanBawaan kelainanBawaan = db.KelainanBawaan.Find(id);
            db.KelainanBawaan.Remove(kelainanBawaan);
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
