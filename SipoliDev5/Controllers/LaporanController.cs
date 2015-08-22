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

namespace SipoliDev5.Controllers
{
    [Authorize(Roles = "Admin,Staf,Pemeriksa")]
    public class LaporanController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: Laporan
        public ActionResult Index()
        {
            return View();
        }

        /*
        public ActionResult _LaporanDataPasien(string term)
        {
            IQueryable<DaftarPasien> data = (from Rm in db.RekamMedik
                                             join Or in db.Orang on Rm.PasienID equals Or.ID
                                             select new DaftarPasien()
                                             {
                                                 ID = Or.ID,
                                                 Nama = Or.Nama,
                                                 TempatLahir = Or.TempatLahir,
                                                 JenisKelaminID = Or.JenisKelaminID,
                                                 TanggalLahir = Or.TanggalLahir,
                                                 //Age = HitungUmur(Or.TanggalLahir),
                                                 Age = DateTime.Now.Year - Or.TanggalLahir.Year,
                                                 TanggalRegis = Rm.Tanggal.Value
                                             }).Distinct();

            if (!String.IsNullOrEmpty(term))
            {
                data = data.Where(s => s.Nama.Contains(term));
            }

            return PartialView("_LaporanDataPasien", data.ToList());
        }
         */

        //public static string HitungUmur(DateTime TanggalLahir)
        //{
        //    DateTime now = DateTime.Now;
        //    int years = now.Year - TanggalLahir.Year;
        //    if (now.Month < TanggalLahir.Month || (now.Month == TanggalLahir.Month && now.Day < TanggalLahir.Day)) years--;

        //    return years.ToString();
        //}

    }
}