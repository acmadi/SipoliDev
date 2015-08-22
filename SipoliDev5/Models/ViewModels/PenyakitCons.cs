using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class PenyakitCons
    {
        public PenyakitCons(int ID, string Nama, string NamaIlmiah, int KelompokPenyakitID)
        {
            this.ID = ID;
            this.Nama = Nama;
            this.NamaIlmiah = NamaIlmiah;
            this.KelompokPenyakitID = KelompokPenyakitID;
        }

        public int ID { get; set; }

        [Display(Name = "Penyakit")]
        public string Nama { get; set; }

        [Display(Name = "Ilmiah")]
        public string NamaIlmiah { get; set; }

        [Display(Name = "Kelompok")]
        public Nullable<int> KelompokPenyakitID { get; set; }

        public virtual ICollection<RiwayatPenyakit> RiwayatPenyakits { get; set; }
        public virtual ICollection<RiwayatPenyakitKeluarga> RiwayatPenyakitKeluargas { get; set; }
        public virtual KelompokPenyakit KelompokPenyakit { get; set; }
    }
}