//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DATHANG()
        {
            this.CHITIETDATHANGs = new HashSet<CHITIETDATHANG>();
            this.NOTIFICATIONs = new HashSet<NOTIFICATION>();
        }
    
        public string MADH { get; set; }
        public Nullable<System.DateTime> NGAYMUA { get; set; }
        public Nullable<double> TONGTIEN { get; set; }
        public Nullable<int> MAKHACHHANG { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }
        public Nullable<int> MADIADIEM { get; set; }
        public Nullable<int> MAQUANLYVOUCHER { get; set; }
        public Nullable<int> MATHANHTOAN { get; set; }
        public Nullable<int> MAVANDON { get; set; }
        public string TINHTRANGDH { get; set; }
        public Nullable<bool> HINHTHUCTHANHTOAN { get; set; }
        public Nullable<System.DateTime> NGAYNHAN { get; set; }
        public Nullable<System.DateTime> NGAYHUY { get; set; }
        public Nullable<bool> TUYCHON { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDATHANG> CHITIETDATHANGs { get; set; }
        public virtual DIADIEM DIADIEM { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual QUANLYVOUCHER QUANLYVOUCHER { get; set; }
        public virtual THANHTOAN THANHTOAN { get; set; }
        public virtual TRANGTHAIGIAOHANG TRANGTHAIGIAOHANG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION> NOTIFICATIONs { get; set; }
    }
}
