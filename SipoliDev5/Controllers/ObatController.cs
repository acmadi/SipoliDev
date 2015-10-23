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
using System.Web.Security;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class ObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        public JsonResult GetDataObat(string term)
        {
            var obat = (from r in db.Obat
                        where r.Nama.ToLower().Contains(term.ToLower())
                        select new { label = r.Nama, value = r.Nama, id = r.ID });//query
            return Json(obat, JsonRequestBehavior.AllowGet);
        }
        // GET: /Obat/
        public ActionResult Index(string Nama, string Satuan, string Golongan, string Sortby, int? page, string pesan, bool? error, string pesan2)
        {
            var obat = from a in db.Obat
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

            ViewBag.error = error;
            ViewBag.pesan = pesan;
            ViewBag.pesan2 = pesan2;

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
            ViewBag.SortNamaParameter = Sortby == "Nama" ? "Nama Desc" : "Nama";
            ViewBag.SortSatuanParameter = Sortby == "Satuan" ? "Satuan Desc" : "Satuan";
            ViewBag.SortGolonganParameter = Sortby == "Golongan" ? "Golongan Desc" : "Golongan";

            switch (Sortby)
            {
                case "Nama Desc":
                    obat = obat.OrderByDescending(b => b.Nama);
                    break;
                case "Nama":
                    obat = obat.OrderBy(b => b.Nama);
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
                    obat = obat.OrderByDescending(g => g.ID);
                    break;
            }

            //export
            Session["obats"] = obat.ToList<Obat_ViewModel>();
            return View(obat.ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult ExportData()
        {
            var obat = (List<Obat_ViewModel>)Session["obats"];
            //obat.ToList();
            obat.ToList<Obat_ViewModel>();
            StringBuilder sb = new StringBuilder();
            if (obat != null && obat.Any())
            {
                sb.Append("<table style='1px solid black; font-size:20px;'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='3' style='width:120px;' align='center'><b>DATA OBAT POLIKLINIK</b></td>");
                sb.Append("<td style='font-size:15px;'>[<i>Terunduh:</i>" + DateTime.Now + "]</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:35px; border:1px solid black;background-color: yellow;'><center><b>NO</b></center></td>");
                sb.Append("<td style='width:300px;border:1px solid black;background-color: yellow;'><center><b>NAMA OBAT</b></center></td>");
                sb.Append("<td style='width:200px;border:1px solid black;background-color: yellow;'><center><b>GOLONGAN OBAT</b></center></td>");
                sb.Append("<td style='width:210px;border:1px solid black;background-color: yellow;'><center><b>SATUAN OBAT</b></center></td>");
                sb.Append("</tr>");

                int i = 1;
                foreach (var result in obat)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='border:1px solid black;'>" + i + "</td>");
                    sb.Append("<td style='border:1px solid black;'>" + result.Nama + "</td>");
                    sb.Append("<td style='border:1px solid black;'>" + result.GolonganObat + "</td>");
                    sb.Append("<td style='border:1px solid black;'>" + result.SatuanObat + "</td>");
                    sb.Append("</tr>");
                    i++;
                }
            }
            string sFileName = "[" + DateTime.Now + "] DATA OBAT POLIKLINIK IPB.xls";
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }

        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
        public PartialViewResult _Create()
        {
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama");
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama");

            return PartialView();
        }

        // POST: /Obat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Obat obat)
        {

            /*var stoks = (from r in db.StokObat
                        where r.KlinikID == 2
                        where r.ObatID == 12
                        select r.ID).ToString();
            StokObat stok = db.StokObat.Find(stoks);
            string hasil =  stok.Stok.Value.ToString();*/
            
            ViewBag.error = false;
            string pesan = "";
            var ada = false;
            ViewBag.pesan2 = "";

            //nama obat sudah ada
            if (db.Obat.Any(p => p.Nama == obat.Nama))
            {
                ViewBag.error = true;
                ViewBag.pesan2 += " Nama Obat "+obat.Nama+" sudah ada.";
            }

            //field kosong
            if (obat.Nama == null || obat.SatuanObatID == null || obat.GolonganObatID == null)
            {
                pesan += "Silakan masukkan:";
            }
            
            if (obat.Nama == null)
            {
                ViewBag.error = true;
                pesan += " Nama Obat";
                ada = true;
            }

            if (obat.SatuanObatID == null)
            {
                ViewBag.error = true;
                if (ada == true) pesan += ",";
                pesan += " Satuan Obat";
                ada = true;
            }
            if (obat.GolonganObatID == null)
            {
                ViewBag.error = true;
                if (ada == true) pesan += ",";
                pesan += " Golongan Obat";
                ada = true;
            }

            //save to database
            if (ModelState.IsValid && !ViewBag.error)
            {
                db.Obat.Add(obat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pesan = pesan;
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);

            return RedirectToAction("Index", new { error = ViewBag.error, pesan = ViewBag.pesan, pesan2 = ViewBag.pesan2 });
        }

        // GET: /Obat/Edit/5
        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
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
            ViewBag.before = obat.Nama;
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);

            return View(obat);
        }

        // POST: /Obat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Obat obat, string before)
        {
            before = Request["before"];
            ViewBag.error = false;
            string pesan = "";

            //Nama obat tidak boleh kosong
            if (obat.Nama == null)
            {
                pesan += "Silakan Masukkan";
            }
            if (obat.Nama == null)
            {
                ViewBag.error = true;
                pesan += " Nama Obat.";
            }

            //Nama obat tidak boleh sama
            if (!String.IsNullOrEmpty(obat.Nama))
            {
                if (obat.Nama.ToUpper() != before.ToUpper()) { 
                    if (db.Obat.Any(p => p.Nama == obat.Nama))
                    {
                        ViewBag.error = true;
                        pesan += " Nama Obat "+obat.Nama+" sudah ada.";
                    }
                }   
            }
           
            //save to database
            if (ModelState.IsValid && !ViewBag.error)
            {
                db.Entry(obat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pesan = pesan;
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama", obat.GolonganObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", obat.SatuanObatID);
            return View(obat);
        }

        // GET: /Obat/Delete/5
        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
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
            //return View(obat);
            return PartialView("_Delete", obat);
        }

        // POST: /Obat/Delete/5
        [Authorize(Roles = "Admin,Staf,StafBaranangsiang")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pengeluaranobat = db.PengeluaranObat.Where(p => p.ObatID == id).ToList();
            var pengadaanobat = db.PengadaanObat.Where(p => p.ObatID == id).ToList();
            var stokobat = db.StokObat.Where(p => p.ObatID == id).ToList();
            if ((pengeluaranobat != null) || (pengadaanobat != null)){
                db.PengeluaranObat.RemoveRange(pengeluaranobat);
                db.PengadaanObat.RemoveRange(pengadaanobat);
                db.StokObat.RemoveRange(stokobat);
                db.SaveChanges();
            }
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

        // GET: /Obat/Create
        /*public ActionResult Create()
        {
            ViewBag.GolonganObatID = new SelectList(db.GolonganObat, "ID", "Nama");
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama");
            return View();
        }*/
    }
}
