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
    public class GolonganObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: /GolonganObat/
        public ActionResult Index()
        {
            return PartialView(db.GolonganObat.ToList());
        }

        // GET: /GolonganObat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolonganObat golonganobat = db.GolonganObat.Find(id);
            if (golonganobat == null)
            {
                return HttpNotFound();
            }
            return View(golonganobat);
        }

        // GET: /GolonganObat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /GolonganObat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Nama")] GolonganObat golonganobat)
        {
            if (ModelState.IsValid)
            {
                db.GolonganObat.Add(golonganobat);
                db.SaveChanges();
                return RedirectToAction("Index", "Obat");
            }

            return View(golonganobat);
        }

        // GET: /GolonganObat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolonganObat golonganobat = db.GolonganObat.Find(id);
            if (golonganobat == null)
            {
                return HttpNotFound();
            }
            return View(golonganobat);
        }

        // POST: /GolonganObat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Nama")] GolonganObat golonganobat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(golonganobat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Obat");
            }
            return View(golonganobat);
        }

        // GET: /GolonganObat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolonganObat golonganobat = db.GolonganObat.Find(id);
            if (golonganobat == null)
            {
                return HttpNotFound();
            }
            return View(golonganobat);
        }

        // POST: /GolonganObat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GolonganObat golonganobat = db.GolonganObat.Find(id);
            db.GolonganObat.Remove(golonganobat);
            db.SaveChanges();
            return RedirectToAction("Index", "Obat");
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
