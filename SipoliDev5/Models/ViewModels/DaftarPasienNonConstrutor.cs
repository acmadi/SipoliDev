using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class DaftarPasienNonConstrutor
    {
        public int No { get; set; }

        public int ID { get; set; }

        public string Nama { get; set; }

        public string NIM { get; set; }
        public string NoKTP { get; set; }
        public string NIDN { get; set; }

        [Display(Name = "Tempat Lahir")]
        public string TempatLahir { get; set; }

        [Display(Name = "Tanggal Lahir")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TanggalLahir { get; set; }

        [Display(Name = "Jenis Kelamin")]
        public byte JenisKelaminID { get; set; }

        [Display(Name = "Registrasi")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TanggalRegis { get; set; }

        [Display(Name = "Umur")]
        public Nullable<int> Age { get; set; }
        public Nullable<int> nomorUrut { get; set; }
    }
}