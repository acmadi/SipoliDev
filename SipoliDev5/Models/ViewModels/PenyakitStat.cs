using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class PenyakitStat
    {
        public PenyakitStat(int No, int PenyakitID, string NamaPenyakit, int jumlah)
        {
            this.No = No;
            this.PenyakitID = PenyakitID;
            this.NamaPenyakit = NamaPenyakit;
            this.jumlah = jumlah;
        }

        public int No { get; set; }
        public int PenyakitID { get; set; }
        public string NamaPenyakit { get; set; }
        public int jumlah { get; set; }

        public virtual Penyakit Penyakit { get; set; }

    }
}