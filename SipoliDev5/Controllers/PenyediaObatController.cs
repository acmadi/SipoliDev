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

namespace SipoliDev3.Controllers
{
    public class PenyediaObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        public JsonResult GetDataPenyedia(string term)
        {
            var penyediaobat = (from r in db.PenyediaObat
                        where r.Nama.ToLower().Contains(term.ToLower())
                        select new { label = r.Nama, value = r.Nama, id = r.Nama });//query
            return Json(penyediaobat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataCP(string term)
        {
            var cp = (from r in db.PenyediaObat
                        where r.ContactPerson.ToLower().Contains(term.ToLower())
                        select new { label = r.ContactPerson, value = r.ContactPerson, id = r.ContactPerson });//query
            return Json(cp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataKotaKabupaten(string term)
        {
            var kotakabupaten = (from r in db.KotaKabupaten
                        where r.Nama.ToLower().Contains(term.ToLower())
                        select new { label = r.Nama, value = r.Nama, id = r.ID });//query
            return Json(kotakabupaten, JsonRequestBehavior.AllowGet);
        }

        // GET: /PenyediaObat/
        public ActionResult Index(string Nama, string Kota, string CP, string Sortby, int? page, string pesan, bool? error, string pesan2, string pesan3)
        {
            var penyediaobat = from a in db.PenyediaObat.Include(p => p.KotaKabupaten)
                               select new PenyediaObat_ViewModel
                               {
                                   ID = a.ID,
                                   Nama = a.Nama,
                                   KotaKabupatenID = a.KotaKabupatenID,
                                   KotaKabupatenNama = a.KotaKabupaten.Nama,
                                   ContactPerson = a.ContactPerson,
                                   NomorContactPerson = a.NomorContactPerson,
                               };

            ViewBag.error = error;
            ViewBag.pesan = pesan;
            ViewBag.pesan2 = pesan2;
            ViewBag.pesan3 = pesan3;
            //ViewBag.pobat = pobat;

            //filtering
            //ViewBag.Kota = new SelectList(db.KotaKabupaten, "ID", "Nama");

            if (!String.IsNullOrEmpty(Kota))
            {
                //int KotaInt = Convert.ToInt32(Kota);
                penyediaobat = penyediaobat.Where(a => a.KotaKabupatenNama.Contains(Kota));
            }

            if (!String.IsNullOrEmpty(Nama))
            {
                penyediaobat = penyediaobat.Where(b => b.Nama.Contains(Nama));
            }

            if (!String.IsNullOrEmpty(CP))
            {
                penyediaobat = penyediaobat.Where(c => c.ContactPerson.Contains(CP));
            }

            //sorting
            ViewBag.SortNamaParameter = Sortby == "Nama" ? "Nama Desc" : "Nama";
            ViewBag.SortKotaParameter = Sortby == "Kota" ? "Kota Desc" : "Kota";
            ViewBag.SortContactPersonParameter = Sortby == "ContactPerson" ? "ContactPerson Desc" : "ContactPerson";

            switch (Sortby)
            {
                case "Nama":
                    penyediaobat = penyediaobat.OrderBy(a => a.Nama);
                    break;
                case "Nama Desc":
                    penyediaobat = penyediaobat.OrderByDescending(a => a.Nama);
                    break;
                case "Kota":
                    penyediaobat = penyediaobat.OrderBy(b => b.KotaKabupatenNama);
                    break;
                case "Kota Desc":
                    penyediaobat = penyediaobat.OrderByDescending(d => d.KotaKabupatenNama);
                    break;
                case "ContactPerson":
                    penyediaobat = penyediaobat.OrderBy(c => c.ContactPerson);
                    break;
                case "ContactPerson Desc":
                    penyediaobat = penyediaobat.OrderByDescending(c => c.ContactPerson);
                    break;
                default:
                    penyediaobat = penyediaobat.OrderBy(g => g.ID);
                    break;
            }

            //export
            Session["penyediaobats"] = penyediaobat.ToList<PenyediaObat_ViewModel>();

            return View(penyediaobat.ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult ExportData()
        {
            var penyediaobat = (List<PenyediaObat_ViewModel>)Session["penyediaobats"];
            //penyediaobat.ToList();
            penyediaobat.ToList<PenyediaObat_ViewModel>();
            StringBuilder sb = new StringBuilder();
            int i = 1;
            if (penyediaobat != null && penyediaobat.Any())
            {
                sb.Append("<table style='1px solid black; font-size:20px;'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='4' style='width:120px;', align='center'><b>DATA PENYEDIA OBAT POLIKLINIK IPB</b></td>");
                sb.Append("<td style='font-size:15px;'>[<i>Terunduh:</i>" + DateTime.Now + "]</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:35px;border:1px solid black;background-color: yellow;'><center><b>NO</b></center></td>");
                sb.Append("<td style='width:300px;border:1px solid black;background-color: yellow;'><center><b>NAMA</b></center></td>");
                sb.Append("<td style='width:300px;border:1px solid black;background-color: yellow;'><center><b>KOTA/KABUPATEN</b></center></td>");
                sb.Append("<td style='width:200px;border:1px solid black;background-color: yellow;'><center><b>CONTACT PERSON</b></center></td>");
                sb.Append("<td style='width:250px;border:1px solid black;background-color: yellow;'><center><b>NOMOR CONTACT PERSON</b></center></td>");
                sb.Append("</tr>");

                foreach (var result in penyediaobat)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='border:1px solid black'>" + i + "</td>");
                    sb.Append("<td style='border:1px solid black'>" + result.Nama + "</td>");
                    sb.Append("<td style='border:1px solid black'>" + result.KotaKabupatenNama + "</td>");
                    sb.Append("<td style='border:1px solid black'>" + result.ContactPerson + "</td>");
                    sb.Append("<td style='border:1px solid black'>" + result.NomorContactPerson + "</td>");
                    sb.Append("</tr>");
                    i++;
                }
            }
            string sFileName = "[" + DateTime.Now + "] DATA PENYEDIA OBAT POLIKLINIK IPB.xls";
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }
        public PartialViewResult _Create()
        {
            ViewBag.KotaKabupatenID = new SelectList(db.KotaKabupaten, "ID", "Nama");

            return PartialView();
        }

        // POST: /PenyediaObat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PenyediaObat penyediaobat)
        {
            ViewBag.error = false;
            string pesan = "";
            var ada = false;
            string pesan2 = "";
            string pesan3 = "";

            //Nama penyedia obat tidak boleh sama 
            if (!String.IsNullOrEmpty(penyediaobat.Nama))
            {
                if (db.PenyediaObat.Any(p => p.Nama == penyediaobat.Nama))
                {
                   ViewBag.error = true;
                   pesan2 += " Nama Penyedia Obat " + penyediaobat.Nama + " sudah ada.";
                }
            }

            //Nama penyedia obat dan kota kabupaten tidak boleh kosong
            if (penyediaobat.Nama == null || penyediaobat.KotaKabupatenID == null)
            {
                pesan += "Silakan masukkan:";
            }
            if (penyediaobat.Nama == null)
            {
                ViewBag.error = true;
                pesan += " Nama Penyedia Obat";
                ada = true;
            }
            if (penyediaobat.KotaKabupatenID == null)
            {
                ViewBag.error = true;
                if (ada == true) pesan += ",";
                pesan += " Kota/Kabupaten";
                ada = true;
            }

            //Nomor CP harus dalam bilangan dan string spasi
            if (!String.IsNullOrEmpty(penyediaobat.NomorContactPerson))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(penyediaobat.NomorContactPerson, "^[0-9 ]+$"))
                {
                    ViewBag.error = true;
                    pesan3 += "Masukkan angka pada Nomor Contact Person.";
                }
            }
            
            //save to database
            if (ModelState.IsValid && !ViewBag.error)
            {
                db.PenyediaObat.Add(penyediaobat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pesan = pesan;
            ViewBag.pesan2 = pesan2;
            ViewBag.pesan3 = pesan3;
            ViewBag.KotaKabupatenID = new SelectList(db.KotaKabupaten, "ID", "Nama", penyediaobat.KotaKabupatenID);
            return RedirectToAction("Index", new { error = ViewBag.error, pesan = ViewBag.pesan, pesan2 = ViewBag.pesan2, pesan3 = ViewBag.pesan3 });
        }

        // GET: /PenyediaObat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenyediaObat penyediaobat = db.PenyediaObat.Find(id);
            if (penyediaobat == null)
            {
                return HttpNotFound();
            }
            ViewBag.before = penyediaobat.Nama;
            ViewBag.KotaKabupatenID = new SelectList(db.KotaKabupaten, "ID", "Nama", penyediaobat.KotaKabupatenID);
            return View(penyediaobat);
        }

        // POST: /PenyediaObat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PenyediaObat penyediaobat, string before)
        {
            before = Request["before"];
            ViewBag.error = false;
            string pesan = "";
            string pesan2 = "";
            
            //Nama penyedia obat tidak boleh kosong
            if (penyediaobat.Nama == null)
            {
                ViewBag.error = true;
                pesan += "Silakan Masukkan Nama Penyedia Obat";
            }

            //Nomor CP harus dalam bilangan dan string spasi
            if (!String.IsNullOrEmpty(penyediaobat.NomorContactPerson)) 
            { 
                if (!System.Text.RegularExpressions.Regex.IsMatch(penyediaobat.NomorContactPerson, "^[0-9 ]+$")) 
                {
                    ViewBag.error = true;
                    pesan2 += "Masukkan angka pada Nomor Contact Person.";
                }
            }

            //Nama penyedia obat tidak boleh sama 
            if (!String.IsNullOrEmpty(penyediaobat.Nama))
            {
                if (penyediaobat.Nama.ToUpper() != before.ToUpper())
                {
                    if (db.PenyediaObat.Any(p => p.Nama == penyediaobat.Nama))
                    {
                        ViewBag.error = true;
                        pesan2 += " Nama Penyedia Obat " + penyediaobat.Nama + " sudah ada.";
                    }
                }
            }

            //save to database
            if (ModelState.IsValid && !ViewBag.error)
            {
                db.Entry(penyediaobat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pesan = pesan;
            ViewBag.pesan2 = pesan2;
            ViewBag.KotaKabupatenID = new SelectList(db.KotaKabupaten, "ID", "Nama", penyediaobat.KotaKabupatenID);
            return View(penyediaobat);
        }

        // GET: /PenyediaObat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenyediaObat penyediaobat = db.PenyediaObat.Find(id);
            if (penyediaobat == null)
            {
                return HttpNotFound();
            }
            //return View(penyediaobat);
            return PartialView("_Delete", penyediaobat);
        }

        // POST: /PenyediaObat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pengadaanobat = db.PengadaanObat.Where(p => p.PenyediaObatID == id).ToList();
            if (pengadaanobat != null)
            { 
                db.PengadaanObat.RemoveRange(pengadaanobat);
                db.SaveChanges();
            }
            
            PenyediaObat penyediaobat = db.PenyediaObat.Find(id);
            db.PenyediaObat.Remove(penyediaobat);
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

        /*// GET: /PenyediaObat/Create
       public ActionResult Create()
       {
           ViewBag.KotaKabupatenID = new SelectList(db.KotaKabupaten, "ID", "Nama");
           return View();
       }*/
    }
}
