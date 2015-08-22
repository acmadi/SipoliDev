using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SipoliDev5.Models.ViewModels
{
    public class PemeriksaConst
    {
        public PemeriksaConst(int PegawaiID, DateTime? TMT, DateTime? TST,
            bool? StatusAktif)
        {
            this.PegawaiID = PegawaiID;
            this.TMT = TMT;
            this.TST = TST;
            this.StatusAktif = StatusAktif;
        }

        [Required]
        public int PegawaiID { get; set; }

        [Display(Name = "Terhitung Mulai Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TMT { get; set; }

        [Display(Name = "Terhitung Selesai Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TST { get; set; }

        [Required]
        [Display(Name = "Status Aktif")]
        public Nullable<bool> StatusAktif { get; set; }
        public IEnumerable<SelectListItem> StatusAktifList
        {
            get
            {
                return new List<SelectListItem>{
                    new SelectListItem{Text="Non-aktif", Value="false"},
                    new SelectListItem{Text="Aktif", Value="true"}
                };
            }
        }

        public virtual Pegawai Pegawai { get; set; }
    }
}