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
    public class RiwayatPenyakitsController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: RiwayatPenyakits
        public ActionResult Index()
        {
            var riwayatPenyakits = db.RiwayatPenyakit.Include(r => r.Orang).Include(r => r.Penyakit);
            return View(riwayatPenyakits.ToList());
        }



        // GET: RiwayatPenyakits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakit riwayatPenyakit = db.RiwayatPenyakit.Find(id);
            if (riwayatPenyakit == null)
            {
                return HttpNotFound();
            }
            return View(riwayatPenyakit);
        }

        // GET: RiwayatPenyakits/Create
        public ActionResult Create()
        {
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama");
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama");
            return View();
        }

        // POST: RiwayatPenyakits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,PenyakitID,Tahun")] RiwayatPenyakit riwayatPenyakit)
        {
            if (ModelState.IsValid)
            {
                db.RiwayatPenyakit.Add(riwayatPenyakit);
                db.SaveChanges();
            }
            return RedirectToAction("Index");

            //ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakit.PasienID);
            //ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakit.PenyakitID);
            //return View(riwayatPenyakit);
        }

        // GET: RiwayatPenyakits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakit riwayatPenyakit = db.RiwayatPenyakit.Find(id);
            if (riwayatPenyakit == null)
            {
                return HttpNotFound();
            }
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakit.PasienID);
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakit.PenyakitID);
            return View(riwayatPenyakit);
        }

        // POST: RiwayatPenyakits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,PenyakitID,Tahun")] RiwayatPenyakit riwayatPenyakit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(riwayatPenyakit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakit.PasienID);
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakit.PenyakitID);
            return View(riwayatPenyakit);
        }

        // GET: RiwayatPenyakits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakit riwayatPenyakit = db.RiwayatPenyakit.Find(id);
            if (riwayatPenyakit == null)
            {
                return HttpNotFound();
            }
            return View(riwayatPenyakit);
        }

        // POST: RiwayatPenyakits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RiwayatPenyakit riwayatPenyakit = db.RiwayatPenyakit.Find(id);
            db.RiwayatPenyakit.Remove(riwayatPenyakit);
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
