using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class DokterDropDownList
    {
        public DokterDropDownList(int ID, string Nama, string GelarDepan)
        {
            this.ID = ID;
            this.Nama = Nama;
            this.GelarDepan = GelarDepan;
        }

        public int ID { get; set; }
        public string Nama { get; set; }
        public string GelarDepan { get; set; }

        
    }
}