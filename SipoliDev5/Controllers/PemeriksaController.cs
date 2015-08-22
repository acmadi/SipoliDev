using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SipoliDev5.Models;
using SipoliDev5.Models.ViewModels;

using PagedList;
using PagedList.Mvc;

namespace SipoliDev5.Controllers
{
    [Authorize(Roles = "Admin,Staf,Pemeriksa")]
    public class PemeriksaController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        private List<Pemeriksa> listPemeriksa = new List<Pemeriksa>();

        // GET: Pemeriksa
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DaftarPemeriksaIndex(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var j = 1;
            //var data1 = db.mstDokter.Include(p => p.Pegawai).Include(o => o.Pegawai.Orang);

            var data = (from d in db.mstDokter
                        join p in db.Pegawai on d.PegawaiID equals p.ID
                        join o in db.Orang on p.ID equals o.ID
                        select new
                        {
                            PegawaiID = d.PegawaiID,
                            Nama = d.Pegawai.Orang.Nama,
                            NomorKTP = d.Pegawai.NomorKTP,
                            TMT = d.TMT,
                            TST = d.TST,
                            StatusAktif = d.StatusAktif,
                            GelarDepan = d.Pegawai.GelarDepan,
                            GelarBelakang = d.Pegawai.GelarBelakang
                        }).Distinct();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.Nama);
                    break;
                case "Date":
                    data = data.OrderBy(s => s.TMT);
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.TMT);
                    break;
                default:
                    data = data.OrderBy(s => s.Nama);
                    break;
            }

            foreach (var item in data)
            {
                listPemeriksa.Add(new Pemeriksa
                {
                    No = j,
                    PegawaiID = item.PegawaiID,
                    NomorKTP = item.NomorKTP,
                    Nama = item.Nama,
                    TMT = item.TMT,
                    TST = item.TST,
                    StatusAktif = item.StatusAktif,
                    GelarDepan = item.GelarDepan,
                    GelarBelakang = item.GelarBelakang,

                });
                j++;
            }

            return PartialView(listPemeriksa.ToPagedList(page ?? 1, 6));
        }

        //Search Pegawai
        public JsonResult SearchPegawai(string term)
        {
            var result = (from Peg in db.Pegawai
                          where Peg.NomorKTP.Contains(term)
                          select new
                          {
                              nama = Peg.Orang.Nama,
                              value = Peg.ID,
                              gelarDepan = Peg.GelarDepan,
                              gelarBelakang = Peg.GelarBelakang,
                              noKTP = Peg.NomorKTP

                          }).Take(10);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Pemeriksa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mstDokter mstDokter = db.mstDokter.Find(id);
            if (mstDokter == null)
            {
                return HttpNotFound();
            }
            return View(mstDokter);
        }

        // GET: Pemeriksa/Create
        public ActionResult Create()
        {
            ViewBag.PegawaiID = new SelectList(db.Pegawai, "ID", "GelarDepan");
            return View(new Pemeriksa
            {
                StatusAktif = true
            });
        }

        // POST: Pemeriksa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PegawaiID,TMT,TST,StatusAktif")] mstDokter mstDokter)
        {
            if (ModelState.IsValid)
            {
                db.mstDokter.Add(mstDokter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PegawaiID = new SelectList(db.Pegawai, "ID", "GelarDepan", mstDokter.PegawaiID);
            return View(mstDokter);
        }

        // GET: Pemeriksa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //mstDokter mstDokter = db.mstDokter.Find(id);
            var mstDokter = db.mstDokter.Find(id);
            if (mstDokter == null)
            {
                return HttpNotFound();
            }
            PemeriksaConst pemeriksa = new PemeriksaConst(mstDokter.PegawaiID, mstDokter.TMT,
                mstDokter.TST, mstDokter.StatusAktif);

            ViewBag.NamaPemeriksa = mstDokter.Pegawai.Orang.Nama;
            ViewBag.GelarDepan = mstDokter.Pegawai.GelarDepan;
            ViewBag.GelarBelakang = mstDokter.Pegawai.GelarBelakang;

            //ViewBag.PegawaiID = new SelectList(db.Pegawai, "ID", "GelarDepan", mstDokter.PegawaiID);

            return View(pemeriksa);
        }

        // POST: Pemeriksa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PegawaiID,TMT,TST,StatusAktif")] mstDokter mstDokter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstDokter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PegawaiID = new SelectList(db.Pegawai, "ID", "GelarDepan", mstDokter.PegawaiID);
            return View(mstDokter);
        }

        // GET: Pemeriksa/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var rekamMedis = db.RekamMedik.Where(p => p.DokterID == id);
            if (rekamMedis != null)
            {
                db.RekamMedik.RemoveRange(rekamMedis);
                db.SaveChanges();
            }

            mstDokter mstDokter = db.mstDokter.Find(id);
            if (mstDokter == null)
            {
                return HttpNotFound();
            }
            return View(mstDokter);
        }

        //LAMA
        /*
         // GET: Pemeriksa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mstDokter mstDokter = db.mstDokter.Find(id);
            if (mstDokter == null)
            {
                return HttpNotFound();
            }
            return View(mstDokter);
        }
          
        // POST: Pemeriksa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mstDokter mstDokter = db.mstDokter.Find(id);
            db.mstDokter.Remove(mstDokter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         */

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
