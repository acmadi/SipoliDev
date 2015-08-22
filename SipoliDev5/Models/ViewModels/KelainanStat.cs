using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class KelainanStat
    {
        public KelainanStat(int No, int KelainanID, string NamaKelainan, int jumlah)
        {
            this.No = No;
            this.KelainanID = KelainanID;
            this.NamaKelainan = NamaKelainan;
            this.jumlah = jumlah;
        }

        public int No { get; set; }
        public int KelainanID { get; set; }
        public string NamaKelainan { get; set; }
        public int jumlah { get; set; }

        public virtual Penyakit Penyakit { get; set; }
    }
}