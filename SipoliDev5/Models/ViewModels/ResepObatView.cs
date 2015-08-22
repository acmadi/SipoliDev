using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SipoliDev5.Models.ViewModels
{
    public class ResepObatView
    {
        public ResepObatView(int? No, int ID, int RekamMedikID, int ObatID, string NamaObat, int? Jumlah,
            int SatuanObatID,string NamaSatuanObat, string Pemakaian, bool? isDihabiskan, bool? isSetelahMakan)
        {
            this.No = No;
            this.ID = ID;
            this.RekamMedikID = RekamMedikID;
            this.ObatID = ObatID;
            this.NamaObat = NamaObat;
            this.Jumlah = (int)Jumlah;
            this.SatuanObatID = SatuanObatID;
            this.NamaSatuanObat = NamaSatuanObat;
            this.Pemakaian = Pemakaian;
            this.isDihabiskan = (bool)isDihabiskan;
            this.isSetelahMakan = (bool)isSetelahMakan;
        }

        public Nullable<int> No { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<int> RekamMedikID { get; set; }

        [Required]
        [Display(Name = "Nama Obat")]
        public Nullable<int> ObatID { get; set; }
        public string NamaObat { get; set; }

        [Display(Name = "Jumlah")]
        public Nullable<int> Jumlah { get; set; }

        [Required]
        [Display(Name = "Satuan Obat")]
        public Nullable<int> SatuanObatID { get; set; }
        public string NamaSatuanObat { get; set; }

        [Required]
        [Display(Name = "Pemakaian")]
        public string Pemakaian { get; set; }

        [Required]
        [Display(Name = "Dihabiskan")]
        public bool isDihabiskan { get; set; }
        //public IEnumerable<SelectListItem> isDihabiskanList
        //{
        //    get
        //    {
        //        return new List<SelectListItem>{
        //            new SelectListItem{Text="Ya",Value="true"},
        //            new SelectListItem{Text="Tidak",Value="false"}
        //        };
        //    }
        //}

        [Required]
        [Display(Name = "Setelah Makan")]
        public bool isSetelahMakan { get; set; }

        public virtual RekamMedik RekamMedik { get; set; }
        public virtual Obat Obat { get; set; }
        public virtual SatuanObat SatuanObat { get; set; }
    }
}