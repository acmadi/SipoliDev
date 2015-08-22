using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class PengadaanObat_ViewModel
    {
        public int ID { get; set; }

        [DisplayName("Tanggal")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Tanggal { get; set; }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int PenyediaObatID { get; set; }

        public string PenyediaObat { get; set; }


        [Required(ErrorMessage = "Tidak boleh kosong")]
        public Nullable<int> ObatID { get; set; }

        public string Obat { get; set; }

        [DisplayName("Jumlah")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Inputan harus berupa angka")]
        [Range(1, 50, ErrorMessage = "Hanya dalam rentang 1 s/d 50")]
        public Nullable<int> Jumlah { get; set; }

        [DisplayName("Harga")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Inputan harus berupa angka")]
        [Range(1000, 500000, ErrorMessage = "Hanya dalam rentang 1000 s/d 500000 rupiah")]
        public Nullable<int> HargaAktual { get; set; }

        [DisplayName("Harga")]
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Inputan harus berupa angka")]
        [Range(1000, 500000, ErrorMessage = "Hanya dalam rentang 1000 s/d 500000 rupiah")]
        public Nullable<int> HET { get; set; }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int SatuanObatID { get; set; }

        public string SatuanObat { get; set; }
    }
}