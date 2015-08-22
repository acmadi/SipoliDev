using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class Obat_ViewModel
    {
        public int ID { get; set; }

        [DisplayName("Nama Obat")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Minimum 5 karaker, maksimum 50 karakter")]
        public string Nama { get; set; }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> SatuanObatID { get; set; }

        public string SatuanObat { get; set; }//tambahan

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> GolonganObatID { get; set; }

        public string GolonganObat { get; set; }//tambahan

        public string Kegunaan { get; set; }
    }
}