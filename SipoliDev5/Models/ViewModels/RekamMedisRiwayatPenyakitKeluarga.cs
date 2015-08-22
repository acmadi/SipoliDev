using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedisRiwayatPenyakitKeluarga
    {
        public int No { get; set; }
        public int ID { get; set; }

        [Required]
        public Nullable<int> PasienID { get; set; }

        [Required]
        [Display(Name = "Hubungan")]
        public Nullable<byte> HubunganKeluargaID { get; set; }

        [Display(Name = "Hubungan")]
        public string NamaHubungan { get; set; }

        [Required]
        [Display(Name = "Penyakit")]
        public Nullable<int> PenyakitID { get; set; }

        [Display(Name = "Penyakit")]
        public string NamaPenyakit { get; set; }

        public virtual HubunganKeluarga HubunganKeluarga { get; set; }
        public virtual Orang Orang { get; set; }
        public virtual Penyakit Penyakit { get; set; }

    }
}