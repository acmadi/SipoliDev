using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SipoliDev5.Models;
using SipoliDev5.Models.ViewModels;

namespace SipoliDev5.Controllers
{
    public class DashboardController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        //
        // GET: /Dashboard/

        public PartialViewResult _Introduce()
        {
            return PartialView("_Introduce");
        }

        public PartialViewResult _JumlahPasienTotal()
        {
            var jumlahRekamMedis = (from Rm in db.RekamMedik
                                    select new { Rm.PasienID }).Distinct().Count();

            if (jumlahRekamMedis >= 1)
            {
                ViewBag.nilai = jumlahRekamMedis;
            }
            else
            {
                ViewBag.nilai = 0;
            }

            return PartialView("_JumlahPasienTotal");
        }

        public PartialViewResult _JumlahPasienMahasiswa()
        {
            var JumlahRekamMedisMahsiswa = (from Rm in db.RekamMedik
                                            join Mh in db.Mahasiswa
                                            on Rm.PasienID equals Mh.ID
                                            select new { Rm.PasienID }).Distinct().Count();
            if (JumlahRekamMedisMahsiswa >= 1)
            {
                ViewBag.nilai = JumlahRekamMedisMahsiswa;
            }
            else
            {
                ViewBag.nilai = 0;
            }

            return PartialView("_JumlahPasienMahasiswa");
        }

        public PartialViewResult _JumlahPasienDosen()
        {
            var JumlahRekamMedisDosen = (from Rm in db.RekamMedik
                                         join Ds in db.Dosen
                                         on Rm.PasienID equals Ds.ID
                                         select new { Rm.PasienID }).Distinct().Count();
            if (JumlahRekamMedisDosen >= 1)
            {
                ViewBag.nilai = JumlahRekamMedisDosen;
            }
            else
            {
                ViewBag.nilai = 0;
            }

            return PartialView("_JumlahPasienDosen");
        }

        public PartialViewResult _JumlahPasienTendik()
        {
            var JumlahRekamMedisTendik = (from Rm in db.RekamMedik
                                          join Tn in db.Tendik
                                          on Rm.PasienID equals Tn.ID
                                          select new { Rm.PasienID }).Distinct().Count();

            if (JumlahRekamMedisTendik >= 1)
            {
                ViewBag.nilai = JumlahRekamMedisTendik;
            }
            else
            {
                ViewBag.nilai = 0;
            }

            return PartialView("_JumlahPasienTendik");
        }

        public PartialViewResult _InformasiObat()
        {
            var InfoObat = (from Ob in db.Obat
                            join St in db.SatuanObat on Ob.SatuanObatID equals St.ID
                            join Gol in db.GolonganObat on Ob.GolonganObatID equals Gol.ID
                            select Ob).Take(5);

            return PartialView("_InfoObat", InfoObat);
        }

        public PartialViewResult _StafPoliklinik()
        {
            IQueryable<Staf> data = from Or in db.Orang
                                    join Peg in db.Pegawai on Or.ID equals Peg.ID
                                    join Mut in db.MutasiPegawai on Peg.ID equals Mut.PegawaiID
                                    where Mut.StrukturOrganisasiID == 39
                                    select new Staf()
                                    {
                                        Nama = Or.Nama,
                                        GelarDepan = Peg.GelarDepan
                                    };


            return PartialView(data.First());
        }

        public PartialViewResult _StatistikPenyakit()
        {
            return PartialView();
        }


    }
}