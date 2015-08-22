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
    public class KunjunganPasienController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        private List<KunjunganConst> listKunjungan = new List<KunjunganConst>();

        // GET: KunjunganPasien
        //[Authorize(Roles="Admin,Pemeriksa")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult DaftarKunjunganPasienIndex(string sortOrder, string searchString, string currentFilter, int? page, int? LokasiID = 1)
        {
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
           

            //var DataKunjungan = db.trxKunjungan.Include(t => t.Orang);
            var DataKunjungan = from k in db.Kunjungan
                                join o in db.Orang on k.OrangID equals o.ID
                                where k.LokasiklinikID == LokasiID
                                select new
                                {
                                    ID = k.ID,
                                    Tanggal = k.Tanggal,
                                    NoUrut = k.NoUrut,
                                    Nama = o.Nama,
                                    StatusPanggil = k.StatusPanggil,
                                    KlinikID = k.LokasiklinikID,
                                    Ket = k.Ket
                                };

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
                DataKunjungan = DataKunjungan.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    DataKunjungan = DataKunjungan.OrderByDescending(s => s.Nama);
                    break;
                case "Date":
                    DataKunjungan = DataKunjungan.OrderBy(s => s.Tanggal);
                    break;
                case "Name":
                    DataKunjungan = DataKunjungan.OrderBy(s => s.Nama);
                    break;
                default:
                    DataKunjungan = DataKunjungan.OrderByDescending(s => s.Tanggal);
                    break;
            }

            
            var j = 1;
            foreach (var item in DataKunjungan)
            {
                listKunjungan.Add(new KunjunganConst((int)j, item.ID, item.Tanggal, null, item.Nama,
                    item.NoUrut, item.StatusPanggil, (int)item.KlinikID, item.Ket));
                j++;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return PartialView(listKunjungan.ToPagedList(pageNumber, pageSize));
        }

        //untuk cetak laporan Kunjungan
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult ViewKunjunganReport(int? bulan, int? tahun, int? klinik, int? page)
        {

            var data1 = from k in db.Kunjungan
                        join o in db.Orang on k.OrangID equals o.ID
                        where k.Tanggal.Value.Month == bulan
                        && k.Tanggal.Value.Year == tahun
                        && k.LokasiklinikID == klinik
                        select new
                        {
                            ID = k.ID,
                            Tanggal = k.Tanggal,
                            NoUrut = k.NoUrut,
                            Nama = o.Nama,
                            StatusPanggil = k.StatusPanggil,
                            KlinikID = k.LokasiklinikID,
                            Ket = k.Ket
                        };

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
            if (klinik == 1)
            {
                ViewBag.LokasiKlinik = "Darmaga";
            }
            else
            {
                ViewBag.LokasiKlinik = "Baranangsiang";
            }

            List<KunjunganConst> data2 = new List<KunjunganConst>();
            var j = 1;
            foreach (var item in data1)
            {
                data2.Add(new KunjunganConst((int)j, item.ID, item.Tanggal, null, item.Nama,
                    item.NoUrut, item.StatusPanggil, (int)item.KlinikID, item.Ket));
                j++;
            }

            return new ViewAsPdf("ViewKunjunganReport", "Empty", data2.ToList().ToPagedList(page ?? 1, 20));

        }


        //Untuk JSON Pencarian Pasien
        public JsonResult SearchPasienNama(string term)
        {
            //filter untuk orang yang sudah jadi pasien tidak boleh registrasi lagi..
            var result = (from r in db.Orang
                          join Rm in db.RekamMedik on r.ID equals Rm.PasienID
                          where r.Nama.ToLower().Contains(term.ToLower())
                          select new
                          {
                              label = r.Nama,
                              value = r.Nama,
                              id = r.ID,
                              alamat = r.TempatLahir
                          }).Distinct().Take(10);


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPasienMahasiswa(string term)
        {
            var result = (from m in db.Mahasiswa
                          join r in db.RekamMedik on m.ID equals r.PasienID
                          where (from sr in db.MahasiswaSarjana where m.ID == sr.ID select sr.NIM).Contains(term.ToLower())
                          ||
                          (from mr in db.MahasiswaMagister where m.ID == mr.ID select mr.NIM).Contains(term.ToLower())
                          ||
                          (from dr in db.MahasiswaDoktor where m.ID == dr.ID select dr.NIM).Contains(term.ToLower())
                          ||
                          (from dp in db.MahasiswaDiploma where m.ID == dp.ID select dp.NIM).Contains(term.ToLower())
                          select new
                          {
                              label = m.Orang.Nama,
                              value = m.Orang.Nama,
                              id = r.PasienID,
                              alamat = r.Orang.TempatLahir
                          }).Distinct().Take(10);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPasienTendik(string term)
        {
            var result = (from p in db.Pegawai
                          join r in db.RekamMedik on p.ID equals r.PasienID
                          where p.NomorKTP.Contains(term.ToLower())
                          select new
                          {
                              label = p.Orang.Nama,
                              value = p.Orang.Nama,
                              id = r.PasienID,
                              alamat = r.Orang.TempatLahir
                          }).Distinct().Take(10);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
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

        public JsonResult PencarianMahasiswa(string term)
        {
            var result = (from m in db.Mahasiswa
                          join or in db.Orang on m.ID equals or.ID
                          where (from sr in db.MahasiswaSarjana where m.ID == sr.ID select sr.NIM).Contains(term.ToLower())
                            ||
                            (from mr in db.MahasiswaMagister where m.ID == mr.ID select mr.NIM).Contains(term.ToLower())
                            ||
                            (from dr in db.MahasiswaDoktor where m.ID == dr.ID select dr.NIM).Contains(term.ToLower())
                            ||
                            (from dp in db.MahasiswaDiploma where m.ID == dp.ID select dp.NIM).Contains(term.ToLower())
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

        //Ketika relasi Mahasiswa ke MahasiswaXX 1 to 1
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

        //untuk modal pendaftaran pasien
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult PendaftaranPasien()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePasienBaru([Bind(Include = "ID,PasienID,Tanggal")] RekamMedik rekammedik)
        {
            if (ModelState.IsValid)
            {
                db.RekamMedik.Add(rekammedik);
                db.SaveChanges();
            }

            return RedirectToAction("Create");
        }

        // GET: KunjunganPasien/Details/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SipoliDev5.Models.Kunjungan trxKunjungan = db.Kunjungan.Find(id);
            if (trxKunjungan == null)
            {
                return HttpNotFound();
            }
            return View(trxKunjungan);
        }



        // GET: KunjunganPasien/Create
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Create()
        {
            ViewBag.OrangID = new SelectList(db.Orang, "ID", "Nama");
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama");

            //tanggal today
            var today = DateTime.Now.Date;

            //bandingkan, jika ada record tanggal yg sama dengan hari ini maka inc++ noUrut
            var compare = db.Kunjungan.Where(t => t.Tanggal.Value.Year == today.Year
                && t.Tanggal.Value.Month == today.Month
                && t.Tanggal.Value.Day == today.Day).Select(n => n.NoUrut);

            //var last1 = db.trxKunjungan.OrderByDescending(t => t.Tanggal).First();

            int noUrut;
            if (compare.Count() == 0)
            {
                noUrut = 1;
                ViewBag.noUrut = noUrut;

            }
            else
            {

                var last3 = compare.OrderByDescending(n => n.Value).First();

                ViewBag.noUrut = last3 + 1;
            }


            return View(new KunjunganForm {Tanggal = DateTime.Now,StatusPanggil=false });
        }


        private int CurrentQue(DateTime dateTime)
        {
            var no = 0;
            if ((db.Kunjungan.Where(t => t.Tanggal == dateTime)) == null) no = 1;

            return no;
        }


        // POST: KunjunganPasien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Tanggal,OrangID,NoUrut,StatusPanggil,Ket,KlinikID")] KunjunganForm trxKunjungan)
        {
            if (ModelState.IsValid)
            {
                SipoliDev5.Models.Kunjungan data = new SipoliDev5.Models.Kunjungan();

                data.ID = (int)trxKunjungan.ID;
                data.Tanggal = trxKunjungan.Tanggal;
                data.OrangID = (int)trxKunjungan.OrangID;
                data.NoUrut = trxKunjungan.NoUrut;
                data.StatusPanggil = trxKunjungan.StatusPanggil;
                data.Ket = trxKunjungan.Ket;
                data.LokasiklinikID = trxKunjungan.KlinikID;

                db.Kunjungan.Add(data);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrangID = new SelectList(db.Orang, "ID", "Nama", trxKunjungan.OrangID);
            ViewBag.KlinikID = new SelectList(db.Klinik, "ID", "Nama");
            ViewBag.noUrut = trxKunjungan.NoUrut;
            return View(trxKunjungan);
        }


        // GET: KunjunganPasien/Edit/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var data = db.Kunjungan.Find(id);
            //trxKunjungan trxKunjungan = db.trxKunjungan.Find(id);

            if (data == null)
            {
                return HttpNotFound();
            }
            KunjunganConst kunjungan = new KunjunganConst(null, data.ID, data.Tanggal, (int)data.OrangID, null,
                (int)data.NoUrut, (bool)data.StatusPanggil, (int)data.LokasiklinikID, data.Ket);

            //ViewBag.OrangID = new SelectList(db.Orangs, "ID", "Nama", trxKunjungan.OrangID);
            ViewBag.OrangID = data.Orang.ID;
            ViewBag.LokasiklinikID = new SelectList(db.Klinik, "ID", "Nama", data.LokasiklinikID);
            ViewBag.NamaOrang = data.Orang.Nama;
            return View(kunjungan);
        }

        // POST: KunjunganPasien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tanggal,OrangID,NoUrut,StatusPanggil,Ket,LokasiklinikID")] SipoliDev5.Models.Kunjungan trxKunjungan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trxKunjungan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrangID = new SelectList(db.Orang, "ID", "Nama", trxKunjungan.OrangID);
            return View(trxKunjungan);
        }

        // GET: KunjunganPasien/Delete/5
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SipoliDev5.Models.Kunjungan trxKunjungan = db.Kunjungan.Find(id);
            if (trxKunjungan == null)
            {
                return HttpNotFound();
            }
            db.Kunjungan.Remove(trxKunjungan);
            db.SaveChanges();
            return RedirectToAction("DaftarKunjunganPasienIndex");
            //return View(trxKunjungan);
        }

        //AWAL
        // POST: KunjunganPasien/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trxKunjungan trxKunjungan = db.trxKunjungan.Find(id);
            db.trxKunjungan.Remove(trxKunjungan);
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
