using System;
using System.Collections.Generic;

namespace carnetutelvt.Models
{
    public partial class Detallestb
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Surnames { get; set; }
        public string? Specialty { get; set; }
        public string? Faculty { get; set; }
        public string? Ci { get; set; }
        public string? Imgcarnet { get; set; }
        public int? Iduser { get; set; }
        public string?  Bloodtype { get; set; }
    public DateTime? Dateupdate { get; set; }
        public DateTime? Datecreate { get; set; }

        public virtual Usertb? IduserNavigation { get; set; }
    }
}
