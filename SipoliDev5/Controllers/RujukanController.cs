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
using System.Data.Entity.Infrastructure;
using Rotativa;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class RujukanController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        private List<RujukanView> listRujukan = new List<RujukanView>();

        // GET: Rujukan
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DaftarRujukan(string sortOrder, string searchString, string currentFilter, int? page)
        {
           

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var rekamMedikRujukan = from Rm in db.RekamMedik
                                    join Rj in db.Rujukan on Rm.ID equals Rj.RekamMedikID
                                    join Rs in db.RumahSakit on Rj.RumahSakitID equals Rs.ID
                                    select new
                                    {
                                        ID = Rm.ID,
                                        Tanggal = Rm.Tanggal,
                                        AnamnesaDiagnosa = Rm.AnamnesaDiagnosa,
                                        DokterID = Rm.DokterID,
                                        NamaGelarDokter = Rm.Pegawai.GelarDepan + Rm.Pegawai.Orang.Nama + Rm.Pegawai.GelarBelakang,
                                        NamaPasien = Rm.Orang.Nama,
                                        NamaRumahSakit = Rm.Rujukan.RumahSakit.Nama,
                                        Bagian = Rm.Rujukan.Bagian
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
                rekamMedikRujukan = rekamMedikRujukan.Where(s => s.NamaPasien.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    rekamMedikRujukan = rekamMedikRujukan.OrderByDescending(s => s.NamaPasien);
                    break;
                case "Date":
                    rekamMedikRujukan = rekamMedikRujukan.OrderBy(s => s.Tanggal);
                    break;
                case "date_desc":
                    rekamMedikRujukan = rekamMedikRujukan.OrderByDescending(s => s.Tanggal);
                    break;
                default:
                    rekamMedikRujukan = rekamMedikRujukan.OrderBy(s => s.NamaPasien);
                    break;
            }

            var j = 1;
            foreach (var itemData in rekamMedikRujukan)
            {
                listRujukan.Add(new RujukanView(j, itemData.ID, itemData.Tanggal, itemData.AnamnesaDiagnosa,
                    itemData.DokterID, itemData.NamaGelarDokter, itemData.NamaPasien,
                    itemData.NamaRumahSakit, itemData.Bagian));
                j++;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return PartialView(listRujukan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Rujukan/Details/5
        /*
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekamMedik = db.RekamMedik.Find(id);
            if (rekamMedik == null)
            {
                return HttpNotFound();
            }
            return View(rekamMedik);
        }
         */ 

        // GET: Rujukan/Cetak - 30/04
        public ActionResult Cetak(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekamMedik = db.RekamMedik.Find(id);
            if (rekamMedik == null)
            {
                return HttpNotFound();
            }

            ViewBag.TanggalHari = rekamMedik.Tanggal.Value.Day;
            ViewBag.TanggalTahun = rekamMedik.Tanggal.Value.Year;
            ViewBag.UmurPasien = HitungUmur(rekamMedik.Orang.TanggalLahir.Year, rekamMedik.Orang.TanggalLahir.Month,
                rekamMedik.Orang.TanggalLahir.Day);
            //ViewBag.UmurPasien = DateTime.Now.Year - rekamMedik.Orang.TanggalLahir.Year;

            switch (rekamMedik.Tanggal.Value.Month.ToString())
            {
                case "1":
                    ViewBag.TanggalBulan = "Januari";
                    break;
                case "2":
                    ViewBag.TanggalBulan = "Februari";
                    break;
                case "3":
                    ViewBag.TanggalBulan = "Maret";
                    break;
                case "4":
                    ViewBag.TanggalBulan = "April";
                    break;
                case "5":
                    ViewBag.TanggalBulan = "Mei";
                    break;
                case "6":
                    ViewBag.TanggalBulan = "Juni";
                    break;
                case "7":
                    ViewBag.TanggalBulan = "Juli";
                    break;
                case "8":
                    ViewBag.TanggalBulan = "Agustus";
                    break;
                case "9":
                    ViewBag.TanggalBulan = "September";
                    break;
                case "10":
                    ViewBag.TanggalBulan = "Oktober";
                    break;
                case "11":
                    ViewBag.TanggalBulan = "Nopember";
                    break;
                default:
                    ViewBag.TanggalBulan = "Desember";
                    break;
            }

            return View(rekamMedik);
        }

        public int HitungUmur(int? tahun, int? bulan, int? hari)
        {
            DateTime now = DateTime.Now;
            int years = now.Year - tahun.Value;
            if (now.Month < bulan.Value || (now.Month == bulan.Value && now.Day < hari.Value)) years--;

            return years;
        }

        [Authorize(Roles="Admin,Staf,Pemeriksa")]
        public ActionResult CetakPDF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekamMedik = db.RekamMedik.Find(id);
            if (rekamMedik == null)
            {
                return HttpNotFound();
            }

            ViewBag.TanggalHari = rekamMedik.Tanggal.Value.Day;
            ViewBag.TanggalTahun = rekamMedik.Tanggal.Value.Year;
            ViewBag.UmurPasien = HitungUmur(rekamMedik.Orang.TanggalLahir.Year, rekamMedik.Orang.TanggalLahir.Month,
                rekamMedik.Orang.TanggalLahir.Day);
            //ViewBag.UmurPasien = DateTime.Now.Year - rekamMedik.Orang.TanggalLahir.Year;

            switch (rekamMedik.Tanggal.Value.Month.ToString())
            {
                case "1":
                    ViewBag.TanggalBulan = "Januari";
                    break;
                case "2":
                    ViewBag.TanggalBulan = "Februari";
                    break;
                case "3":
                    ViewBag.TanggalBulan = "Maret";
                    break;
                case "4":
                    ViewBag.TanggalBulan = "April";
                    break;
                case "5":
                    ViewBag.TanggalBulan = "Mei";
                    break;
                case "6":
                    ViewBag.TanggalBulan = "Juni";
                    break;
                case "7":
                    ViewBag.TanggalBulan = "Juli";
                    break;
                case "8":
                    ViewBag.TanggalBulan = "Agustus";
                    break;
                case "9":
                    ViewBag.TanggalBulan = "September";
                    break;
                case "10":
                    ViewBag.TanggalBulan = "Oktober";
                    break;
                case "11":
                    ViewBag.TanggalBulan = "Nopember";
                    break;
                default:
                    ViewBag.TanggalBulan = "Desember";
                    break;
            }

            return new ViewAsPdf("CetakPDF","Empty",rekamMedik);
        }

        // GET: Rujukan/Create
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Create()
        {
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama");
            ViewBag.ID = new SelectList(db.Rujukan, "RekamMedikID", "Bagian");

            //List RS - default
            //ViewBag.RumahSakitID = new SelectList(db.RumahSakit, "ID", "Nama");

            //list RS dengan populate -P1 -FAILED
            //PopulateRumahSakitDropDownList();

            //list RS untuk dL - 29/04 - NF -P2
            /**/ 
            var RsQuery = from r in db.RumahSakit
                          select new
                          {
                              ID = r.ID.ToString(),
                              Nama = r.Nama.ToString()
                          };
            List<SelectListItem> listRS = new List<SelectListItem>();
            foreach (var itemRs in RsQuery)
            {
                listRS.Add(new SelectListItem { Text = itemRs.Nama, Value = itemRs.ID });
            }
            ViewData["isiRSList"] = listRS;
             
            //end

            //list RS 2015-07-02
            //ViewBag.RumahSakitID = new SelectList(db.RumahSakit, "ID", "Nama");


            /* percobaan membuat list dokter untuk DropdownList - 28/04 - GOOD JOB */
            var dokterQuery = from d in db.mstDokter
                              join p in db.Pegawai on d.PegawaiID equals p.ID
                              select new
                              {
                                  PegawaiID = d.PegawaiID.ToString(),
                                  Nama = p.Orang.Nama.ToString(),
                                  GelarDepan = p.GelarDepan.ToString()
                              };

            List<SelectListItem> listDokter = new List<SelectListItem>();
            foreach (var itemData in dokterQuery)
            {
                listDokter.Add(new SelectListItem { Text = itemData.GelarDepan + itemData.Nama, Value = itemData.PegawaiID });
            }
            ViewData["isiDokterList"] = listDokter;
            /*end */

            return View();
        }

        /*
        public JsonResult PencarianKecamatan(string term)
        {
            var result = (from k in db.Kecamatan
                          where k.KotaKabupatenID == 122
                          select new
                          {
                              label = k.Nama,
                              value = k.Nama,
                              id = k.ID
                          }).Take(10);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
         */ 

        //create rs dengan partial view dan modal bootstrap
        /*
        [HttpGet]
        public ActionResult _CreateRS()
        {
            //list Kecamatan
            
            var kecamatanQuery = from k in db.Kecamatan
                                 where k.KotaKabupatenID == 122
                                 select k;
            ViewBag.KecamatanID = new SelectList(kecamatanQuery, "ID", "Nama");
            
              
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateRS([Bind(Include = "ID,Nama,KecamatanID,NoTelepon")] RumahSakitForm rumahSakitForm)
        {
            if (ModelState.IsValid)
            {
                RumahSakit data = new RumahSakit();

                data.ID = rumahSakitForm.ID;
                data.Nama = rumahSakitForm.Nama;
                data.KecamatanID = rumahSakitForm.KecamatanID;

                db.RumahSakit.Add(data);
                db.SaveChanges();
            }

            return RedirectToAction("Create");
        }
         */ 

        [HttpGet]
        public ActionResult CreateRS()
        {
            //list Kecamatan
            /**/
            var kecamatanQuery = from k in db.Kecamatan
                                 where k.KotaKabupatenID == 122
                                 select k;
            ViewBag.KecamatanID = new SelectList(kecamatanQuery, "ID", "Nama");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRS([Bind(Include = "ID,Nama,NoTelepon,KecamatanID")] RumahSakitForm rumahSakitForm)
        {
            if (ModelState.IsValid)
            {
                RumahSakit data = new RumahSakit();

                data.ID = rumahSakitForm.ID;
                data.Nama = rumahSakitForm.Nama;
                data.KecamatanID = rumahSakitForm.KecamatanID;

                db.RumahSakit.Add(data);
                db.SaveChanges();
                
                return RedirectToAction("Create");
            }

            var kecamatanQuery = from k in db.Kecamatan
                                 where k.KotaKabupatenID == 122
                                 select k;
            ViewBag.KecamatanID = new SelectList(kecamatanQuery, "ID", "Nama");


            return View(rumahSakitForm);
        }

        private void PopulateRumahSakitDropDownList(object selectedRumahSakit = null)
        {
            var RumahSakitQuery = from r in db.RumahSakit
                                  select r;
            ViewBag.RumahSakitID = new SelectList(RumahSakitQuery, "ID", "Nama", selectedRumahSakit);
        }

        // POST: Rujukan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,Tanggal,AnamnesaDiagnosa,DokterID,Rujukan")] RekamMedik rekamMedik)
        {
            if (ModelState.IsValid)
            {
                /*
                RekamMedik data = new RekamMedik();
                data.ID = rekamMedik.ID;
                data.PasienID = rekamMedik.PasienID;
                data.Tanggal = rekamMedik.Tanggal;
                data.AnamnesaDiagnosa = rekamMedik.AnamnesaDiagnosa;
                data.DokterID = rekamMedik.DokterID;
                data.Rujukan.RumahSakitID = rekamMedik.Rujukan.RumahSakitID;
                data.Rujukan.Bagian = rekamMedik.Rujukan.Bagian;
                */                                    
                db.RekamMedik.Add(rekamMedik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", rekamMedik.PasienID);
            //ViewBag.DokterID = new SelectList(db.Pegawai, "ID", "GelarDepan", rekamMedik.DokterID);
            ViewBag.ID = new SelectList(db.Rujukan, "RekamMedikID", "Bagian", rekamMedik.ID);
            //ViewBag.RumahSakitID = new SelectList(db.RumahSakit, "ID", "Nama");
            //list RS untuk dL - 29/04 - NF -P2
            /**/
            var RsQuery = from r in db.RumahSakit
                          select new
                          {
                              ID = r.ID.ToString(),
                              Nama = r.Nama.ToString()
                          };
            List<SelectListItem> listRS = new List<SelectListItem>();
            foreach (var itemRs in RsQuery)
            {
                listRS.Add(new SelectListItem { Text = itemRs.Nama, Value = itemRs.ID });
            }
            ViewData["isiRSList"] = listRS;
            
            //end

            //list dokter
            /* percobaan membuat list dokter untuk DropdownList - 28/04 - GOOD JOB */
            var dokterQuery = from d in db.mstDokter
                              join p in db.Pegawai on d.PegawaiID equals p.ID
                              select new
                              {
                                  PegawaiID = d.PegawaiID.ToString(),
                                  Nama = p.Orang.Nama.ToString(),
                                  GelarDepan = p.GelarDepan.ToString()
                              };

            List<SelectListItem> listDokter = new List<SelectListItem>();
            foreach (var itemData in dokterQuery)
            {
                listDokter.Add(new SelectListItem { Text = itemData.GelarDepan + itemData.Nama, Value = itemData.PegawaiID });
            }
            ViewData["isiDokterList"] = listDokter; 
            /*end */

            //Kembalikan nama pasien yg dipilih (string)
            ViewBag.PasienID2 = rekamMedik.PasienID;
            if (rekamMedik.PasienID != null)
            {
                ViewBag.NamaPasien = db.Orang.Find(rekamMedik.PasienID).Nama;
            }
            
            

            return View(rekamMedik);
        }

        private void PopulateDokterDropdownList(object selectedDokter = null)
        {
            var dokter = from d in db.mstDokter
                         select new Dokter()
                         {
                             ID = d.Pegawai.ID,
                             Nama = d.Pegawai.Orang.Nama,
                             GelarDepan = d.Pegawai.GelarDepan,
                             GelarDanNama = d.Pegawai.GelarDepan + d.Pegawai.Orang.Nama
                         };

            ViewBag.DokterID = new SelectList(dokter, "ID", "GelarDanNama", selectedDokter);
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


        // GET: Rujukan/Edit/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.RekamMedik.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            RujukanConst rujukan = new RujukanConst((int)data.ID, (int)data.PasienID,
                data.Tanggal, data.AnamnesaDiagnosa, (int)data.DokterID);

            
            //Tgl sblmnya
            //ViewBag.Tanggal = data.Tanggal.Value.Date;

            //Nama Pasien
            ViewBag.NamaPasien = data.Orang.Nama;

            //Populate Dokter
            PopulateDokterDropdownList(data.Pegawai.ID);

            //Bagian RS
            ViewBag.Bagian = data.Rujukan.Bagian;

            //Populate Rumah Sakit TUjuan
            PopulateRumahSakitDropDownList(data.Rujukan.RumahSakitID);

            ViewBag.ID = new SelectList(db.Rujukan, "RekamMedikID", "Bagian", data.ID);
            return View(rujukan);
        }


        //AWAL
        // POST: Rujukan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,Tanggal,AnamnesaDiagnosa,DokterID, trxRujukan")] RekamMedik rekamMedik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rekamMedik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PasienID = new SelectList(db.Orang, "ID", "Nama", rekamMedik.PasienID);
            ViewBag.DokterID = new SelectList(db.Pegawai, "ID", "GelarDepan", rekamMedik.DokterID);
            ViewBag.ID = new SelectList(db.trxRujukan, "RekamMedikID", "Bagian", rekamMedik.ID);
            return View(rekamMedik);
        }
         */

        //POST EDIT - NEW - 30/04
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rujukanToUpdate = db.RekamMedik
                .Include(i => i.Rujukan)
                .Where(i => i.ID == id)
                .Single();

            if (TryUpdateModel(rujukanToUpdate, "",
                new string[] { "ID", "PasienID", "Tanggal", "AnamnesaDiagnosa", "DokterID", "Rujukan" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(rujukanToUpdate.Rujukan.Bagian))
                    {
                        rujukanToUpdate.Rujukan.Bagian = null;
                    }

                    db.Entry(rujukanToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex*/)
                {
                    //log the error, uncommend dex and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try Again, and if the problem persists, see your system administrator.");
                }
            }
            return View(rujukanToUpdate);
        }

        // GET: Rujukan/Delete/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekamMedik = db.RekamMedik.Find(id);

            if (rekamMedik == null)
            {
                return HttpNotFound();
            }


            return View(rekamMedik);
        }

        // POST: Rujukan/Delete/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RekamMedik rekamMedik = db.RekamMedik.Find(id);
            db.RekamMedik.Remove(rekamMedik);
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
