using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class PenyakitView
    {
        public int No;
        public int ID { get; set; }

        [Required(ErrorMessage="Nama penyakit harus valid.")]
        [Display(Name = "Penyakit")]
        public string Nama { get; set; }

        //[Required(ErrorMessage = "Nama ilmiah penyakit harus valid.")]
        [Display(Name = "Ilmiah")]
        public string NamaIlmiah { get; set; }

        [Required(ErrorMessage = "Kelompok penyakit harus valid.")]
        [Display(Name = "Kelompok")]
        public Nullable<int> KelompokPenyakitID { get; set; }

        public virtual ICollection<RiwayatPenyakit> RiwayatPenyakits { get; set; }
        public virtual ICollection<RiwayatPenyakitKeluarga> RiwayatPenyakitKeluargas { get; set; }
        public virtual KelompokPenyakit KelompokPenyakit { get; set; }
    }
}