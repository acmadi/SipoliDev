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
using System.Reflection;
using System.Data.Entity.Infrastructure;//view model


namespace SipoliDev5.Controllers
{
    public class PengadaanObatController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        public JsonResult GetDataStokObat()
        {
            var coba = from r in db.StokObat where r.KlinikID == 2 select new { Value = r.Obat.ID, Text = r.Obat.Nama, Stok = r.Stok };
            return Json(coba);
        }
        public JsonResult GetDataObat(string term)
        {
            var obat = (from r in db.StokObat
                        where r.Obat.Nama.ToLower().Contains(term.ToLower())
                        where r.KlinikID==2
                        select new { label = r.Obat.Nama, value = r.Obat.Nama, id = r.Obat.ID, stok=r.Stok, satuan=r.Obat.SatuanObat.Nama}).Distinct();//query
            return Json(obat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataPenyediaObat(string term)
        {
            var obat = (from r in db.PenyediaObat
                        where r.Nama.Contains(term.ToLower())
                        select new { label = r.Nama, value = r.Nama, id = r.ID}).Distinct();//query
            return Json(obat, JsonRequestBehavior.AllowGet);
        }

        // GET: /PengadaanObat/
        public ActionResult Index(string Obat, string PenyediaObat, DateTime? Date, int? Year, string Month, string Sortby, int? page, string Command, bool? undeleteable, string stk, string jml)
        {
            if (undeleteable == true)
            {
                ViewBag.ErrorDelete = true;
                ViewBag.PesanDelete = "Jumlah obat yang dihapus lebih besar dari jumlah stok obat saat ini.";
            }
            var pengadaanobat = from a in db.PengadaanObat
                                select new PengadaanObat_ViewModel()
                                {
                                    ID = a.ID,
                                    Tanggal = a.Tanggal,
                                    PenyediaObatID = a.PenyediaObatID,
                                    ObatID = a.ObatID,
                                    Jumlah = a.Jumlah,
                                    HargaAktual = a.HargaAktual,
                                    HET = a.HET,
                                    Obat = a.Obat.Nama,
                                    PenyediaObat = a.PenyediaObat.Nama,
                                    SatuanObat = a.Obat.SatuanObat.Nama,
                                };

            //filtering
            var SelectListYear = ((from year in db.PengadaanObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
            ViewBag.Year = new SelectList(SelectListYear);

            if (Obat != null)
            {
                pengadaanobat = pengadaanobat.Where(a => a.Obat.Contains(Obat));//paging
            }

            if (PenyediaObat != null)
            {
                pengadaanobat = pengadaanobat.Where(b => b.PenyediaObat.Contains(Obat));
            }

            if (Date != null)
            {
                pengadaanobat = pengadaanobat.Where(c => c.Tanggal == Date);
            }

            if (Year != null)
            {
                pengadaanobat = pengadaanobat.Where(d => d.Tanggal.Value.Year == Year);
            }

            if (!String.IsNullOrEmpty(Month))
            {
               var MonthInt = Int32.Parse(Month);
               pengadaanobat = pengadaanobat.Where(e => e.Tanggal.Value.Month == MonthInt);
            }

            //sorting
            ViewBag.SortTanggalParameter = string.IsNullOrEmpty(Sortby) ? "Tanggal" : "";
            ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
            ViewBag.SortPenyediaObatParameter = Sortby == "PenyediaObat" ? "PenyediaObat Desc" : "PenyediaObat";

            switch (Sortby)
            {
                case "Tanggal":
                    pengadaanobat = pengadaanobat.OrderBy(a => a.Tanggal);
                    break;
                case "Obat":
                    pengadaanobat = pengadaanobat.OrderBy(b => b.Obat);
                    break;
                case "Obat Desc":
                    pengadaanobat = pengadaanobat.OrderByDescending(b => b.Obat);
                    break;
                case "PenyediaObat":
                    pengadaanobat = pengadaanobat.OrderBy(c => c.PenyediaObat);
                    break;
                case "PenyediaObat Desc":
                    pengadaanobat = pengadaanobat.OrderByDescending(c => c.PenyediaObat);
                    break;
                default:
                    pengadaanobat = pengadaanobat.OrderByDescending(g => g.Tanggal);
                    break;
            }

            //export
            if (Command=="Export")
            {
                //var pengadaanobat = (List<PengadaanObat_ViewModel>)Session["pengadaanobats"];
                pengadaanobat.ToList<PengadaanObat_ViewModel>();
                //pengadaanobat.ToList();
                StringBuilder sb = new StringBuilder();
                var Month1 = "";
                if (pengadaanobat != null && pengadaanobat.Any())
                {
                    sb.Append("<table style='1px solid black; font-size:20px;'>");
                    sb.Append("<tr style='width:15px'></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:15px'></td>");
                    sb.Append("<td colspan='8' style='width:120px;background-color: yellow;border:1px solid black', align='center'><b>DATA PENGADAAN OBAT POLIKLINIK</b></td>");
                    sb.Append("</tr>");
                    if (!string.IsNullOrEmpty(Obat) || !string.IsNullOrEmpty(PenyediaObat) || Year != null || !string.IsNullOrEmpty(Month) || Date != null)
                    {
                        if (!string.IsNullOrEmpty(Month)) { switch (Month) { case "1":Month1 = "Januari"; break; case "2":Month1 = "Februari"; break; case "3":Month1 = "Maret"; break; case "4":Month1 = "April"; break; case "5":Month1 = "Mei"; break; case "6":Month1 = "Juni"; break; case "7":Month1 = "Juli"; break; case "8":Month1 = "Agustus"; break; case "9":Month1 = "September"; break; case "10":Month1 = "Oktober"; break; case "11":Month1 = "November"; break; case "12":Month1 = "Desember"; break; default: Month1 = "-"; break; } }
                        sb.Append("<tr>");
                        sb.Append("<td style='width:15px'></td>");
                        sb.Append("<td colspan='2' style='width:190px; border:1px solid black'><b>TANGGAL</b></td>");
                        sb.Append("<td style='width:300px; border:1px solid black'>" + Date + "</td>");
                        sb.Append("<td colspan='2'style='width:120px; border:1px solid black'><b>BULAN</b></td>");
                        sb.Append("<td colspan='3'style='width:120px; border:1px solid black'>" + Month1 + "</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='width:15px'></td>");
                        sb.Append("<td colspan='2' style='width:190px; border:1px solid black'><b>NAMA OBAT</b></td>");
                        sb.Append("<td style='width:300px; border:1px solid black'>" + Obat + "</td>");
                        sb.Append("<td colspan='2'style='width:120px; border:1px solid black'><b>TAHUN</b></td>");
                        sb.Append("<td colspan='3'style='width:120px; border:1px solid black;text-align: left;'>" + Year + "</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='width:15px'></td>");
                        sb.Append("<td colspan='2' style='width:190px; border:1px solid black'><b>PENYEDIA OBAT</b></td>");
                        sb.Append("<td style='width:120px; border:1px solid black'>" + PenyediaObat + "</td>");
                        sb.Append("<td colspan='5' style='border:1px solid black'></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr>");
                    sb.Append("<td style='width:15px'></td>");
                    sb.Append("<td style='width:35px;border:1px solid black;background-color: yellow;'><center><b>NO</b></center></td>");
                    sb.Append("<td style='width:150px;border:1px solid black;background-color: yellow;'><center><b>TANGGAL</b></center></td>");
                    sb.Append("<td style='width:300px;border:1px solid black;background-color: yellow;'><center><b>NAMA OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;border:1px solid black;background-color: yellow;'><center><b>JUMLAH</b></center></td>");
                    sb.Append("<td style='width:120px;border:1px solid black;background-color: yellow;'><center><b>SATUAN OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;border:1px solid black;background-color: yellow;'><center><b>HARGA AKTUAL</b></center></td>");
                    sb.Append("<td style='width:120px;border:1px solid black;background-color: yellow;'><center><b>HET</b></center></td>");
                    sb.Append("<td style='width:120px;border:1px solid black;background-color: yellow;'><center><b>PENYEDIA OBAT</b></center></td>");
                    sb.Append("</tr>");

                    int i = 1;
                    foreach (var result in pengadaanobat)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='width:15px'></td>");
                        sb.Append("<td style='border:1px solid black'>" + i + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.Tanggal + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.Obat + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.Jumlah + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.SatuanObat + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.HargaAktual + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.HET + "</td>");
                        sb.Append("<td style='border:1px solid black'>" + result.PenyediaObat + "</td>");
                        sb.Append("</tr>");
                        i++;
                    }
                    if (!string.IsNullOrEmpty(Obat) || !string.IsNullOrEmpty(PenyediaObat) || Year != null || !string.IsNullOrEmpty(Month) || Date != null)
                    {
                        if (!string.IsNullOrEmpty(Obat))
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               group a by new { b = Obat, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(PenyediaObat))
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (Year != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Year == Year
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Month == intmonth
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (Date != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal == Date
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(PenyediaObat))
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && Year != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.Tanggal.Value.Year == Year
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.Tanggal.Value.Month == intmonth
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && Date != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.Tanggal == Date
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }

                        if (!string.IsNullOrEmpty(PenyediaObat) && Year != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Year == Year
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(PenyediaObat) && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Month == intmonth
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(PenyediaObat) && Date != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal == Date
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (Year != null && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Month == intmonth
                                               where a.Tanggal.Value.Year == Year
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }

                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(PenyediaObat) && Year != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               where a.Tanggal.Value.Year == Year
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(PenyediaObat) && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               where a.Tanggal.Value.Month == intmonth
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(PenyediaObat) && Date != null)
                        {
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               where a.Tanggal == Date
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && Year != null && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.Tanggal.Value.Month == intmonth
                                               where a.Tanggal.Value.Year == Year
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(PenyediaObat) && Year != null && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Tanggal.Value.Year == Year
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               where a.Tanggal.Value.Month == intmonth
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        if (!string.IsNullOrEmpty(Obat) && !string.IsNullOrEmpty(PenyediaObat) && Year != null && !string.IsNullOrEmpty(Month))
                        {
                            var intmonth = int.Parse(Month);
                            pengadaanobat = (from a in db.PengadaanObat
                                               where a.Obat.Nama == Obat
                                               where a.PenyediaObat.Nama == PenyediaObat
                                               where a.Tanggal.Value.Year == Year
                                               where a.Tanggal.Value.Month == intmonth
                                               group a by new { b = a.Obat.Nama, c = a.Obat.SatuanObat.Nama } into g
                                               select new PengadaanObat_ViewModel()
                                               {
                                                   Obat = g.Key.b,
                                                   SatuanObat = g.Key.c,
                                                   Total = g.Select(m => m.Jumlah).Sum()
                                               });
                        }
                        sb.Append("<tr></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='border:1px solid black;background-color: yellow;'><center><b>NAMA OBAT</b></center></td>");
                        sb.Append("<td style='border:1px solid black;background-color: yellow;'><center><b>TOTAL</b></center></td>");
                        sb.Append("<td style='border:1px solid black;background-color: yellow;'><center><b>SATUAN OBAT</b></center></td>");
                        sb.Append("</tr>");
                        foreach (var result in pengadaanobat)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td></td>");
                            sb.Append("<td></td>");
                            sb.Append("<td></td>");
                            sb.Append("<td style='border:1px solid black;'><b>" + result.Obat + "</b></td>");
                            sb.Append("<td style='border:1px solid black;'>" + result.Total + "</td>");
                            sb.Append("<td style='border:1px solid black;'>" + result.SatuanObat + "</td>");
                            sb.Append("</tr>");
                        }
                    }

                }
                string sFileName = "[" + DateTime.Now + "] DATA PENGADAAN OBAT POLIKLINIK IPB.xls";
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(buffer, "application/vnd.ms-excel");
           }
            //Session["pengadaanobats"] = pengadaanobat.ToList<PengadaanObat_ViewModel>();
            return View(pengadaanobat.ToList().ToPagedList(page ?? 1, 20));
        }

        // GET: /PengadaanObat/Create
        public ActionResult Create()
        {
            ViewBag.PenyediaObatID = new SelectList(db.PenyediaObat, "ID", "Nama");
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama");
            ViewBag.count = 1;
            return View();
        }

       
        // POST: /PengadaanObat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PengadaanObat pengadaanobat)
        {
            /*int count = Int32.Parse(Request["count"]);
            for (int kepo = 1; kepo <= count; kepo++)
            {
                ViewBag.Jumlah1 = Request["Jumlah" + kepo + ""].ToString();
                ViewBag.ObatID1 = Request["ObatID" + kepo + ""].ToString();
                ViewBag.HargaAktual1 = Request["HargaAktual" + kepo + ""].ToString();
                ViewBag.HET1 = Request["HET" + kepo + ""].ToString();
            }*/
            if (ModelState.IsValid)
            {
                int count = Int32.Parse(Request["count"]);
                ViewBag.count = count;
                for (int kepo = 1; kepo <= count; kepo++)
                {
                    var Jumlah = Request["Jumlah" + kepo + ""].ToString();
                    var ObatID = Request["ObatID" + kepo + ""].ToString();
                    var HargaAktual = Request["HargaAktual" + kepo + ""].ToString();
                    var HET = Request["HET" + kepo + ""].ToString();
                    if (Jumlah == null)
                        Int32.Parse(Jumlah);
                    pengadaanobat.Jumlah = Int32.Parse(Jumlah);
                    pengadaanobat.ObatID = Int32.Parse(ObatID);
                    if (!String.IsNullOrEmpty(HargaAktual) || !String.IsNullOrEmpty(HET))
                    {
                        pengadaanobat.HargaAktual = Int32.Parse(HargaAktual);
                        pengadaanobat.HET = Int32.Parse(HET);
                    }
                    db.PengadaanObat.Add(pengadaanobat);
                    db.SaveChanges();
                }
                
                ViewBag.PenyediaObatID = new SelectList(db.PenyediaObat, "ID", "Nama", pengadaanobat.PenyediaObatID);
                return View(pengadaanobat);
                
                //return RedirectToAction("Index");
            }

            ViewBag.PenyediaObatID = new SelectList(db.PenyediaObat, "ID", "Nama", pengadaanobat.PenyediaObatID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengadaanobat.ObatID);
            return View(pengadaanobat);
        }

        // GET: /PengadaanObat/Edit/5
        public ActionResult Edit(int? id,bool? E, bool? E1, bool? E2, bool? E3, bool? E4, string S)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PengadaanObat pengadaanobat = db.PengadaanObat.Find(id);
            if (pengadaanobat == null)
            {
                return HttpNotFound();
            }
            ViewBag.PenyediaObatID = new SelectList(db.PenyediaObat, "ID", "Nama", pengadaanobat.PenyediaObatID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengadaanobat.ObatID);

            //simpan jumlah stok sblm diedit
            ViewBag.jmlsblmedit = pengadaanobat.Jumlah;

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
            return View(pengadaanobat);
        }

        // POST: /PengadaanObat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ObatID,Jumlah,Tanggal,HET,HargaAktual,PenyediaObatID")] PengadaanObat pengadaanobat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pengadaanobat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PenyediaObatID = new SelectList(db.PenyediaObat, "ID", "Nama", pengadaanobat.PenyediaObatID);
            ViewBag.ObatID = new SelectList(db.Obat, "ID", "Nama", pengadaanobat.ObatID);
            return View(pengadaanobat);
        }*/

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, PengadaanObat pengadaanobat, string jmlsblmedit)
        {
            ViewBag.E = false;

            //tanggal tidak boleh kosong
            if (pengadaanobat.Tanggal == null) 
            {
                ViewBag.E = true;
                ViewBag.E1 = true;
            }

            //jumlah obat tidak boleh kosong
            if (pengadaanobat.Jumlah == null) 
            {
                ViewBag.E = true;
                ViewBag.E2 = true;
            }

            //jumlah obat harus bilangan positif
            string jumlah = pengadaanobat.Jumlah.ToString();
            if (pengadaanobat.Jumlah != null) 
            { 
                if (!System.Text.RegularExpressions.Regex.IsMatch(jumlah, "^[0-9]+$")) 
                {
                    ViewBag.E = true; 
                    ViewBag.E3 = true;
                }
            }

            //jumlah obat jika stoksaatedit=stoksblmedit-jmlsblmedit bernilai negatif
            var stoksblmedit = (from i in db.StokObat
                               where i.ObatID == pengadaanobat.ObatID
                               where i.KlinikID == 2
                               select i.Stok).FirstOrDefault().ToString();
            var intjmlsblmedit = int.Parse(jmlsblmedit);
            var intstoksblmedit = int.Parse(stoksblmedit);
            var stoksaatedit = intstoksblmedit - intjmlsblmedit;
            if (pengadaanobat.Jumlah != null)
            {
                var jmlsaatedit = pengadaanobat.Jumlah.ToString();
                var intjmlsaatedit = int.Parse(jmlsaatedit);

                if (stoksaatedit<0 && intjmlsaatedit<Math.Abs(stoksaatedit))
                {
                    ViewBag.E = true;
                    ViewBag.E4 = true;
                    ViewBag.stoksaatedit = Math.Abs(stoksaatedit);
                }
            }
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pengadaanupdate = db.PengadaanObat.Where(i => i.ID == id).Single();

            if(TryUpdateModel(pengadaanupdate,"",
                new string[] { "ID", "ObatID", "Jumlah", "Tanggal", "HET", "HargaAktual", "PenyediaObatID" }) && !ViewBag.E)
            {
                try
                {
                    db.Entry(pengadaanupdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex*/)
                {
                    //log the error, uncommend dex and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try Again, and if the problem persists, see your system administrator.");
                }
            }
            //kalau gagal balik ke action edit
            return RedirectToAction("Edit", new { E = ViewBag.E, E1 = ViewBag.E1, E2 = ViewBag.E2, E3 = ViewBag.E3, E4 = ViewBag.E4, S = ViewBag.stoksaatedit});
        }

        
        // GET: /PengadaanObat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PengadaanObat pengadaanobat = db.PengadaanObat.Find(id);
            if (pengadaanobat == null)
            {
                return HttpNotFound();
            }
            //return View(pengadaanobat);
            return PartialView("_Delete", pengadaanobat);
        }

        // POST: /PengadaanObat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Error = false;
            PengadaanObat pengadaanobat = db.PengadaanObat.Find(id);

            var jumlah = pengadaanobat.Jumlah.ToString();
            var stoks = (from i in db.StokObat
                         where i.ObatID == pengadaanobat.ObatID
                         where i.KlinikID == 2
                         select i.Stok).FirstOrDefault().ToString();
            var intjumlah = int.Parse(jumlah);
            var intstoks = int.Parse(stoks);

            if (intjumlah > intstoks)
            {
                ViewBag.Error = true;
                ViewBag.stringstoks = intstoks.ToString();
                ViewBag.stringjumlah = intjumlah.ToString();
            }

            if (!ViewBag.Error)
            {
                db.PengadaanObat.Remove(pengadaanobat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { undeleteable = ViewBag.Error, stk = ViewBag.stringstoks, jml = ViewBag.stringjumlah });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*public ActionResult MultipleCommand(string Obat, string PenyediaObat, DateTime? Date, int? Year, string Month, string Command, string Sortby, int? page)
        {
        
                var pengadaanobat = from a in db.PengadaanObat.Include(t => t.PenyediaObat).Include(t => t.Obat)
                                    select new PengadaanObat_ViewModel()
                                    {
                                        ID = a.ID,
                                        Tanggal = a.Tanggal,
                                        PenyediaObatID = a.PenyediaObatID,
                                        ObatID = a.ObatID,
                                        Jumlah = a.Jumlah,
                                        HargaAktual = a.HargaAktual,
                                        HET = a.HET,
                                        Obat = a.Obat.Nama,
                                        PenyediaObat = a.PenyediaObat.Nama,
                                        SatuanObat = a.Obat.SatuanObat.Nama,
                                    };
                ViewBag.Obat = new SelectList(db.Obat, "ID", "Nama");//output berupa ID tetapi dalam bentuk string
                ViewBag.PenyediaObat = new SelectList(db.PenyediaObat, "ID", "Nama");

                var SelectListYear = ((from year in db.PengadaanObat orderby year.Tanggal.Value.Year select year.Tanggal.Value.Year)).Distinct().ToList();
                ViewBag.Year = new SelectList(SelectListYear);

                if (!String.IsNullOrEmpty(Obat))
                {  
                    int ObatInt = Convert.ToInt32(Obat);
                    pengadaanobat = pengadaanobat.Where(a => a.ObatID == ObatInt);//paging
                }

                if (!String.IsNullOrEmpty(PenyediaObat))
                {
                    int PenyediaObatInt = Convert.ToInt32(PenyediaObat);
                    pengadaanobat = pengadaanobat.Where(b => b.PenyediaObatID == PenyediaObatInt);
                }

                if (Date != null)
                {
                    pengadaanobat = pengadaanobat.Where(c => c.Tanggal == Date);
                }

                if (Year != null)
                {
                    pengadaanobat = pengadaanobat.Where(d => d.Tanggal.Value.Year == Year);
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
                    pengadaanobat = pengadaanobat.Where(e => e.Tanggal.Value.Month == MonthInt);
                }
          if (Command == "Filter")
          {
              //sorting
            ViewBag.SortTanggalParameter = string.IsNullOrEmpty(Sortby) ? "Tanggal Desc" : "";
            ViewBag.SortObatParameter = Sortby == "Obat" ? "Obat Desc" : "Obat";
            ViewBag.SortPenyediaObatParameter = Sortby == "PenyediaObat" ? "PenyediaObat Desc" : "PenyediaObat";

            switch (Sortby)
            {
                case "Tanggal Desc":
                    pengadaanobat = pengadaanobat.OrderByDescending(a => a.Tanggal);
                    break;
                case "Obat":
                    pengadaanobat = pengadaanobat.OrderBy(b => b.Obat);
                    break;
                case "Obat Desc":
                    pengadaanobat = pengadaanobat.OrderByDescending(b => b.Obat);
                    break;
                case "PenyediaObat":
                    pengadaanobat = pengadaanobat.OrderBy(c => c.PenyediaObat);
                    break;
                case "PenyediaObat Desc":
                    pengadaanobat = pengadaanobat.OrderByDescending(c => c.PenyediaObat);
                    break;
                default:
                    pengadaanobat = pengadaanobat.OrderBy(g => g.Tanggal);
                    break;
            }

            //export
            Session["pengadaanobats"] = pengadaanobat.ToList<PengadaanObat_ViewModel>();

            return View(pengadaanobat.ToList().ToPagedList(page ?? 1, 2));
        
          }
          if (Command == "Export")
          {
                //var pengadaanobat = (List<PengadaanObat_ViewModel>)Session["pengadaanobats"];
                pengadaanobat.ToList<PengadaanObat_ViewModel>();
                //pengadaanobat.ToList();
                StringBuilder sb = new StringBuilder();
                if (pengadaanobat != null && pengadaanobat.Any())
                {
                    sb.Append("<table style='1px solid black; font-size:12px;'>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='width:120px', align='center'><b>DATA PENGADAAN OBAT POLIKLINIK</b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:120px;'><center><b>TANGGAL</b></center></td>");
                    sb.Append("<td style='width:300px;'><center><b>NAMA OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>JUMLAH</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>SATUAN OBAT</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>HARGA AKTUAL</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>HET</b></center></td>");
                    sb.Append("<td style='width:120px;'><center><b>PENYEDIA OBAT</b></center></td>");
                    sb.Append("</tr>");

                    foreach (var result in pengadaanobat)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + result.Tanggal + "</td>");
                        sb.Append("<td>" + result.Obat + "</td>");
                        sb.Append("<td>" + result.Jumlah + "</td>");
                        sb.Append("<td>" + result.SatuanObat + "</td>");
                        sb.Append("<td>" + result.HargaAktual + "</td>");
                        sb.Append("<td>" + result.HET + "</td>");
                        sb.Append("<td>" + result.PenyediaObat + "</td>");
                        sb.Append("</tr>");
                    }
                }
                string sFileName = "DATA PENGADAAN OBAT POLIKLINIK.xls";
                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(buffer, "application/vnd.ms-excel");
            }
            return RedirectToAction("Index");
        }*/
    }
}
