using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class DaftarPasien
    {
        public DaftarPasien(int? j, int? ID, string Nama, DateTime? TanggalLahir, string TempatLahir,
            byte JenisKelaminID, DateTime? TanggalRegis, string NIM, string NIDN, string NoKTP, int? Age)
        {
            this.No = j;
            this.ID = (int)ID;
            this.Nama = Nama;
            this.TanggalLahir = (DateTime)TanggalLahir;
            this.TempatLahir = TempatLahir;
            this.JenisKelaminID = JenisKelaminID;
            this.TanggalRegis = TanggalRegis;
            this.NIM = NIM;
            this.NIDN = NIDN;
            this.NoKTP = NoKTP;
            this.Age = Age;
        }

        public Nullable<int> No { get; set; }

        public int ID { get; set; }

        public string Nama { get; set; }

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

        public string NIM { get; set; }

        public string NIDN { get; set; }

        public string NoKTP { get; set; }
        
    }
}