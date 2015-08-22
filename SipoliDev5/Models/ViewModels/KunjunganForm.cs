using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SipoliDev5.Models.ViewModels
{
    public class KunjunganForm
    {
        public int No;
        public int ID { get; set; }

        [DisplayName("Klinik")]
        [Required(ErrorMessage = "Harus dipilih.")]
        public int KlinikID { get; set; }


        [DisplayName("Waktu Kunjungan")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<System.DateTime> Tanggal { get; set; }

        [Required(ErrorMessage="Pasien harus terisi.")]
        [DisplayName("Nama Pasien")]
        //[Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> OrangID { get; set; }

        public string Nama { get; set; }

        [Range(1, 200)]
        [DisplayName("Nomor Tunggu")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> NoUrut { get; set; }

        [DisplayName("Status Panggil")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<bool> StatusPanggil { get; set; }
        public IEnumerable<SelectListItem> StatusPanggilList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Tunggu", Value = "false" },
                    new SelectListItem { Text = "Periksa", Value = "true" }
                };
            }
        }

        [DisplayName("Keterangan")]
        public string Ket { get; set; }

        public virtual Orang Orang { get; set; }
    }
}