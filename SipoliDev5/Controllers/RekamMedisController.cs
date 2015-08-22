using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SipoliDev5.Models;
using SipoliDev5.Models.ViewModels;

using PagedList;
using PagedList.Mvc;
using Rotativa;

namespace SipoliDev5.Models
{
    [Authorize]
    public class RekamMedisController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: /RekamMedis/
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DaftarRekamMedis(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


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

            var j = 1;
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j,itemData.ID,itemData.Nama,itemData.TanggalLahir,
                    itemData.TempatLahir,itemData.JenisKelaminID,null,null,null,null,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }


        public PartialViewResult DaftarRekamMedisMahasiswaDiploma(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Mh in db.MahasiswaDiploma on Rm.PasienID equals Mh.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NIM = Mh.NIM,
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
            
            
            var j = 1;
            
            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j, itemData.ID, itemData.Nama, itemData.TanggalLahir, itemData.TempatLahir,
                    itemData.JenisKelaminID, null, itemData.NIM,null,null,null));
                j++;
            }
 

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }


        public PartialViewResult DaftarRekamMedisMahasiswaSarjana(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Mh in db.MahasiswaSarjana on Rm.PasienID equals Mh.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NIM = Mh.NIM,
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
            

            var j = 1;

            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j, itemData.ID, itemData.Nama, itemData.TanggalLahir, itemData.TempatLahir,
                    itemData.JenisKelaminID, null, itemData.NIM,null,null,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }

        public PartialViewResult DaftarRekamMedisMahasiswaMagister(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Mh in db.MahasiswaMagister on Rm.PasienID equals Mh.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NIM = Mh.NIM,
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


            var j = 1;

            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j, itemData.ID, itemData.Nama, itemData.TanggalLahir, itemData.TempatLahir,
                    itemData.JenisKelaminID, null, itemData.NIM,null,null,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }

        public PartialViewResult DaftarRekamMedisMahasiswaDoktor(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Mh in db.MahasiswaDoktor on Rm.PasienID equals Mh.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NIM = Mh.NIM,
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


            var j = 1;

            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j, itemData.ID, itemData.Nama, itemData.TanggalLahir, itemData.TempatLahir,
                    itemData.JenisKelaminID, null, itemData.NIM,null,null,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }

        [Authorize(Roles="Admin,Staf,Dokter,Pemeriksa")]
        public PartialViewResult _RmSeseorang(int? id, int? page)
        {
            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Pm in db.mstDokter on Rm.DokterID equals Pm.PegawaiID
                         where Rm.PasienID == id
                         select new
                         {
                             ID = Rm.ID,
                             Tanggal = Rm.Tanggal,
                             AnamnesaDiagnosa = Rm.AnamnesaDiagnosa,
                             Therapie = Rm.Therapie,
                             NamaDokter = Rm.Pegawai.Orang.Nama,
                             GelarDepan = Rm.Pegawai.GelarDepan,
                             GelarBelakang = Rm.Pegawai.GelarBelakang
                         }).OrderByDescending(t => t.Tanggal);

            ViewBag.PasienID = id;

            List<RekamMedis> data2 = new List<RekamMedis>();

            var j = 1;
            foreach (var item in data1)
            {
                data2.Add(new RekamMedis
                {
                    No = j,
                    ID = item.ID,
                    Tanggal = (DateTime)item.Tanggal,
                    AnamnesaDiagnosa = item.AnamnesaDiagnosa,
                    Therapie = item.Therapie,
                    NamaDokter = item.NamaDokter,
                    GelarDepan = item.GelarDepan,
                    GelarBelakang = item.GelarBelakang
                });
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 7));

        }

        [Authorize(Roles="Admin,Staf,Dokter,Pemeriksa")]
        public ActionResult DetailRekamMedis(int? id, int? page)
        {
            ViewBag.id = id;
            ViewBag.page = page;

            return View();
        }

        //Untuk cetak Detail RM Pasien
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult DetailRMTercetak(int? id, int? page)
        {
            ViewBag.id = id;
            ViewBag.page = page;

            return new ViewAsPdf("DetailRMTercetak","Empty",null);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public PartialViewResult _IdentitasPasienCetak(int? id)
        {

            var data1 = (from Or in db.Orang
                         select new
                         {
                             ID = (int)Or.ID,
                             Nama = Or.Nama,
                             TempatLahir = Or.TempatLahir,
                             TanggalLahir = Or.TanggalLahir,
                             AlamatAsal = Or.TempatLahir,
                             JenisKelaminID = Or.JenisKelaminID
                         }).Single(s => s.ID == id);


            //id Orang
            ViewBag.id = id;

            var data1ViewModel = new RekamMedisIdentitasPasien(data1.ID, data1.Nama, data1.TempatLahir,
                data1.TanggalLahir, data1.AlamatAsal, HitungUmur(data1.TanggalLahir.Year, data1.TanggalLahir.Month, data1.TanggalLahir.Day), data1.JenisKelaminID);


            return PartialView(data1ViewModel);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult _RiwayatPenyakitCetak(int? id)
        {
            //var riwayatpenyakits = db.RiwayatPenyakit.Where(pass => pass.PasienID == PasienID);

            ViewBag.id = id;

            var dataPenyakit = from p in db.RiwayatPenyakit
                               where p.PasienID == id
                               select new
                               {
                                   ID = p.ID,
                                   Tahun = p.Tahun,
                                   PenyakitID = p.PenyakitID,
                                   NamaPenyakit = p.Penyakit.Nama
                               };
            List<RekamMedisRiwayatPenyakit> listRiwayatPenyakit = new List<RekamMedisRiwayatPenyakit>();
            var j = 1;
            foreach (var item in dataPenyakit)
            {
                listRiwayatPenyakit.Add(new RekamMedisRiwayatPenyakit
                {
                    No = j,
                    ID = item.ID,
                    Tahun = item.Tahun,
                    PenyakitID = item.PenyakitID,
                    NamaPenyakit = item.NamaPenyakit
                });
                j++;
            }


            return PartialView(listRiwayatPenyakit);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult _RiwayatPenyakitKeluargaCetak(int? id)
        {
            ViewBag.id = id;

            var dataPenyakit = from p in db.RiwayatPenyakitKeluarga
                               where p.PasienID == id
                               select new
                               {
                                   ID = p.ID,
                                   hubID = p.HubunganKeluargaID,
                                   NamaHubungan = p.HubunganKeluarga.Nama,
                                   penyakitID = p.PenyakitID,
                                   NamaPenyakit = p.Penyakit.Nama

                               };
            List<RekamMedisRiwayatPenyakitKeluarga> listRiwayatPenyakitKeluarga = new List<RekamMedisRiwayatPenyakitKeluarga>();
            var j = 1;
            foreach (var item in dataPenyakit)
            {
                listRiwayatPenyakitKeluarga.Add(new RekamMedisRiwayatPenyakitKeluarga
                {
                    No = j,
                    ID = item.ID,
                    HubunganKeluargaID = item.hubID,
                    NamaHubungan = item.NamaHubungan,
                    PenyakitID = item.penyakitID,
                    NamaPenyakit = item.NamaPenyakit
                });
                j++;
            }

            return PartialView(listRiwayatPenyakitKeluarga);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult _KelainanBawaanCetak(int? id)
        {
            ViewBag.id = id;

            var kelainanBawaan = from k in db.KelainanBawaan
                                 join kk in db.KelainanBawaan1 on k.KelainanBawaanID equals kk.ID
                                 where k.PasienID == id
                                 select new
                                 {
                                     ID = k.ID,
                                     PasienID = k.PasienID,
                                     KelainanBawaanID = k.KelainanBawaanID,
                                     NamaKelainan = k.KelainanBawaan1.Nama,
                                     NamaIlmiah = k.KelainanBawaan1.NamaIlmiah
                                 };

            List<RekamMedisKelainanBawaan> listKelainan = new List<RekamMedisKelainanBawaan>();
            var j = 1;
            foreach (var item in kelainanBawaan)
            {
                listKelainan.Add(new RekamMedisKelainanBawaan
                {
                    No = j,
                    PasienID = item.PasienID,
                    KelainanBawaanID = item.KelainanBawaanID,
                    NamaKelainanBawaan = item.NamaKelainan,
                    NamaIlmiah = item.NamaIlmiah
                });
                j++;
            };

            //var kelainanBawaan = db.KelainanBawaan.Where(pass => pass.PasienID == id);

            return PartialView(listKelainan);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public PartialViewResult _RmSeseorangCetak(int? id, int? page)
        {
            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join Pm in db.mstDokter on Rm.DokterID equals Pm.PegawaiID
                         where Rm.PasienID == id
                         select new
                         {
                             ID = Rm.ID,
                             Tanggal = Rm.Tanggal,
                             AnamnesaDiagnosa = Rm.AnamnesaDiagnosa,
                             Therapie = Rm.Therapie,
                             NamaDokter = Rm.Pegawai.Orang.Nama,
                             GelarDepan = Rm.Pegawai.GelarDepan,
                             GelarBelakang = Rm.Pegawai.GelarBelakang
                         }).OrderByDescending(t => t.Tanggal);

            ViewBag.PasienID = id;

            List<RekamMedis> data2 = new List<RekamMedis>();

            var j = 1;
            foreach (var item in data1)
            {
                data2.Add(new RekamMedis
                {
                    No = j,
                    ID = item.ID,
                    Tanggal = (DateTime)item.Tanggal,
                    AnamnesaDiagnosa = item.AnamnesaDiagnosa,
                    Therapie = item.Therapie,
                    NamaDokter = item.NamaDokter,
                    GelarDepan = item.GelarDepan,
                    GelarBelakang = item.GelarBelakang
                });
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 7));

        }


        public PartialViewResult DaftarRekamMedisDosen(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join p in db.Pegawai on Or.ID equals p.ID
                         join Ds in db.Dosen on p.ID equals Ds.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NoKTP = p.NomorKTP,
                             NIDN = Ds.NIDN,
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


            var j = 1;
            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j,itemData.ID,itemData.Nama,itemData.TanggalLahir,
                    itemData.TempatLahir,itemData.JenisKelaminID,null,null,itemData.NIDN,itemData.NoKTP,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }

        public PartialViewResult DaftarRekamMedisTendik(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.JkSortParm = sortOrder == "JK" ? "jk_desc" : "JK";


            var data1 = (from Rm in db.RekamMedik
                         join Or in db.Orang on Rm.PasienID equals Or.ID
                         join p in db.Pegawai on Or.ID equals p.ID
                         join Tn in db.Tendik on p.ID equals Tn.ID
                         select new
                         {
                             ID = Or.ID,
                             Nama = Or.Nama,
                             NoKTP = p.NomorKTP,
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


            var j = 1;
            List<DaftarPasien> data2 = new List<DaftarPasien>();
            foreach (var itemData in data1)
            {
                data2.Add(new DaftarPasien(j,itemData.ID,itemData.Nama,itemData.TanggalLahir,itemData.TempatLahir,
                    itemData.JenisKelaminID,null,null,null,itemData.NoKTP,null));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 10));
        }


        public JsonResult Search(string term)
        {
            var result = (from r in db.Orang
                          where r.Nama.ToLower().Contains(term.ToLower())
                          select new { label = r.Nama, value = r.Nama, id = r.ID });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles="Admin,Staf,Pemeriksa,Dokter")]
        public PartialViewResult _IdentitasPasien(int? id)
        {
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             */

            var data1 = (from Or in db.Orang
                         select new
                         {
                             ID = (int)Or.ID,
                             Nama = Or.Nama,
                             TempatLahir = Or.TempatLahir,
                             TanggalLahir = Or.TanggalLahir,
                             AlamatAsal = Or.TempatLahir,
                             JenisKelaminID = Or.JenisKelaminID
                         }).Single(s => s.ID == id);


            //id Orang
            ViewBag.id = id;

            var data1ViewModel = new RekamMedisIdentitasPasien(data1.ID, data1.Nama, data1.TempatLahir,
                data1.TanggalLahir, data1.AlamatAsal, HitungUmur(data1.TanggalLahir.Year, data1.TanggalLahir.Month, data1.TanggalLahir.Day), data1.JenisKelaminID);

            /*
            if (data1 == null)
            {
                return HttpNotFound();
            }
              */

            return PartialView(data1ViewModel);
        }

        public int HitungUmur(int? tahun, int? bulan, int? hari)
        {
            DateTime now = DateTime.Now;
            int years = now.Year - tahun.Value;
            if (now.Month < bulan.Value || (now.Month == bulan.Value && now.Day < hari.Value)) years--;

            return years;
        }

        //_RiwayatPenyakit
        [Authorize(Roles = "Admin,Staf,Pemeriksa,Dokter")]
        public ActionResult _RiwayatPenyakit(int? id)
        {
            //var riwayatpenyakits = db.RiwayatPenyakit.Where(pass => pass.PasienID == PasienID);

            ViewBag.id = id;

            var dataPenyakit = from p in db.RiwayatPenyakit
                               where p.PasienID == id
                               select new
                               {
                                   ID = p.ID,
                                   Tahun = p.Tahun,
                                   PenyakitID = p.PenyakitID,
                                   NamaPenyakit = p.Penyakit.Nama
                               };
            List<RekamMedisRiwayatPenyakit> listRiwayatPenyakit = new List<RekamMedisRiwayatPenyakit>();
            var j = 1;
            foreach (var item in dataPenyakit)
            {
                listRiwayatPenyakit.Add(new RekamMedisRiwayatPenyakit
                {
                    No = j,
                    ID = item.ID,
                    Tahun = item.Tahun,
                    PenyakitID = item.PenyakitID,
                    NamaPenyakit = item.NamaPenyakit
                });
                j++;
            }


            return PartialView("_RiwayatPenyakit", listRiwayatPenyakit);
        }

        
        public PartialViewResult _FormRiwayatPenyakit(int? id)
        {
            ViewBag.id = id;

            //list penyakit - DPList
            var penyakitQuery = from p in db.Penyakit
                                select new
                                {
                                    ID = p.ID,
                                    Nama = p.Nama
                                };
            List<SelectListItem> listPenyakit = new List<SelectListItem>();
            foreach (var item in penyakitQuery)
            {
                listPenyakit.Add(new SelectListItem { Text = item.Nama, Value = item.ID.ToString() });
            }
            ViewData["isiPenyakitList"] = listPenyakit;
            //end

            return PartialView();
        }

        //coba GOOD
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRiwayatPenyakit([Bind(Include = "ID,PasienID,PenyakitID,Tahun")] RiwayatPenyakit riwayatPenyakit)
        {
            if (ModelState.IsValid)
            {
                db.RiwayatPenyakit.Add(riwayatPenyakit);
                db.SaveChanges();
            }
            var id = riwayatPenyakit.PasienID;

            return RedirectToAction("_RiwayatPenyakit", new { id = id });
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        public ActionResult DeleteRiwayatPenyakit(int id)
        {
            RiwayatPenyakit riwayatPenyakit = db.RiwayatPenyakit.Find(id);

            var PasienID = riwayatPenyakit.PasienID;

            db.RiwayatPenyakit.Remove(riwayatPenyakit);
            db.SaveChanges();
            return RedirectToAction("_RiwayatPenyakit", new { id = PasienID });
        }


        //_RiwayatPenyakitKeluarga
        [Authorize(Roles = "Admin,Staf,Dokter,Pemeriksa")]
        public ActionResult _RiwayatPenyakitKeluarga(int? id)
        {
            ViewBag.id = id;

            var dataPenyakit = from p in db.RiwayatPenyakitKeluarga
                               where p.PasienID == id
                               select new
                               {
                                   ID = p.ID,
                                   hubID = p.HubunganKeluargaID,
                                   NamaHubungan = p.HubunganKeluarga.Nama,
                                   penyakitID = p.PenyakitID,
                                   NamaPenyakit = p.Penyakit.Nama

                               };
            List<RekamMedisRiwayatPenyakitKeluarga> listRiwayatPenyakitKeluarga = new List<RekamMedisRiwayatPenyakitKeluarga>();
            var j = 1;
            foreach (var item in dataPenyakit)
            {
                listRiwayatPenyakitKeluarga.Add(new RekamMedisRiwayatPenyakitKeluarga
                {
                    No = j,
                    ID = item.ID,
                    HubunganKeluargaID = item.hubID,
                    NamaHubungan = item.NamaHubungan,
                    PenyakitID = item.penyakitID,
                    NamaPenyakit = item.NamaPenyakit
                });
                j++;
            }


            return PartialView("_RiwayatPenyakitKeluarga", listRiwayatPenyakitKeluarga);
        }

       
        public PartialViewResult _FormRiwayatPenyakitKeluarga(int? id)
        {
            ViewBag.id = id;

            //list keluarga - DPList
            var hubunganQuery = from k in db.HubunganKeluarga
                                select new
                                {
                                    ID = k.ID,
                                    Nama = k.Nama
                                };
            List<SelectListItem> listHubungan = new List<SelectListItem>();
            foreach (var item in hubunganQuery)
            {
                listHubungan.Add(new SelectListItem { Text = item.Nama, Value = item.ID.ToString() });
            }
            ViewData["isiHubunganList"] = listHubungan;
            //end

            //list penyakit - DPList
            var penyakitQuery = from p in db.Penyakit
                                select new
                                {
                                    ID = p.ID,
                                    Nama = p.Nama
                                };
            List<SelectListItem> listPenyakit = new List<SelectListItem>();
            foreach (var item in penyakitQuery)
            {
                listPenyakit.Add(new SelectListItem { Text = item.Nama, Value = item.ID.ToString() });
            }
            ViewData["isiPenyakitList"] = listPenyakit;
            //end

            return PartialView();
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRiwayatPenyakitKeluarga([Bind(Include = "ID,PasienID,HubunganKeluargaID,PenyakitID")] RiwayatPenyakitKeluarga riwayatPenyakitKeluarga)
        {
            if (ModelState.IsValid)
            {
                db.RiwayatPenyakitKeluarga.Add(riwayatPenyakitKeluarga);
                db.SaveChanges();
            }
            var id = riwayatPenyakitKeluarga.PasienID;

            return RedirectToAction("_RiwayatPenyakitKeluarga", new { id = id });
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        public ActionResult DeleteRiwayatPenyakitKeluarga(int id)
        {
            RiwayatPenyakitKeluarga riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Find(id);

            var PasienID = riwayatPenyakitKeluarga.PasienID;

            db.RiwayatPenyakitKeluarga.Remove(riwayatPenyakitKeluarga);
            db.SaveChanges();
            return RedirectToAction("_RiwayatPenyakitKeluarga", new { id = PasienID });
        }



        //_KelainanBawaan
        [Authorize(Roles = "Admin,Staf,Pemeriksa,Dokter")]
        public ActionResult _KelainanBawaan(int? id)
        {
            ViewBag.id = id;

            var kelainanBawaan = from k in db.KelainanBawaan
                                 join kk in db.KelainanBawaan1 on k.KelainanBawaanID equals kk.ID
                                 where k.PasienID == id
                                 select new
                                 {
                                     ID = k.ID,
                                     PasienID = k.PasienID,
                                     KelainanBawaanID = k.KelainanBawaanID,
                                     NamaKelainan = k.KelainanBawaan1.Nama,
                                     NamaIlmiah = k.KelainanBawaan1.NamaIlmiah
                                 };

            List<RekamMedisKelainanBawaan> listKelainan = new List<RekamMedisKelainanBawaan>();
            var j = 1;
            foreach (var item in kelainanBawaan)
            {
                listKelainan.Add(new RekamMedisKelainanBawaan
                {
                    No = j,
                    PasienID = item.PasienID,
                    KelainanBawaanID = item.KelainanBawaanID,
                    NamaKelainanBawaan = item.NamaKelainan,
                    NamaIlmiah = item.NamaIlmiah
                });
                j++;
            };

            //var kelainanBawaan = db.KelainanBawaan.Where(pass => pass.PasienID == id);

            return PartialView("_KelainanBawaan", listKelainan);
        }

        
        public PartialViewResult _FormKelainanBawaan(int? id)
        {
            ViewBag.id = id;

            //list kelaian - DPList
            var kelainanQuery = from k in db.KelainanBawaan1
                                select new
                                {
                                    KelainanBawaanID = k.ID,
                                    NamaKelainan = k.Nama + " (" + k.NamaIlmiah + ")"
                                };
            List<SelectListItem> listKelainan = new List<SelectListItem>();
            foreach (var item in kelainanQuery)
            {
                listKelainan.Add(new SelectListItem { Text = item.NamaKelainan, Value = item.KelainanBawaanID.ToString() });
            }
            ViewData["isiKelainanList"] = listKelainan;
            //end


            return PartialView();
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateKelainanBawaan([Bind(Include = "ID,PasienID,KelainanBawaanID")] KelainanBawaan kelainanBawaan)
        {
            if (ModelState.IsValid)
            {
                db.KelainanBawaan.Add(kelainanBawaan);
                db.SaveChanges();
            }
            var id = kelainanBawaan.PasienID;

            return RedirectToAction("_KelainanBawaan", new { id = id });
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        public ActionResult DeleteKelainanBawaan(int id)
        {
            KelainanBawaan kelainanBawaan = db.KelainanBawaan.Find(id);

            var PasienID = kelainanBawaan.PasienID;

            db.KelainanBawaan.Remove(kelainanBawaan);
            db.SaveChanges();
            return RedirectToAction("_KelainanBawaan", new { id = PasienID });
        }


        // GET: /RekamMedis/Details/5
        public ActionResult Details(int? id, int? idPasien)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RekamMedik rekammedik = db.RekamMedik.Find(id);

            //opt
            //ViewBag.IdPasien = id;
            //ViewBag.NamaPasien = db.Orangs.Find(idPasien).Nama;

            if (rekammedik == null)
            {
                return HttpNotFound();
            }
            return View(rekammedik);
        }

        [Authorize(Roles = "Admin,Staf,Dokter,Pemeriksa")]
        public ActionResult ResepIndex(int? id, int? idPasien)
        {
            ViewBag.id = id;
            ViewBag.idPasien = idPasien;

            return View();
        }

        public PartialViewResult DetailHasilPeriksa(int? id, int? idPasien)
        {
            
            RekamMedik rekammedik = db.RekamMedik.Find(id);

            return PartialView(rekammedik);
        }

        [Authorize(Roles = "Admin,Pemeriksa,Dokter")]
        public PartialViewResult _ResepObat(int? id, int? page)
        {
            var data1 = (from Rm in db.RekamMedik
                         join rs in db.ResepObat on Rm.ID equals rs.RekamMedikID
                         join ob in db.Obat on rs.ObatID equals ob.ID
                         join sto in db.SatuanObat on rs.SatuanObatID equals sto.ID
                         where Rm.ID == id
                         select new
                         {
                             RekamMedisID = Rm.ID,
                             ID = rs.ID,
                             ObatID = ob.ID,
                             NamaObat = ob.Nama,
                             SatuanObatID = sto.ID,
                             NamaSatuanObat = sto.Nama,
                             Jumlah = rs.Jumlah,
                             Pemakaian = rs.Pemakaian,
                             isDihabiskan = rs.isDihabiskan,
                             isSetelahMakan = rs.isSetelahMakan
                         }).OrderByDescending(t => t.ID);

            ViewBag.ID = id;

            List<ResepObatView> data2 = new List<ResepObatView>();

            var j = 1;
            foreach (var item in data1)
            {
                data2.Add(new ResepObatView(j,item.ID,item.RekamMedisID,item.ObatID,item.NamaObat,item.Jumlah,
                    item.SatuanObatID,item.NamaSatuanObat,item.Pemakaian,item.isDihabiskan,item.isSetelahMakan));
                j++;
            }

            return PartialView(data2.ToList().ToPagedList(page ?? 1, 7));

        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public PartialViewResult _CreateDataResep(int? id)
        {
            ViewBag.RekamMedikID = id;
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama");
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama");

            var selectListItemDihabiskan = new List<SelectListItem>();
            selectListItemDihabiskan.Add(new SelectListItem { Text = "Ya", Value = "true" });
            selectListItemDihabiskan.Add(new SelectListItem { Text = "Tidak", Value = "false" });
            ViewBag.selectDihabiskan = new SelectList(selectListItemDihabiskan, "Value", "Text");

            var selectListItemSetelahMakan = new List<SelectListItem>();
            selectListItemSetelahMakan.Add(new SelectListItem { Text = "Ya", Value = "true" });
            selectListItemSetelahMakan.Add(new SelectListItem { Text = "Tidak", Value = "false" });
            ViewBag.selectSetelahMakan = new SelectList(selectListItemDihabiskan, "Value", "Text");

            return PartialView();
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateDataResep([Bind(Include="ID,RekamMedikID,ObatID,Jumlah,SatuanObatID,Pemakaian,isDihabiskan,isSetelahMakan")] ResepObat resepObat){
            if(ModelState.IsValid){
                db.ResepObat.Add(resepObat);
                db.SaveChanges();
            }

            var id = resepObat.RekamMedikID;

            return RedirectToAction("ResepIndex", new { id = id });
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult EditResepObat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resepObat = db.ResepObat.Find(id);
            if (resepObat == null)
            {
                return HttpNotFound();
            }
            ResepObatView resepObatEdit = new ResepObatView(null, resepObat.ID, (int)resepObat.RekamMedikID, (int)resepObat.ObatID, null, resepObat.Jumlah, (int)resepObat.SatuanObatID,
                null, resepObat.Pemakaian, resepObat.isDihabiskan, resepObat.isSetelahMakan);

            ViewBag.id = resepObat.RekamMedikID;
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", resepObat.ObatID);
            ViewBag.SatuanObatID = new SelectList(db.SatuanObat, "ID", "Nama", resepObat.SatuanObatID);

            var selectListItemDihabiskan = new List<SelectListItem>();
            selectListItemDihabiskan.Add(new SelectListItem { Text = "Ya", Value = "true" });
            selectListItemDihabiskan.Add(new SelectListItem { Text = "Tidak", Value = "false" });
            ViewBag.selectDihabiskan = new SelectList(selectListItemDihabiskan, "Value", "Text");

            var selectListItemSetelahMakan = new List<SelectListItem>();
            selectListItemSetelahMakan.Add(new SelectListItem { Text = "Ya", Value = "true" });
            selectListItemSetelahMakan.Add(new SelectListItem { Text = "Tidak", Value = "false" });
            ViewBag.selectSetelahMakan = new SelectList(selectListItemDihabiskan, "Value", "Text");

            return View(resepObatEdit);
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditResepObat([Bind(Include = "ID,RekamMedikID,ObatID,Jumlah,SatuanObatID,Pemakaian,isDihabiskan,isSetelahMakan")] ResepObat resepObat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resepObat).State = EntityState.Modified;
                db.SaveChanges();
            }
            var id = resepObat.RekamMedikID;

            return RedirectToAction("ResepIndex", new { id = id });
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        public ActionResult DeleteResepObat(int id, int RekamMedikID)
        {
            ResepObat resepObat = db.ResepObat.Find(id);
            
            db.ResepObat.Remove(resepObat);
            db.SaveChanges();
            return RedirectToAction("_ResepObat", new { id = RekamMedikID});
        }


        // GET: /RekamMedis/Create
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Create(int? id)
        {

            //id orang untuk di field hidden form pemeriksaan baru
            ViewBag.id = id;
            ViewBag.NamaPasien = db.Orang.Find(id).Nama;

            ViewBag.LokasiklinikID = new SelectList(db.Klinik, "ID", "Nama");

            //mengumpulkan nama Dokter
            PopulateDokterDropdownList();
            //ViewBag.DokterID = new SelectList(dokter, "ID", "GelarDanNama");

            return View(new RekamMedis
            {
                Tanggal = DateTime.Now
            });
        }

        private void PopulateDokterDropdownList(object selectedDokter = null)
        {
            var dokter = from d in db.mstDokter
                         where d.StatusAktif == true
                         select new Dokter()
                         {
                             ID = d.Pegawai.ID,
                             Nama = d.Pegawai.Orang.Nama,
                             GelarDepan = d.Pegawai.GelarDepan,
                             GelarDanNama = d.Pegawai.GelarDepan + d.Pegawai.Orang.Nama
                         };

            ViewBag.DokterID = new SelectList(dokter, "ID", "GelarDanNama", selectedDokter);
        }

        // POST: /RekamMedis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PasienID,Tanggal,AnamnesaDiagnosa,Therapie,DokterID,LokasiklinikID")] RekamMedik rekammedik)
        {
            if (ModelState.IsValid)
            {
                db.RekamMedik.Add(rekammedik);
                db.SaveChanges();
                var id = rekammedik.PasienID;

                return RedirectToAction("DetailRekamMedis", new { id = id });
            }

            //ViewBag.PasienID = new SelectList(db.Orangs, "ID", "Nama", rekammedik.PasienID);
            //ViewBag.DokterID = new SelectList(db.Pegawais, "ID", "GelarDepan", rekammedik.DokterID);

            PopulateDokterDropdownList(rekammedik.DokterID);

            return View(rekammedik);
        }

        // GET: /RekamMedis/Edit/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.RekamMedik.Find(id);

            //Hide
            ViewBag.IdPasien = data.PasienID;
            ViewBag.NamaPasien = data.Orang.Nama;
            ViewBag.LokasiklinikID = new SelectList(db.Klinik, "ID", "Nama", data.LokasiklinikID);

            if (data == null)
            {
                return HttpNotFound();
            }

            RekamMedisConst rekamMedis = new RekamMedisConst(data.ID,data.LokasiklinikID, (int)data.PasienID, (DateTime)data.Tanggal,
                data.AnamnesaDiagnosa, data.Therapie, (int)data.DokterID);

            //ViewBag.PasienID = new SelectList(db.Orangs, "ID", "Nama", rekammedik.PasienID);
            //ViewBag.DokterID = new SelectList(dokter, "ID", "GelarDanNama");
            PopulateDokterDropdownList(data.Pegawai.ID);

            return View(rekamMedis);
        }

        // POST: /RekamMedis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PasienID,Tanggal,AnamnesaDiagnosa,Therapie,DokterID")] RekamMedik rekammedik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rekammedik).State = EntityState.Modified;
                db.SaveChanges();

                var id = rekammedik.PasienID;
                return RedirectToAction("DetailRekamMedis", new { id = id });
            }

            //ViewBag.PasienID = new SelectList(db.Orangs, "ID", "Nama", rekammedik.PasienID);
            ViewBag.DokterID = new SelectList(db.Pegawai, "ID", "GelarDepan", rekammedik.DokterID);
            return View(rekammedik);
        }

        // GET: /RekamMedis/Delete/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Delete(int? id, int? OrangID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //cek rujukan
            var rujukan = db.Rujukan.Where(r => r.RekamMedikID == id);
            if (rujukan != null)
            {
                db.Rujukan.RemoveRange(rujukan);
                db.SaveChanges();
            }

            //cek resepObat
            var resepObat = db.ResepObat.Where(p => p.RekamMedikID == id).ToList();
            if (resepObat != null)
            {
                db.ResepObat.RemoveRange(resepObat);
                db.SaveChanges();
            }

            var rekammedis1 = db.RekamMedik.Find(id);
            if (rekammedis1 == null)
            {
                return HttpNotFound();
            }
            db.RekamMedik.Remove(rekammedis1);
            db.SaveChanges();

            return RedirectToAction("_RmSeseorang", new { id = OrangID });
            //return View(rekammedis1);
        }


        /*AWAL
        // POST: /RekamMedis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RekamMedik rekammedik = db.RekamMedik.Find(id);
            db.RekamMedik.Remove(rekammedik);
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
