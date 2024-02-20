using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegApp.Models
{
    public class UserList
    {
        public int Id { get; set; }
        public string uname { get; set; }
        public string uemail { get; set; }
        public string ugender { get; set; }
        public bool isActive { get; set; }
        public string uphoto { get; set; }
        public string sname { get; set; }
        public string cname { get; set; }
    }
}