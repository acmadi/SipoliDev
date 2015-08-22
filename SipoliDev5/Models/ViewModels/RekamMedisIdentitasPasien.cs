using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedisIdentitasPasien
    {
        public RekamMedisIdentitasPasien(int ID, string Nama, string TempatLahir,
            DateTime TanggalLahir, string AlamatAsal, int Umur, byte JenisKelaminID)
        {
            this.ID = ID;
            this.Nama = Nama;
            this.TempatLahir = TempatLahir;
            this.TanggalLahir = TanggalLahir;
            this.AlamatAsal = AlamatAsal;
            this.Umur = Umur;
            this.JenisKelaminID = JenisKelaminID;
        }

        public int ID { get; set; }

        [Display(Name = "Nama Lengkap")]
        public string Nama { get; set; }
        public string TempatLahir { get; set; }

        [Display(Name = "Tanggal Lahir")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TanggalLahir { get; set; }

        [Display(Name = "Alamat Asal")]
        public string AlamatAsal { get; set; }

        [Display(Name = "Umur")]
        public int Umur { get; set; }

        [Display(Name = "Jenis Kelamin")]
        public byte JenisKelaminID { get; set; }

    }
}