using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class Block
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool? Active { get; set; }
    }
}
