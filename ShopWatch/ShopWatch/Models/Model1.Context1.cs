﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopWatch.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DHEntities : DbContext
    {
        public DHEntities()
            : base("name=DHEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CHITIETDATHANG> CHITIETDATHANGs { get; set; }
        public virtual DbSet<CHITIETGIOHANG> CHITIETGIOHANGs { get; set; }
        public virtual DbSet<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        public virtual DbSet<DATHANG> DATHANGs { get; set; }
        public virtual DbSet<DIADIEM> DIADIEMs { get; set; }
        public virtual DbSet<GIOHANG> GIOHANGs { get; set; }
        public virtual DbSet<HINHANH> HINHANHs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<MATHANG> MATHANGs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<NHAPHANG> NHAPHANGs { get; set; }
        public virtual DbSet<QUANLYVOUCHER> QUANLYVOUCHERs { get; set; }
        public virtual DbSet<SALE> SALEs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<THANHTOAN> THANHTOANs { get; set; }
        public virtual DbSet<THONGKE> THONGKEs { get; set; }
        public virtual DbSet<THONGKENHAPHANG> THONGKENHAPHANGs { get; set; }
        public virtual DbSet<TRANGTHAIGIAOHANG> TRANGTHAIGIAOHANGs { get; set; }
        public virtual DbSet<VOUCHER> VOUCHERs { get; set; }
    }
}