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
    public class RiwayatPenyakitKeluargaController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: RiwayatPenyakitKeluarga
        public ActionResult Index()
        {
            var riwayatPenyakitKeluargas = db.RiwayatPenyakitKeluarga.Include(r => r.HubunganKeluarga).Include(r => r.Orang).Include(r => r.Penyakit);
            return View(riwayatPenyakitKeluargas.ToList());
        }

        // GET: RiwayatPenyakitKeluarga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakitKeluarga riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Find(id);
            if (riwayatPenyakitKeluarga == null)
            {
                return HttpNotFound();
            }
            return View(riwayatPenyakitKeluarga);
        }

        // GET: RiwayatPenyakitKeluarga/Create
        public ActionResult Create()
        {
            ViewBag.HubunganKeluargaID = new SelectList(db.HubunganKeluarga, "ID", "Nama");
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama");
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama");
            return View();
        }

        // POST: RiwayatPenyakitKeluarga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,HubunganKeluargaID,PenyakitID")] RiwayatPenyakitKeluarga riwayatPenyakitKeluarga)
        {
            if (ModelState.IsValid)
            {
                db.RiwayatPenyakitKeluarga.Add(riwayatPenyakitKeluarga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HubunganKeluargaID = new SelectList(db.HubunganKeluarga, "ID", "Nama", riwayatPenyakitKeluarga.HubunganKeluargaID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakitKeluarga.PasienID);
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakitKeluarga.PenyakitID);
            return View(riwayatPenyakitKeluarga);
        }

        // GET: RiwayatPenyakitKeluarga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakitKeluarga riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Find(id);
            if (riwayatPenyakitKeluarga == null)
            {
                return HttpNotFound();
            }
            ViewBag.HubunganKeluargaID = new SelectList(db.HubunganKeluarga, "ID", "Nama", riwayatPenyakitKeluarga.HubunganKeluargaID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakitKeluarga.PasienID);
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakitKeluarga.PenyakitID);
            return View(riwayatPenyakitKeluarga);
        }

        // POST: RiwayatPenyakitKeluarga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,HubunganKeluargaID,PenyakitID")] RiwayatPenyakitKeluarga riwayatPenyakitKeluarga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(riwayatPenyakitKeluarga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HubunganKeluargaID = new SelectList(db.HubunganKeluarga, "ID", "Nama", riwayatPenyakitKeluarga.HubunganKeluargaID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", riwayatPenyakitKeluarga.PasienID);
            ViewBag.PenyakitID = new SelectList(db.Penyakit, "ID", "Nama", riwayatPenyakitKeluarga.PenyakitID);
            return View(riwayatPenyakitKeluarga);
        }

        // GET: RiwayatPenyakitKeluarga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiwayatPenyakitKeluarga riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Find(id);
            if (riwayatPenyakitKeluarga == null)
            {
                return HttpNotFound();
            }
            return View(riwayatPenyakitKeluarga);
        }

        // POST: RiwayatPenyakitKeluarga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RiwayatPenyakitKeluarga riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Find(id);
            db.RiwayatPenyakitKeluarga.Remove(riwayatPenyakitKeluarga);
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
