using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SipoliDev5.Models.ViewModels
{
    public class RujukanView
    {


        public RujukanView(int? j, int RekamMedikID, DateTime? Tanggal, string AnamnesaDiagnosa, int? DokterID,
            string NamaDokter, string NamaPasien, string Tujuan, string Bagian)
        {
            this.j = (int)j;
            this.ID = RekamMedikID;
            this.Tanggal = (DateTime)Tanggal;
            this.AnamnesaDiagnosa = AnamnesaDiagnosa;
            this.DokterID = DokterID;
            this.NamaDokter = NamaDokter;
            this.NamaPasien = NamaPasien;
            this.Tujuan = Tujuan;
            this.Bagian = Bagian;
        }

        public int j { get; set; }
        public int ID { get; set; }
        public int RekamMedikID { get; set; }

        [Required]
        [Display(Name = "Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tanggal { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Anamnesa & Diagnosa")]
        public string AnamnesaDiagnosa { get; set; }

        [Required]
        [Display(Name = "Nama Dokter")]
        public int? DokterID { get; set; }

        [Required(ErrorMessage = "Pasien belum dipilih.")]
        [Display(Name = "Nama Pasien")]
        public int PasienID { get; set; }

        [Display(Name = "Nama Pasien")]
        public string NamaPasien { get; set; }

        [Display(Name = "Nama Dokter")]
        public string NamaDokter { get; set; }

        [Display(Name = "Tujuan")]
        [StringLength(65, MinimumLength = 3)]
        public string Tujuan { get; set; }

        [Required(ErrorMessage = "Bagian belum diisi.")]
        [Display(Name = "Bagian")]
        [StringLength(65, MinimumLength = 3)]
        public string Bagian { get; set; }


        public virtual RekamMedik RekamMedik { get; set; }
        public virtual Orang Orang { get; set; }
        public virtual Pegawai Pegawai { get; set; }
        public virtual RumahSakit RumahSakit { get; set; }
        public virtual Rujukan Rujukan { get; set; }

    }
}