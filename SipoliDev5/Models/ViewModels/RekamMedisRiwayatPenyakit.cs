using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedisRiwayatPenyakit
    {
        public int No { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public Nullable<int> PasienID { get; set; }

        [Required]
        [Display(Name = "Penyakit")]
        public Nullable<int> PenyakitID { get; set; }

        [Display(Name = "Penyakit")]
        [StringLength(70, MinimumLength = 3)]
        [RegularExpression(@"[A-Z]+[a-zA-Z''-\s]*$")]
        public string NamaPenyakit { get; set; }

        [Required]
        [Range(1950, 2015)]
        [RegularExpression(@"[0-9]*\/?[0-9]+", ErrorMessage = "{0} harus berupa angka Tahun.")]
        public Nullable<int> Tahun { get; set; }


        public virtual Orang Orang { get; set; }
        public virtual Penyakit Penyakit { get; set; }

    }
}