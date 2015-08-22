using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RujukanConst
    {
        public RujukanConst(int ID, int PasienID, DateTime? Tanggal, string AnamnesaDiagnosa,
            int DokterID)
        {
            this.ID = ID;
            this.PasienID = PasienID;
            this.Tanggal = Tanggal;
            this.AnamnesaDiagnosa = AnamnesaDiagnosa;
            this.DokterID = DokterID;

            this.ResepObat = new HashSet<ResepObatView>();
        }

        public int ID { get; set; }

        [Required]
        public Nullable<int> PasienID { get; set; }

        [Required]
        [Display(Name = "Tanggal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode=true)]
        public Nullable<System.DateTime> Tanggal { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name = "Anamnesa & Diagnosa")]
        public string AnamnesaDiagnosa { get; set; }

        [Required]
        public Nullable<int> DokterID { get; set; }


        public virtual Orang Orang { get; set; }
        public virtual Pegawai Pegawai { get; set; }
        public virtual ICollection<ResepObatView> ResepObat { get; set; }
        public virtual Rujukan Rujukan { get; set; }
        public virtual RumahSakit RumahSakit { get; set; }
    }
}