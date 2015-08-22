using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class KelainanBawaanCons
    {
        public KelainanBawaanCons(int? No, int ID, string Nama, string NamaIlmiah)
        {
            this.No = No;
            this.ID = ID;
            this.Nama = Nama;
            this.NamaIlmiah = NamaIlmiah;
        }

        public Nullable<int> No { get; set; }

        public int ID { get; set; }

        [Display(Name = "Nama Kelainan")]
        public string Nama { get; set; }

        [Display(Name = "Nama Ilmiah")]
        public string NamaIlmiah { get; set; }

        public virtual ICollection<KelainanBawaan> KelainanBawaan { get; set; }
    }
}