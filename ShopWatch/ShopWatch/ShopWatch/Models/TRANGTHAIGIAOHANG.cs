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
    
    public partial class TRANGTHAIGIAOHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRANGTHAIGIAOHANG()
        {
            this.DATHANGs = new HashSet<DATHANG>();
        }
    
        public int MAVANDON { get; set; }
        public string VITRI { get; set; }
        public Nullable<System.DateTime> THOIGIANGIAOHANG { get; set; }
        public Nullable<System.DateTime> THOIGIANNHANHANG { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DATHANG> DATHANGs { get; set; }
    }
}
