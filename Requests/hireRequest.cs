using System;

namespace KütüphaneAPI
{
    public class hireRequest
    {
        public int bookid { get; set; }
        public int userid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public DateTime deliverydate { get; set; }
    }
}