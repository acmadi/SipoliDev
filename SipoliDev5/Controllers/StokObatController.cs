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
using System.Reflection;//view model

namespace SipoliDev5.Controllers
{
    public class StokObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: /StokObat/
        public ActionResult Index(string Obat, string Klinik, DateTime? Date, int? Year, string Month, string Sortby, int? page)
        {
            var stokobat = (from a in db.StokObat
                            //join b in db.PengeluaranObat on a.ObatID equals b.ObatID
                            //join c in db.PengadaanObat on a.ObatID equals c.ObatID
                            //where a.KlinikID == 1 
                            /*where (from b in db.StokObat
                                           orderby b.Tanggal descending                                            
                                           group b by new { b.ObatID, b.KlinikID }
                                           into c
                                           select
                                           c.Key.ObatID).Contains(a.ID)*/
                            select new StokObat_ViewModel()
                            {
                                ID = a.ID,
                                Tanggal = a.Tanggal,
                                KlinikID = a.KlinikID,
                                ObatID = a.ObatID,
                                Stok = a.Stok,
                                Obat = a.Obat.Nama,
                                Klinik = a.Klinik.Nama,
                                SatuanObat = a.Obat.SatuanObat.Nama
                            });

            //filtering
            ViewBag.Obat = new SelectList(db.Obat, "ID", "Nama");//output berupa ID tetapi dalam bentuk string
            ViewBag.Klinik = new SelectList(db.Klinik, "ID", "Nama");

            var SelectListYear = ((from year in db.PengadaanObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
            ViewBag.Year = new SelectList(SelectListYear);


          
            if (!String.IsNullOrEmpty(Obat))
            {
                stokobat = stokobat.Where(s => s.Obat.ToUpper().Contains(Obat.ToUpper()));
            
            }

            if (!String.IsNullOrEmpty(Klinik))
            {
                stokobat = stokobat.Where(b => b.Klinik == Klinik);
            }

            if (Date != null)
            {
                stokobat = stokobat.Where(c => c.Tanggal == Date);
            }

            if (Year != null)
            {
                stokobat = stokobat.Where(d => d.Tanggal.Value.Year == Year);
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
                stokobat = stokobat.Where(e => e.Tanggal.Value.Month == MonthInt);
            }

            //sorting
            ViewBag.SortTanggalParameter = string.IsNullOrEmpty(Sortby) ? "Tanggal" : "";
            ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
            ViewBag.SortKlinikParameter = Sortby == "Klinik" ? "Klinik Desc" : "Klinik";

            switch (Sortby)
            {
                case "Tanggal":
                    stokobat = stokobat.OrderBy(a => a.Tanggal);
                    break;
                case "Obat":
                    stokobat = stokobat.OrderBy(b => b.Obat);
                    break;
                case "Obat Desc":
                    stokobat = stokobat.OrderByDescending(b => b.Obat);
                    break;
                case "Klinik":
                    stokobat = stokobat.OrderBy(c => c.Klinik);
                    break;
                case "Klinik Desc":
                    stokobat = stokobat.OrderByDescending(c => c.Klinik);
                    break;
                default:
                    stokobat = stokobat.OrderByDescending(g => g.Tanggal);
                    break;
            }

            //export
            Session["stokobats"] = stokobat.ToList<StokObat_ViewModel>();

            return View(stokobat.ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult ExportData()
        {
            var stokobat = (List<StokObat_ViewModel>)Session["stokobats"];
            //obat.ToList();
            stokobat.ToList<StokObat_ViewModel>();
            StringBuilder sb = new StringBuilder();
            if (stokobat != null && stokobat.Any())
            {
                sb.Append("<table style='1px solid black; font-size:12px;'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='3' style='width:120px', align='center'><b>DATA OBAT POLIKLINIK</b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:30px;'><center><b>NO</b></center></td>");
                sb.Append("<td style='width:300px;'><center><b>TANGGAL</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>NAMA OBAT</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>STOK OBAT</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                sb.Append("<td style='width:120px;'><center><b>KLINIK</b></center></td>");
                sb.Append("</tr>");

                int i = 1;
                foreach (var result in stokobat)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + i + "</td>");
                    sb.Append("<td>" + result.Tanggal + "</td>");
                    sb.Append("<td>" + result.Obat + "</td>");
                    sb.Append("<td>" + result.Stok + "</td>");
                    sb.Append("<td>" + result.SatuanObat + "</td>");
                    sb.Append("<td>" + result.Klinik + "</td>");
                    sb.Append("</tr>");
                    i++;
                }
            }
            string sFileName = "DATA STOK OBAT POLIKLINIK.xls";
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }

        // GET: /StokObat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokobat = db.StokObat.Find(id);
            if (stokobat == null)
            {
                return HttpNotFound();
            }
            return View(stokobat);
        }

        // GET: /StokObat/Create
        public ActionResult Create()
        {
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama");
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama");
            return View();
        }

        // POST: /StokObat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StokObat stokobat)
        {
            if (ModelState.IsValid)
            {
                db.StokObat.Add(stokobat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama", stokobat.KlinikID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", stokobat.ObatID);
            return View(stokobat);
        }

        // GET: /StokObat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokobat = db.StokObat.Find(id);
            if (stokobat == null)
            {
                return HttpNotFound();
            }
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama", stokobat.KlinikID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", stokobat.ObatID);
            return View(stokobat);
        }

        // POST: /StokObat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StokObat stokobat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stokobat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama", stokobat.KlinikID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", stokobat.ObatID);
            return View(stokobat);
        }

        // GET: /StokObat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokobat = db.StokObat.Find(id);
            if (stokobat == null)
            {
                return HttpNotFound();
            }
            return View(stokobat);
        }

        // POST: /StokObat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StokObat stokobat = db.StokObat.Find(id);
            db.StokObat.Remove(stokobat);
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
       /* public ActionResult MultipleCommand(string Obat, string Klinik, DateTime? Date, int? Year, string Month, string Command, string Sortby, int? page)
        {
            var stokobat = from a in db.StokObat.Include(t => t.Klinik).Include(t => t.Obat)
                           select new StokObat_ViewModel()
                           {
                               ID = a.ID,
                               Tanggal = a.Tanggal,
                               KlinikID = a.KlinikID,
                               ObatID = a.ObatID,
                               Stok = a.Stok,
                               Obat = a.Obat.Nama,
                               Klinik = a.Klinik.Nama,
                               SatuanObat = a.Obat.SatuanObat.Nama
                           };

            //filtering
            ViewBag.Obat = new SelectList(db.Obat, "ID", "Nama");//output berupa ID tetapi dalam bentuk string
            ViewBag.Klinik = new SelectList(db.Klinik, "ID", "Nama");

            var SelectListYear = ((from year in db.PengadaanObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
            ViewBag.Year = new SelectList(SelectListYear);

            if (!String.IsNullOrEmpty(Obat))
            {
                int ObatInt = Convert.ToInt32(Obat);
                stokobat = stokobat.Where(a => a.ObatID == ObatInt);//paging
            }

            if (!String.IsNullOrEmpty(Klinik))
            {
                int KlinikInt = Convert.ToInt32(Klinik);
                stokobat = stokobat.Where(b => b.KlinikID == KlinikInt);
            }

            if (Date != null)
            {
                stokobat = stokobat.Where(c => c.Tanggal == Date);
            }

            if (Year != null)
            {
                stokobat = stokobat.Where(d => d.Tanggal.Value.Year == Year);
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
                stokobat = stokobat.Where(e => e.Tanggal.Value.Month == MonthInt);
            }
           
            if (Command == "Filter")
            {
                //sorting
                ViewBag.SortTanggalParameter = string.IsNullOrEmpty(Sortby) ? "Tanggal" : "";
                ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
                ViewBag.SortKlinikParameter = Sortby == "Klinik" ? "Klinik Desc" : "Klinik";

                switch (Sortby)
                {
                    case "Tanggal":
                        stokobat = stokobat.OrderBy(a => a.Tanggal);
                        break;
                    case "Obat":
                        stokobat = stokobat.OrderBy(b => b.Obat);
                        break;
                    case "Obat Desc":
                        stokobat = stokobat.OrderByDescending(b => b.Obat);
                        break;
                    case "Klinik":
                        stokobat = stokobat.OrderBy(c => c.Klinik);
                        break;
                    case "Klinik Desc":
                        stokobat = stokobat.OrderByDescending(c => c.Klinik);
                        break;
                    default:
                        stokobat = stokobat.OrderByDescending(g => g.Tanggal);
                        break;
                }

                //export
                Session["stokobats"] = stokobat.ToList<StokObat_ViewModel>();

                return View(stokobat.ToList().ToPagedList(page ?? 1, 2));

            }
            if (Command == "Export")
            {
                
                stokobat.ToList<StokObat_ViewModel>();
                StringBuilder sb = new StringBuilder();
                if (stokobat != null && stokobat.Any())
                {
                    sb.Append("<table style='1px solid black; font-size:12px;'>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='width:120px', align='center'><b>DATA PENGADAAN OBAT POLIKLINIK</b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:120px;'><center><b>TANGGAL</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>STOK</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>LOKASI KLINIK</b></center></td>");
                    sb.Append("</tr>");

                    foreach (var result in stokobat)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + result.Tanggal + "</td>");
                        sb.Append("<td>" + result.Obat + "</td>");
                        sb.Append("<td>" + result.Stok + "</td>");
                        sb.Append("<td>" + result.SatuanObat + "</td>");
                        sb.Append("<td>" + result.Klinik + "</td>");
                        sb.Append("</tr>");
                    }
                }
                string sFileName = "DATA STOK OBAT POLIKLINIK.xls";
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(buffer, "application/vnd.ms-excel");
            }
            return RedirectToAction("Index");
        }*/
