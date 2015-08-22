using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RujukanForm
    {
        
        public int ID { get; set; }
        //public int RekamMedikID { get; set; }

        [Required(ErrorMessage="Tanggal belum valid.")]
        [Display(Name = "Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tanggal { get; set; }

        [Required(ErrorMessage="Anamnesa dan Diagnosa belum valid.")]
        [StringLength(100)]
        [Display(Name = "Anamnesa & Diagnosa")]
        public string AnamnesaDiagnosa { get; set; }

        [Required(ErrorMessage="Nama dokter belum valid.")]
        [Display(Name = "Nama Dokter")]
        public int? DokterID { get; set; }

        [Required(ErrorMessage = "Pasien belum valid.")]
        [Display(Name = "Nama Pasien")]
        public int PasienID { get; set; }

        
        [Required(ErrorMessage = "Bagian belum diisi.")]
        [Display(Name = "Bagian")]
        [StringLength(65, MinimumLength = 3)]
        public string Bagian { get; set; }

        /*
        [Required(ErrorMessage="Rumah sakit tujuan belum valid.")]
        public int RumahSakitID { get; set; }

        [Required(ErrorMessage="Bagian yang dituju di Rumah Sakit belum valid.")]
        public string Bagian { get; set; }
         */ 

        /* */
        public virtual Klinik Klinik { get; set; }
        //public virtual RekamMedik RekamMedik { get; set; }
        public virtual Orang Orang { get; set; }
        public virtual Pegawai Pegawai { get; set; }
        //public virtual RumahSakit RumahSakit { get; set; }
        public virtual Rujukan Rujukan { get; set; }
        public ICollection<ResepObat> ResepObat { get; set; }

         
    }
}