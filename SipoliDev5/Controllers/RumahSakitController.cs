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
    public class RumahSakitController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: RumahSakit
        public ActionResult Index()
        {
            var rumahSakit = db.RumahSakit.Include(r => r.Kecamatan);
            return View(rumahSakit.ToList());
        }

        // GET: RumahSakit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RumahSakit rumahSakit = db.RumahSakit.Find(id);
            if (rumahSakit == null)
            {
                return HttpNotFound();
            }
            return View(rumahSakit);
        }

        // GET: RumahSakit/Create
        public ActionResult Create()
        {
            ViewBag.KecamatanID = new SelectList(db.Kecamatan, "ID", "Nama");
            return View();
        }

        // POST: RumahSakit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nama,KecamatanID,NoTelepon")] RumahSakit rumahSakit)
        {
            if (ModelState.IsValid)
            {
                db.RumahSakit.Add(rumahSakit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KecamatanID = new SelectList(db.Kecamatan, "ID", "Nama", rumahSakit.KecamatanID);
            return View(rumahSakit);
        }

        // GET: RumahSakit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RumahSakit rumahSakit = db.RumahSakit.Find(id);
            if (rumahSakit == null)
            {
                return HttpNotFound();
            }
            ViewBag.KecamatanID = new SelectList(db.Kecamatan, "ID", "Nama", rumahSakit.KecamatanID);
            return View(rumahSakit);
        }

        // POST: RumahSakit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nama,KecamatanID,NoTelepon")] RumahSakit rumahSakit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rumahSakit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KecamatanID = new SelectList(db.Kecamatan, "ID", "Nama", rumahSakit.KecamatanID);
            return View(rumahSakit);
        }

        // GET: RumahSakit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RumahSakit rumahSakit = db.RumahSakit.Find(id);
            if (rumahSakit == null)
            {
                return HttpNotFound();
            }
            return View(rumahSakit);
        }

        // POST: RumahSakit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RumahSakit rumahSakit = db.RumahSakit.Find(id);
            db.RumahSakit.Remove(rumahSakit);
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
