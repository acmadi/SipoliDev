using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SipoliDev5.Models.ViewModels
{
    public class Pemeriksa
    {
        public int No { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 10)]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} harus sesuai dengan KTP")]
        [Display(Name = "No. KTP")]
        public string NomorKTP { get; set; }

        [Required]
        public int PegawaiID { get; set; }


        public string Nama { get; set; }

        public string GelarDepan { get; set; }

        public string GelarBelakang { get; set; }

        [Display(Name = "Terhitung Mulai Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TMT { get; set; }

        [Display(Name = "Terhitung Selesai Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TST { get; set; }

        [Required]
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