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

using Rotativa;

using PagedList;
using PagedList.Mvc;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class DaftarPasienController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        
        // GET: /DaftarPasien/
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Index()
        {
            return View();
        }

        //Untuk Tabel data daftar pasien
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public PartialViewResult DaftarPasienIndex(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";

            var j = 1;
            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             TempatLahir = Or.TempatLahir,
                             JenisKelaminID = Or.JenisKelaminID,
                             TanggalLahir = Or.TanggalLahir
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
                data1 = data1.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data1 = data1.OrderByDescending(s => s.Nama);
                    break;
                case "Date":
                    data1 = data1.OrderBy(s => s.TanggalLahir);
                    break;
                case "date_desc":
                    data1 = data1.OrderByDescending(s => s.TanggalLahir);
                    break;
                case "address_desc":
                    data1 = data1.OrderByDescending(s => s.TempatLahir);
                    break;
                case "Address":
                    data1 = data1.OrderBy(s => s.TempatLahir);
                    break;
                case "jk_desc":
                    data1 = data1.OrderByDescending(s => s.JenisKelaminID);
                    break;
                case "JK":
                    data1 = data1.OrderBy(s => s.JenisKelaminID);
                    break;
                default:
                    data1 = data1.OrderBy(s => s.Nama);
                    break;
            }

            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j,itemData.ID,itemData.Nama,itemData.TanggalLahir,itemData.TempatLahir,
                    itemData.JenisKelaminID,null,null,null,null,HitungUmur(itemData.TanggalLahir.Year, itemData.TanggalLahir.Month,
                    itemData.TanggalLahir.Day)));
                j++;
            }


            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }

        //untuk modal pendaftaran pasien
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult PendaftaranPasien()
        {
            return PartialView("_PendaftaranPasien");
        }

        //untuk cetak laporan daftar pasien
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult ViewDaftarPasienReport(int? bulan, int? tahun, int? page)
        {

            var j = 1;
            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         where Rm.AnamnesaDiagnosa == null
                         && Rm.Tanggal.Value.Month == bulan
                         && Rm.Tanggal.Value.Year == tahun
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             TempatLahir = Or.TempatLahir,
                             JenisKelaminID = Or.JenisKelaminID,
                             TanggalLahir = Or.TanggalLahir
                         }).Distinct();

            switch (bulan)
            {
                case 1:
                    ViewBag.Bulan = "Januari";
                    break;
                case 2:
                    ViewBag.Bulan = "Februari";
                    break;
                case 3:
                    ViewBag.Bulan = "Maret";
                    break;
                case 4:
                    ViewBag.Bulan = "April";
                    break;
                case 5:
                    ViewBag.Bulan = "Mei";
                    break;
                case 6:
                    ViewBag.Bulan = "Juni";
                    break;
                case 7:
                    ViewBag.Bulan = "Juli";
                    break;
                case 8:
                    ViewBag.Bulan = "Agustus";
                    break;
                case 9:
                    ViewBag.Bulan = "September";
                    break;
                case 10:
                    ViewBag.Bulan = "Oktober";
                    break;
                case 11:
                    ViewBag.Bulan = "November";
                    break;
                default:
                    ViewBag.Bulan = "Desember";
                    break;
            }
            ViewBag.Tahun = tahun;


            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j, itemData.ID, itemData.Nama, itemData.TanggalLahir, itemData.TempatLahir,
                    itemData.JenisKelaminID, null, null, null, null, HitungUmur(itemData.TanggalLahir.Year, itemData.TanggalLahir.Month,
                    itemData.TanggalLahir.Day)));
                j++;
            }

            return new ViewAsPdf("ViewDaftarPasienReport", "Empty", data2.ToList().ToPagedList(page ?? 1, 10));

        }




        public int HitungUmur(int? tahun, int? bulan, int? hari)
        {
            DateTime now = DateTime.Now;
            int years = now.Year - tahun.Value;
            if (now.Month < bulan.Value || (now.Month == bulan.Value && now.Day < hari.Value)) years--;

            return years;
        }

        //dari form ajax
        //[HttpPost]
        //public ActionResult Index()
        //{
        //    return Content("Thanks", "text/html");
        //}

        /**/
        public JsonResult PencarianOrang(string term)
        {
            //filter untuk orang yang sudah jadi pasien tidak boleh registrasi lagi..
            var result = (from r in db.Orang
                          where r.Nama.ToLower().Contains(term.ToLower())
                                    &&
                                !(from rm in db.RekamMedik select rm.PasienID).Contains(r.ID)
                          select new
                          {
                              label = r.Nama,
                              value = r.Nama,
                              id = r.ID,
                              alamat = r.TempatLahir,
                              tanggal = r.TanggalLahir,
                              jk = r.JenisKelaminID
                          }).Take(10);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /**/
        public JsonResult PencarianMahasiswa(string term)
        {
            var result = (from m in db.Mahasiswa
                          join or in db.Orang on m.ID equals or.ID
                          where (from sr in db.MahasiswaSarjana where m.ID == sr.ID select sr.NIM).Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            (from mr in db.MahasiswaMagister where m.ID == mr.ID select mr.NIM).Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            (from dr in db.MahasiswaDoktor where m.ID == dr.ID select dr.NIM).Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            (from dp in db.MahasiswaDiploma where m.ID == dp.ID select dp.NIM).Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                                select new{
                                    label = or.Nama,
                                    value = or.Nama,
                                    id = or.ID,
                                    alamat = or.TempatLahir,
                                    tanggal = or.TanggalLahir,
                                    jk = or.JenisKelaminID
                                }).Take(10);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Ketika relasi Mahasiswa dan MahasiswaXX 1 to 1
        /*
        public JsonResult PencarianMahasiswa(string term)
        {
            var result = (from m in db.Mahasiswa
                          join or in db.Orang on m.ID equals or.ID
                          where m.MahasiswaSarjana.NIM.Contains(term.ToLower())
                          &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            m.MahasiswaMagister.NIM.Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            m.MahasiswaDoktor.NIM.Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                            ||
                            m.MahasiswaDiploma.NIM.Contains(term.ToLower())
                            &&
                            !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)

                          select new
                          {
                              label = or.Nama,
                              value = or.Nama,
                              id = or.ID,
                              alamat = or.TempatLahir,
                              tanggal = or.TanggalLahir,
                              jk = or.JenisKelaminID
                          }).Take(10);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
         */ 


        /**/
        public JsonResult PencarianTendik(string term)
        {
            var result = (from m in db.Pegawai
                          join or in db.Orang on m.ID equals or.ID
                          where m.NomorKTP.Contains(term.ToLower())
                          &&
                          !(from rm in db.RekamMedik select rm.PasienID).Contains(m.ID)
                          select new
                          {
                              label = or.Nama,
                              value = m.NomorKTP,
                              id = or.ID,
                              alamat = or.TempatLahir,
                              tanggal = or.TanggalLahir,
                              jk = or.JenisKelaminID
                          }).Take(10);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


       
        // GET: /DaftarPasien/Details/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekammedik = db.RekamMedik.Find(id);
            if (rekammedik == null)
            {
                return HttpNotFound();
            }
            return View(rekammedik);
        }

        // GET: /DaftarPasien/Create
        //public ActionResult Create()
        //{
        //    //ViewBag.PasienID = new SelectList(db.Orangs, "ID", "Nama");
        //    //ViewBag.DokterID = new SelectList(db.Pegawais, "ID", "GelarDepan");

        //    return View();
        //}

        // POST: /DaftarPasien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,Tanggal")] RekamMedik rekammedik)
        {
            if (ModelState.IsValid)
            {
                db.RekamMedik.Add(rekammedik);
                db.SaveChanges();
            }

            return RedirectToAction("DaftarPasienIndex");
        }

        // GET: /DaftarPasien/Edit/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekammedik = db.RekamMedik.Find(id);
            if (rekammedik == null)
            {
                return HttpNotFound();
            }
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", rekammedik.PasienID);
            ViewBag.DokterID = new SelectList(db.Pegawai, "ID", "GelarDepan", rekammedik.DokterID);
            return View(rekammedik);
        }

        // POST: /DaftarPasien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,Tanggal,AnamnesaDiagnosa,Therapie,DokterID")] RekamMedik rekammedik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rekammedik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", rekammedik.PasienID);
            ViewBag.DokterID = new SelectList(db.Pegawai, "ID", "GelarDepan", rekammedik.DokterID);
            return View(rekammedik);
        }


        // GET: /DaftarPasien/Delete/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rekammedik = db.RekamMedik.Where(r => r.PasienID == id).ToList();

            //ViewBag.NamaPasien = db.Orang.Find(id).Nama;

            if (rekammedik == null)
            {
                return HttpNotFound();
            }
            db.RekamMedik.RemoveRange(rekammedik);
            db.SaveChanges();
            return RedirectToAction("DaftarPasienIndex");
        }

        /* POST DELETE
        // POST: /DaftarPasien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var rekammedik = db.RekamMedik.Where(r => r.PasienID == id).ToList();
            db.RekamMedik.RemoveRange(rekammedik);
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
