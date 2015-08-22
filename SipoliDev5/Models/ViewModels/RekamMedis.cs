using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedis
    {
        public int No { get; set; }

        [Display(Name = "ID RM")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Klinik")]
        public int KlinikID { get; set; }

        [Required]
        [Display(Name = "ID Pasien")]
        public int PasienID { get; set; }

        [Display(Name = "Nama Pasien")]
        public string NamaPasien { get; set; }

        [Display(Name = "Waktu Periksa")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy  hh:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public System.DateTime Tanggal { get; set; }

        [Required(ErrorMessage = "Anamnesa dan Diagnosa harus diisi.")]
        [StringLength(150, MinimumLength = 3)]
        [Display(Name = "Anamnesa & Diagnosa")]
        public string AnamnesaDiagnosa { get; set; }

        [Display(Name = "Therapie")]
        [StringLength(150, MinimumLength = 3)]
        public string Therapie { get; set; }


        public string GelarDepan { get; set; }

        public string GelarBelakang { get; set; }

        [Display(Name = "Nama Dokter")]
        public string NamaDokter { get; set; }

        [Required(ErrorMessage = "Dokter belum dipilih.")]
        public int DokterID { get; set; }

        [Display(Name = "Tempat Lahir")]
        public string TempatLahir { get; set; }

        [Display(Name = "Tanggal Lahir")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TanggalLahir { get; set; }

        [Display(Name = "Usia")]
        public int Age { get; set; }


        //public int Age
        //{
        //    get
        //    {
        //        DateTime now = DateTime.Today;
        //        int age = now.Year - Orang.TanggalLahir.Year;
        //        return age;

        //    }
        //}

        public virtual Orang Orang { get; set; }
        public virtual Pegawai Pegawai { get; set; }

    }
}