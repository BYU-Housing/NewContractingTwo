using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string NetID { get; set; } = string.Empty;
        public DateTime? DateLastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }
        public virtual Role? Role { get; set; }
        public int? RoleID { get; set; }
    }
}
