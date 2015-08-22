using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class Staf
    {
        [Display(Name = "Nama")]
        public string Nama { get; set; }

        [Display(Name = "Gelar Depan")]
        public string GelarDepan { get; set; }
    }
}