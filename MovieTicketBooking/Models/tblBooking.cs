//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieTicketBooking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBooking
    {
        public int id { get; set; }
        public Nullable<int> seat_Number { get; set; }
        public Nullable<int> cust_id { get; set; }
        public Nullable<bool> status { get; set; }
        public string passCode { get; set; }
    
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblSeat tblSeat { get; set; }
    }
}
