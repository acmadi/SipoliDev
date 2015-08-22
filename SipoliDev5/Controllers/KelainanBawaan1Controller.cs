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
    [Authorize]
    public class KelainanBawaan1Controller : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        private List<KelainanBawaanCons> listKelainan = new List<KelainanBawaanCons>();

        // GET: KelainanBawaan1
        public PartialViewResult Index(int? page)
        {
            var data = (from k in db.KelainanBawaan1
                        select new
                        {
                            ID = k.ID,
                            Nama = k.Nama,
                            NamaIlmiah = k.NamaIlmiah
                        }).Distinct();

            var j = 1;
            foreach (var item in data)
            {
                listKelainan.Add(new KelainanBawaanCons((int)j, item.ID, item.Nama, item.NamaIlmiah));
                j++;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return PartialView(listKelainan.ToPagedList(pageNumber, pageSize));
        }



        // GET: KelainanBawaan1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan1 kelainanBawaan1 = db.KelainanBawaan1.Find(id);
            if (kelainanBawaan1 == null)
            {
                return HttpNotFound();
            }
            return View(kelainanBawaan1);
        }

        // GET: KelainanBawaan1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KelainanBawaan1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nama,NamaIlmiah")] KelainanBawaan1 kelainanBawaan1)
        {
            if (ModelState.IsValid)
            {
                db.KelainanBawaan1.Add(kelainanBawaan1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kelainanBawaan1);
        }

        // GET: KelainanBawaan1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan1 kelainanBawaan1 = db.KelainanBawaan1.Find(id);
            if (kelainanBawaan1 == null)
            {
                return HttpNotFound();
            }
            return View(kelainanBawaan1);
        }

        // POST: KelainanBawaan1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nama,NamaIlmiah")] KelainanBawaan1 kelainanBawaan1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kelainanBawaan1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kelainanBawaan1);
        }

        // GET: KelainanBawaan1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KelainanBawaan1 kelainanBawaan1 = db.KelainanBawaan1.Find(id);
            if (kelainanBawaan1 == null)
            {
                return HttpNotFound();
            }
            return View(kelainanBawaan1);
        }

        // POST: KelainanBawaan1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KelainanBawaan kelainanBawaan1 = db.KelainanBawaan.Find(id);
            db.KelainanBawaan.Remove(kelainanBawaan1);
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
