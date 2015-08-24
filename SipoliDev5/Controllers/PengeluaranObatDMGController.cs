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
using SipoliDev5.Models.ViewModels;
using System.Data.Entity.Infrastructure;//view model
namespace SipoliDev5.Controllers
{
    public class PengeluaranObatDMGController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        public JsonResult GetDataStokObat()
        {
            var coba = from r in db.StokObat where r.KlinikID == 1 where r.Stok > 0 select new { Value = r.Obat.ID, Text = r.Obat.Nama, Stok = r.Stok };
            return Json(coba);
        }
        // GET: /PengeluaranObatDMG/
        public JsonResult GetDataPasien(string term)
        {
            var result = (from r in db.Orang
                          where r.Nama.ToLower().Contains(term.ToLower())
                          select new { label = r.Nama, value = r.Nama, id = r.ID });//query
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataObat(string term)
        {
            var obat = (from r in db.Obat
                        where r.Nama.ToLower().Contains(term.ToLower())
                        select new { label = r.Nama, value = r.Nama, id = r.ID });//query
            return Json(obat, JsonRequestBehavior.AllowGet);
        }

        // GET: /PengeluaranObat/
        public ActionResult Index(string Obat, string Pasien, string Month, DateTime? Date, int? Year, string Sortby, int? page, string Command)
        {
            var pengeluaranobat = from a in db.PengeluaranObat
                                  where a.KlinikID == 1
                                  select new PengeluaranObat_ViewModel()
                                  {
                                      ID = a.ID,
                                      Tanggal = a.Tanggal,
                                      PasienID = a.PasienID,
                                      ObatID = a.ObatID,
                                      KlinikID = a.KlinikID,
                                      Jumlah = a.Jumlah,
                                      Pasien = a.Orang.Nama,
                                      Obat = a.Obat.Nama,
                                      SatuanObat = a.Obat.SatuanObat.Nama,
                                      Klinik = a.Klinik.Nama,
                                      TujuanKlinikID = a.TujuanKlinikID,
                                      //TujuanKlinik = a.TujuanKlinik.Klinik.Nama,
                                  };

            //filtering
            var SelectListYear = ((from year in db.PengeluaranObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
            ViewBag.Year = new SelectList(SelectListYear);

            if (!String.IsNullOrEmpty(Obat))
            {
                pengeluaranobat = pengeluaranobat.Where(a => a.Obat.Contains(Obat));
            }

            if (!String.IsNullOrEmpty(Pasien))
            {
                pengeluaranobat = pengeluaranobat.Where(b => b.Pasien.Contains(Pasien));
            }

            if (Date != null)
            {
                pengeluaranobat = pengeluaranobat.Where(c => c.Tanggal == Date);
            }

            if (Year != null)
            {
                pengeluaranobat = pengeluaranobat.Where(d => d.Tanggal.Value.Year == Year);
            }

            if (!String.IsNullOrEmpty(Month))
            {
                int MonthInt;
                switch (Month)
                {
                    case "Januari":
                        MonthInt = 1;
                        break;
                    case "Februari":
                        MonthInt = 2;
                        break;
                    case "Maret":
                        MonthInt = 3;
                        break;
                    case "April":
                        MonthInt = 4;
                        break;
                    case "Mei":
                        MonthInt = 5;
                        break;
                    case "Juni":
                        MonthInt = 6;
                        break;
                    case "Juli":
                        MonthInt = 7;
                        break;
                    case "Agustus":
                        MonthInt = 8;
                        break;
                    case "September":
                        MonthInt = 9;
                        break;
                    case "Oktober":
                        MonthInt = 10;
                        break;
                    case "November":
                        MonthInt = 11;
                        break;
                    case "Desember":
                        MonthInt = 12;
                        break;
                    default:
                        MonthInt = 0;
                        break;
                }
                pengeluaranobat = pengeluaranobat.Where(e => e.Tanggal.Value.Month == MonthInt);
            }

            //sorting
            ViewBag.SortTanggalParameter = Sortby == "Tanggal" ? "Tanggal Desc" : "Tanggal";
            ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
            ViewBag.SortPasienParameter = Sortby == "Pasien" ? "Pasien Desc" : "Pasien";

            switch (Sortby)
            {
                case "Tanggal":
                    pengeluaranobat = pengeluaranobat.OrderBy(a => a.Tanggal);
                    break;
                case "Tanggal Desc":
                    pengeluaranobat = pengeluaranobat.OrderByDescending(a => a.Tanggal);
                    break;
                case "Obat":
                    pengeluaranobat = pengeluaranobat.OrderBy(b => b.Obat);
                    break;
                case "Obat Desc":
                    pengeluaranobat = pengeluaranobat.OrderByDescending(b => b.Obat);
                    break;
                case "Pasien":
                    pengeluaranobat = pengeluaranobat.OrderBy(c => c.Pasien);
                    break;
                case "Pasien Desc":
                    pengeluaranobat = pengeluaranobat.OrderByDescending(c => c.Pasien);
                    break;
                default:
                    pengeluaranobat = pengeluaranobat.OrderByDescending(g => g.ID);
                    break;
            }

            //export
            //Session["pengeluaranobats"] = pengeluaranobat.ToList<PengeluaranObat_ViewModel>();
            if (Command == "Export")
            {
                //var pengeluaranobat = (List<PengeluaranObat_ViewModel>)Session["pengeluaranobats"];
                //pengeluaranobat.ToList();
                pengeluaranobat.ToList<PengeluaranObat_ViewModel>();
                StringBuilder sb = new StringBuilder();
                if (pengeluaranobat != null && pengeluaranobat.Any())
                {
                    sb.Append("<table style='1px solid black; font-size:12px;'>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='width:120px', align='center'><b>DATA PERJALANAN HARIAN OBAT POLIKLINIK DMG</b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:120px;'><center><b>TANGGAL</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA PASIEN</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>JUMLAH</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                    sb.Append("</tr>");

                    foreach (var result in pengeluaranobat)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + result.Tanggal + "</td>");
                        sb.Append("<td>" + result.Pasien + "</td>");
                        sb.Append("<td>" + result.Obat + "</td>");
                        sb.Append("<td>" + result.Jumlah + "</td>");
                        sb.Append("<td>" + result.SatuanObat + "</td>");
                        sb.Append("</tr>");
                    }
                }
                string sFileName = "DATA PERJALANAN HARIAN OBAT POLIKLINIK DMG.xls";
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(buffer, "application/vnd.ms-excel");
            }

            return View(pengeluaranobat.ToList().ToPagedList(page ?? 1, 20));
        }

        

        // GET: /PengeluaranObatDMG/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PengeluaranObat pengeluaranobat = db.PengeluaranObat.Find(id);
            if (pengeluaranobat == null)
            {
                return HttpNotFound();
            }
            return View(pengeluaranobat);
        }

        // GET: /PengeluaranObatDMG/Create
        public ActionResult Create()
        {
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama");
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama");
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama");
            return View();
        }

        // POST: /PengeluaranObatDMG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PengeluaranObat pengeluaranobat)
        {
            if (ModelState.IsValid)
            {
                int count = Int32.Parse(Request["count"]);
                for (int kep = 1; kep <= count; kep++)
                {
                    var Jumlah = Request["Jumlah" + kep + ""].ToString();
                    var ObatID = Request["ObatID" + kep + ""].ToString();
                    if (Jumlah == null)
                        Int32.Parse(Jumlah);
                    pengeluaranobat.Jumlah = Int32.Parse(Jumlah);
                    pengeluaranobat.ObatID = Int32.Parse(ObatID);
                    db.PengeluaranObat.Add(pengeluaranobat);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
           
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengeluaranobat.ObatID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", pengeluaranobat.PasienID);
            return View(pengeluaranobat);
        }

        // GET: /PengeluaranObatDMG/Edit/5
        public ActionResult Edit(int? id, bool? E, bool? E1, bool? E2, bool? E3, bool? E4, string S)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PengeluaranObat pengeluaranobat = db.PengeluaranObat.Find(id);
            if (pengeluaranobat == null)
            {
                return HttpNotFound();
            }
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengeluaranobat.ObatID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", pengeluaranobat.PasienID);

            //simpan jumlah stok sblm diedit
            ViewBag.jmlsblmedit = pengeluaranobat.Jumlah;

            //error handling
            if (E == true)
            {
                ViewBag.E = true;
            }
            if (E1 == true)
            {
                ViewBag.E1 = true;
            }
            if (E2 == true)
            {
                ViewBag.E2 = true;
            }
            if (E3 == true)
            {
                ViewBag.E3 = true;
            }
            if (E4 == true)
            {
                ViewBag.stoksaat = S;
                ViewBag.E4 = true;
            }

            return View(pengeluaranobat);
        }

        // POST: /PengeluaranObatDMG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, PengeluaranObat pengeluaranobat, string jmlsblmedit)
        {
            ViewBag.E = false;

            //tanggal tdk blh kosong
            if (pengeluaranobat.Tanggal == null)
            {
                ViewBag.E = true;
                ViewBag.E1 = true;
            }

            //jumlah obat tdk blh kosong
            if (pengeluaranobat.Jumlah == null)
            {
                ViewBag.E = true;
                ViewBag.E2 = true;
            }

            //jumlah obat harus bilangan positif
            string jumlah = pengeluaranobat.Jumlah.ToString();
            if (pengeluaranobat.Jumlah != null)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(jumlah, "^[0-9]+$"))
                {
                    ViewBag.E = true;
                    ViewBag.E3 = true;
                }
            }

            //jumlah obat tdk blh lebih dari stok saat edit
            var stoksblmedit = (from i in db.StokObat
                                where i.ObatID == pengeluaranobat.ObatID
                                where i.KlinikID == 1
                                select i.Stok).FirstOrDefault().ToString();
            var intstoksblmedit = int.Parse(stoksblmedit);
            var intjmlsblmedit = int.Parse(jmlsblmedit);
            var stoksaatedit = intstoksblmedit + intjmlsblmedit;
            if (pengeluaranobat.Jumlah != null)
            {
                var jmlsaatedit = pengeluaranobat.Jumlah.ToString();
                var intjmlsaatedit = int.Parse(jmlsaatedit);

                if (intjmlsaatedit > stoksaatedit)
                {
                    ViewBag.E = true;
                    ViewBag.E4 = true;
                    ViewBag.stoksaatedit = stoksaatedit;
                }
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pengeluaranupdate = db.PengeluaranObat.Where(i => i.ID == id).Single();
            if (TryUpdateModel(pengeluaranupdate, "",
                new string[] { "ID", "ObatID", "Jumlah", "Tanggal", "HET", "HargaAktual", "PenyediaObatID" }) && !ViewBag.E)
            {
                try
                {
                    db.Entry(pengeluaranupdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    //log the error, uncommend dex and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try Again, and if the problem persists, see your system administrator.");
                }
            }
            //kalau gagal, kembali ke action edit
            return RedirectToAction("Edit", new { E = ViewBag.E, E1 = ViewBag.E1, E2 = ViewBag.E2, E3 = ViewBag.E3, E4 = ViewBag.E4, S = ViewBag.stoksaatedit});
        }

        // GET: /PengeluaranObatDMG/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PengeluaranObat pengeluaranobat = db.PengeluaranObat.Find(id);
            if (pengeluaranobat == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", pengeluaranobat);
        }

        // POST: /PengeluaranObatDMG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PengeluaranObat pengeluaranobat = db.PengeluaranObat.Find(id);
            db.PengeluaranObat.Remove(pengeluaranobat);
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
/*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PengeluaranObat pengeluaranobat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pengeluaranobat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama", pengeluaranobat.KlinikID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengeluaranobat.ObatID);
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", pengeluaranobat.PasienID);
            return View(pengeluaranobat);
        }*/
/*public ActionResult MultipleCommand(string Obat, string Pasien, string Month, DateTime? Date, int? Year, string Sortby, int? page, string Command)
        {
            var pengeluaranobat = from a in db.PengeluaranObat.Include(h => h.Orang).Include(h => h.Obat).Include(h => h.Klinik)
                                  where a.KlinikID == 1
                                  select new PengeluaranObat_ViewModel()
                                  {
                                      ID = a.ID,
                                      Tanggal = a.Tanggal,
                                      PasienID = a.PasienID,
                                      ObatID = a.ObatID,
                                      KlinikID = a.KlinikID,
                                      Jumlah = a.Jumlah,
                                      Pasien = a.Orang.Nama,
                                      Obat = a.Obat.Nama,
                                      SatuanObat = a.Obat.SatuanObat.Nama,
                                      Klinik = a.Klinik.Nama
                                  };

            //filtering
            var SelectListYear = ((from year in db.PengeluaranObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
            ViewBag.Year = new SelectList(SelectListYear);

            if (!String.IsNullOrEmpty(Obat))
            {
                pengeluaranobat = pengeluaranobat.Where(a => a.Obat.Contains(Obat));
            }

            if (!String.IsNullOrEmpty(Pasien))
            {
                pengeluaranobat = pengeluaranobat.Where(b => b.Pasien.Contains(Pasien));
            }

            if (Date != null)
            {
                pengeluaranobat = pengeluaranobat.Where(c => c.Tanggal == Date);
            }

            if (Year != null)
            {
                pengeluaranobat = pengeluaranobat.Where(d => d.Tanggal.Value.Year == Year);
            }

            if (!String.IsNullOrEmpty(Month))
            {
                int MonthInt;
                switch (Month)
                {
                    case "Januari":
                        MonthInt = 1;
                        break;
                    case "Februari":
                        MonthInt = 2;
                        break;
                    case "Maret":
                        MonthInt = 3;
                        break;
                    case "April":
                        MonthInt = 4;
                        break;
                    case "Mei":
                        MonthInt = 5;
                        break;
                    case "Juni":
                        MonthInt = 6;
                        break;
                    case "Juli":
                        MonthInt = 7;
                        break;
                    case "Agustus":
                        MonthInt = 8;
                        break;
                    case "September":
                        MonthInt = 9;
                        break;
                    case "Oktober":
                        MonthInt = 10;
                        break;
                    case "November":
                        MonthInt = 11;
                        break;
                    case "Desember":
                        MonthInt = 12;
                        break;
                    default:
                        MonthInt = 0;
                        break;
                }
                pengeluaranobat = pengeluaranobat.Where(e => e.Tanggal.Value.Month == MonthInt);
            }

            if (Command == "Filter")
            {
                //sorting
                ViewBag.SortTanggalParameter = Sortby == "Tanggal" ? "Tanggal Desc" : "Tanggal";
                ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
                ViewBag.SortPasienParameter = Sortby == "Pasien" ? "Pasien Desc" : "Pasien";

                switch (Sortby)
                {
                    case "Tanggal":
                        pengeluaranobat = pengeluaranobat.OrderBy(a => a.Tanggal);
                        break;
                    case "Tanggal Desc":
                        pengeluaranobat = pengeluaranobat.OrderByDescending(a => a.Tanggal);
                        break;
                    case "Obat":
                        pengeluaranobat = pengeluaranobat.OrderBy(b => b.Obat);
                        break;
                    case "Obat Desc":
                        pengeluaranobat = pengeluaranobat.OrderByDescending(b => b.Obat);
                        break;
                    case "Pasien":
                        pengeluaranobat = pengeluaranobat.OrderBy(c => c.Pasien);
                        break;
                    case "Pasien Desc":
                        pengeluaranobat = pengeluaranobat.OrderByDescending(c => c.Pasien);
                        break;
                    default:
                        pengeluaranobat = pengeluaranobat.OrderByDescending(g => g.ID);
                        break;
                }

                //export
                Session["pengeluaranobats"] = pengeluaranobat.ToList<PengeluaranObat_ViewModel>();

                return View(pengeluaranobat.ToList().ToPagedList(page ?? 1, 2));

            }
            if (Command == "Export")
            {
                //var pengeluaranobat = (List<PengeluaranObat_ViewModel>)Session["pengeluaranobats"];
                //pengeluaranobat.ToList();
                pengeluaranobat.ToList<PengeluaranObat_ViewModel>();
                StringBuilder sb = new StringBuilder();
                if (pengeluaranobat != null && pengeluaranobat.Any())
                {
                    sb.Append("<table style='1px solid black; font-size:12px;'>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='width:120px', align='center'><b>DATA PERJALANAN HARIAN OBAT POLIKLINIK DMG</b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:120px;'><center><b>TANGGAL</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA PASIEN</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>JUMLAH</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                    sb.Append("</tr>");

                    foreach (var result in pengeluaranobat)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + result.Tanggal + "</td>");
                        sb.Append("<td>" + result.Pasien + "</td>");
                        sb.Append("<td>" + result.Obat + "</td>");
                        sb.Append("<td>" + result.Jumlah + "</td>");
                        sb.Append("<td>" + result.SatuanObat + "</td>");
                        sb.Append("</tr>");
                    }
                }
                string sFileName = "DATA PERJALANAN HARIAN OBAT POLIKLINIK DMG.xls";
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(buffer, "application/vnd.ms-excel");
            }
            return RedirectToAction("Index");
        }*/
