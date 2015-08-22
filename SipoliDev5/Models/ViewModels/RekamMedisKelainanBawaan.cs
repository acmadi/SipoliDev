using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RekamMedisKelainanBawaan
    {
        public int No { get; set; }
        public int PasienID { get; set; }

        [Required]
        [Display(Name = "Kelainan")]
        public Nullable<int> KelainanBawaanID { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        [RegularExpression(@"[A-Z]+[a-zA-Z''-\s]*$")]
        [Display(Name = "Kelainan")]
        public string NamaKelainanBawaan { get; set; }

        [StringLength(70, MinimumLength = 3)]
        [RegularExpression(@"[A-Z]+[a-zA-Z''-\s]*$")]
        [Display(Name = "Ilmiah")]
        public string NamaIlmiah { get; set; }

        public virtual KelainanBawaanCons KelainanBawaan1 { get; set; }
        public virtual Orang Orang { get; set; }
    }
}