using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RumahSakitForm
    {
        public int ID { get; set; }

        [Display(Name = "Nama")]
        [Required(ErrorMessage="Nama Rumah Sakit belum valid.")]
        [StringLength(60, MinimumLength = 3,ErrorMessage="Minimal 3 karakter, maksimal 60 karakter.")]
        public string Nama { get; set; }

        [Required(ErrorMessage = "Lokasi kecamatan belum dipilih.")]
        [Display(Name="Kecamatan")]
        public int KecamatanID { get; set; }

        
        [Display(Name = "Telepon")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Harus diisi angka semua.")]
        [StringLength(16, MinimumLength = 3,ErrorMessage="Panjang nilai tidak valid.")]
        public string NoTelepon { get; set; }
    }
}