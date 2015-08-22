using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class Dokter
    {

        public int ID { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        [RegularExpression(@"[A-Z]+[a-zA-Z''-\s]*$")]
        [Display(Name = "Nama")]
        public string Nama { get; set; }

        [Display(Name = "Gelar Depan")]
        public string GelarDepan { get; set; }

        [Display(Name = "Nama Dokter")]
        public string GelarDanNama { get; set; }
    }
}