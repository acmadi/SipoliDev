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
    
    public partial class Dosen
    {
        public int ID { get; set; }
        public string NIDN { get; set; }
        public string NIRA { get; set; }
        public Nullable<int> TahunSerdos { get; set; }
        public string NomorSerdos { get; set; }
        public string KUM { get; set; }
    
        public virtual JenisPegawai JenisPegawai { get; set; }
    }
}
