using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace empldotnet.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public virtual User user { get; set; }
    }
}