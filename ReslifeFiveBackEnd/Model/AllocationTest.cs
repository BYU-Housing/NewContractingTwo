using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class AllocationTest
    {
        public int Id { get; set; }
        public int? Area { get; set; }
        public int? Building { get; set; }
        public int? Floor { get; set; }
        public string Bedspace { get; set; } = string.Empty;
        public bool? Active { get; set; }
        public bool? Swappable { get; set; }

    }
}
