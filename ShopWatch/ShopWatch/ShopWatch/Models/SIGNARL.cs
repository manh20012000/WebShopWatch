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
    
    public partial class SIGNARL
    {
        public int ID { get; set; }
        public Nullable<int> MAKHACHHANG { get; set; }
        public string CONTEN { get; set; }
        public string TITLE { get; set; }
        public Nullable<System.DateTime> TIME { get; set; }
        public string TENKHACHHANG { get; set; }
        public string AVATAR { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}
