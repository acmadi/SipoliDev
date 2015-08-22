using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SipoliDev5.Models;
using PagedList; //paging
using PagedList.Mvc;
using System.Text; //paging
using SipoliDev5.Models.ViewModels;//view model

namespace SipoliDev5.Controllers
{
    public class ObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: /Obat/
        public ActionResult Index(string Nama, string Satuan, string Golongan, string Sortby, int? page)
        {
            var obat = from a in db.Obat.Include(m => m.GolonganObat).Include(m => m.SatuanObat)
                       select new Obat_ViewModel()
                       {
                           ID = a.ID,
                           Nama = a.Nama,
                           SatuanObatID = a.SatuanObatID,
                           GolonganObatID = a.GolonganObatID,
                           SatuanObat = a.SatuanObat.Nama,
                           GolonganObat = a.GolonganObat.Nama,
                           Kegunaan = a.Kegunaan
                       };


            //filtering
            ViewBag.Satuan = new SelectList(db.SatuanObat, "ID", "Nama");//output berupa ID tetapi dalam bentuk string
            ViewBag.Golongan = new SelectList(db.GolonganObat, "ID", "Nama");

            if (!String.IsNullOrEmpty(Nama))
            {
                obat = obat.Where(b => b.Nama.Contains(Nama));
            }
            if (!String.IsNullOrEmpty(Satuan))
            {
                int SatuanInt = Convert.ToInt32(Satuan);
                obat = obat.Where(c => c.SatuanObatID == SatuanInt);
            }
            if (!String.IsNullOrEmpty(Golongan))
            {
                int GolonganInt = Convert.ToInt32(Golongan);
                obat = obat.Where(c => c.GolonganObatID == GolonganInt);
            }

            //sorting
            ViewBag.SortNamaParameter = string.IsNullOrEmpty(Sortby) ? "Nama Desc" : "";
            ViewBag.SortSatuanParameter = Sortby == "Satuan" ? "Satuan Desc" : "Satuan";
            ViewBag.SortGolonganParameter = Sortby == "Golongan" ? "Golongan Desc" : "Golongan";

            switch (Sortby)
            {
                case "Nama Desc":
                    obat = obat.OrderByDescending(b => b.Nama);
                    break;
                case "Satuan":
                    obat = obat.OrderBy(c => c.SatuanObat);
                    break;
                case "Satuan Desc":
                    obat = obat.OrderByDescending(d => d.SatuanObat);
                    break;
                case "Golongan":
                    obat = obat.OrderBy(e => e.GolonganObat);
                    break;
                case "Golongan Desc":
                    obat = obat.OrderByDescending(f => f.GolonganObat);
                    break;
                default:
                    obat = obat.OrderBy(g => g.Nama);
                    break;
            }

            //export
            Session["obats"] = obat.ToList<Obat_ViewModel>();


            return View(obat.ToList().ToPagedList(page ?? 1, 5));
        }

        public ActionResult ExportData()
        {
            var obat = (List<Obat_ViewModel>)Session["obats"];
            obat.ToList();
            StringBuilder sb = new StringBuilder();
            if (obat != null && obat.Any())
            {
                sb.Append("<table style='1px solid black; font-size:12px;'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='3' style='width:120px', align='center'><b>DATA OBAT POLIKLINIK</b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:300px;'><center><b>NAMA OBAT</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>GOLONGAN OBAT</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                sb.Append("</tr>");

                foreach (var result in obat)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + result.Nama + "</td>");
                    sb.Append("<td>" + result.GolonganObat + "</td>");
                    sb.Append("<td>" + result.SatuanObat + "</td>");
                    sb.Append("</tr>");
                }
            }
            string sFileName = "DATA OBAT POLIKLINIK.xls";
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }

        // GET: /Obat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
         
            if (obat == null)
            {
                return HttpNotFound();
            }
            return View(obat);
        }

        // GET: /Obat/Create
        public ActionResult Create()
        {
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama");
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama");
            return View();
        }


        public PartialViewResult _Create()
        {
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama");
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama");

            return PartialView();
        }

        // POST: /Obat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Nama,SatuanObatID,GolonganObatID,Kegunaan")] Obat obat)
        {
            if (ModelState.IsValid)
            {
                db.Obat.Add(obat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);
            return View(obat);
        }

        // GET: /Obat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
            if (obat == null)
            {
                return HttpNotFound();
            }
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);
            return View(obat);
        }

        // POST: /Obat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Nama,SatuanObatID,GolonganObatID,Kegunaan")] Obat obat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);
            return View(obat);
        }

        // GET: /Obat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
            if (obat == null)
            {
                return HttpNotFound();
            }
            return View(obat);
        }

        // POST: /Obat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Obat obat = db.Obat.Find(id);
            db.Obat.Remove(obat);
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
