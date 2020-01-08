using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Helpers
{
    public class Response
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}