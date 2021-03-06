//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SipoliDev5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class PengeluaranObat
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<System.DateTime> Tanggal { get; set; }
        public Nullable<int> KlinikID { get; set; }
        public Nullable<int> PasienID { get; set; }
        public Nullable<int> ObatID { get; set; }
        public Nullable<int> Jumlah { get; set; }
        public Nullable<int> TujuanKlinikID { get; set; }
    
        public virtual Orang Orang { get; set; }
        public virtual Klinik Klinik { get; set; }
        public virtual Klinik Klinik1 { get; set; }
        public virtual Obat Obat { get; set; }
        public virtual RekamMedik RekamMedik { get; set; }
    }
}
