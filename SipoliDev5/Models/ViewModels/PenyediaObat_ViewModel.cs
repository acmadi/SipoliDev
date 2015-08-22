using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class PenyediaObat_ViewModel
    {
        public int ID { get; set; }

        [DisplayName("Penyedia Obat")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Minimum 5 karaker, maksimum 50 karakter")]
        public string Nama { get; set; }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> KotaKabupatenID { get; set; }

        public string KotaKabupatenNama { get; set; }

        [DisplayName("Contact Person")]
        [StringLength(50, ErrorMessage = "Maksimum 50 karakter")]
        public string ContactPerson { get; set; }

        [DisplayName("Nomor CP")]
        [MaxLength(20, ErrorMessage = "Panjang maksimum 20 karakter")]
        public string NomorContactPerson { get; set; }
    }
}