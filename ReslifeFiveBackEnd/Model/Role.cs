using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool? Active { get; set; } = false;
        public DateTime? DateLastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
