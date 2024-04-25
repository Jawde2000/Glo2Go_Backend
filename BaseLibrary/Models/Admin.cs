using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AdminEmail { get; set; }
        public string? AdminPass { get; set; }
    }
}
