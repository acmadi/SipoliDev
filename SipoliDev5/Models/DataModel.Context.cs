﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntitiesConnection : DbContext
    {
        public EntitiesConnection()
            : base("name=EntitiesConnection")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<trx_UserSIPOLI> trx_UserSIPOLI { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<KelainanBawaan> KelainanBawaan { get; set; }
        public virtual DbSet<MutasiPegawai> MutasiPegawai { get; set; }
        public virtual DbSet<RekamMedik> RekamMedik { get; set; }
        public virtual DbSet<ResepObat> ResepObat { get; set; }
        public virtual DbSet<RiwayatPenyakit> RiwayatPenyakit { get; set; }
        public virtual DbSet<RiwayatPenyakitKeluarga> RiwayatPenyakitKeluarga { get; set; }
        public virtual DbSet<Dosen> Dosen { get; set; }
        public virtual DbSet<JenisPegawai> JenisPegawai { get; set; }
        public virtual DbSet<Mahasiswa> Mahasiswa { get; set; }
        public virtual DbSet<MahasiswaDiploma> MahasiswaDiploma { get; set; }
        public virtual DbSet<MahasiswaDoktor> MahasiswaDoktor { get; set; }
        public virtual DbSet<MahasiswaMagister> MahasiswaMagister { get; set; }
        public virtual DbSet<MahasiswaSarjana> MahasiswaSarjana { get; set; }
        public virtual DbSet<Obat> Obat { get; set; }
        public virtual DbSet<Orang> Orang { get; set; }
        public virtual DbSet<Pegawai> Pegawai { get; set; }
        public virtual DbSet<Tendik> Tendik { get; set; }
        public virtual DbSet<GolonganObat> GolonganObat { get; set; }
        public virtual DbSet<HubunganKeluarga> HubunganKeluarga { get; set; }
        public virtual DbSet<JalurMasuk> JalurMasuk { get; set; }
        public virtual DbSet<Kecamatan> Kecamatan { get; set; }
        public virtual DbSet<KelainanBawaan1> KelainanBawaan1 { get; set; }
        public virtual DbSet<KelompokPenyakit> KelompokPenyakit { get; set; }
        public virtual DbSet<Klinik> Klinik { get; set; }
        public virtual DbSet<KotaKabupaten> KotaKabupaten { get; set; }
        public virtual DbSet<Penyakit> Penyakit { get; set; }
        public virtual DbSet<SatuanObat> SatuanObat { get; set; }
        public virtual DbSet<mstDokter> mstDokter { get; set; }
        public virtual DbSet<Rujukan> Rujukan { get; set; }
        public virtual DbSet<Kunjungan> Kunjungan { get; set; }
        public virtual DbSet<RumahSakit> RumahSakit { get; set; }
    }
}