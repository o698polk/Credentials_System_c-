using System;
using System.Collections.Generic;

namespace carnetutelvt.Models
{
    public partial class Usertb
    {
        public Usertb()
        {
            Detallestbs = new HashSet<Detallestb>();
        }

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Passwords { get; set; }
        public DateTime? Dateupdate { get; set; }
        public DateTime? Datecreate { get; set; }
        public string? Numberverify { get; set; }
        public int? Verifyuser { get; set; } 
        public string? Typess { get; set; }
       public string? Rol{ get; set; }
		public virtual ICollection<Detallestb> Detallestbs { get; set; }

    }
}
