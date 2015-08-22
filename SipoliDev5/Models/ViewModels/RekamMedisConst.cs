using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedisConst
    {
        public RekamMedisConst(int ID, int? KlinikID, int PasienID, DateTime Tanggal,
            string AnamnesaDiagnosa, string Therapie, int DokterID)
        {
            this.ID = ID;
            this.KlinikID = KlinikID;
            this.PasienID = PasienID;
            this.Tanggal = Tanggal;
            this.AnamnesaDiagnosa = AnamnesaDiagnosa;
            this.Therapie = Therapie;
            this.DokterID = DokterID;
        }


        public int ID { get; set; }

        [Required]
        [Display(Name = "Klinik")]
        public Nullable<int> KlinikID { get; set; }

        [Display(Name = "ID Pasien")]
        public int PasienID { get; set; }

        [Display(Name = "Waktu Periksa")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public System.DateTime Tanggal { get; set; }

        [Required]
        [Display(Name = "Anamnesa & Diagnosa")]
        [StringLength(150, MinimumLength = 3)]
        public string AnamnesaDiagnosa { get; set; }

        [Display(Name = "Therapie")]
        [StringLength(150, MinimumLength = 3)]
        public string Therapie { get; set; }

        [Required]
        [Display(Name = "Dokter")]
        public int DokterID { get; set; }



        public virtual Orang Orang { get; set; }
        public virtual Pegawai Pegawai { get; set; }
    }
}