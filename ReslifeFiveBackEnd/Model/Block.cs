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

        // Active will default to false in the database so that it is not null this way it will not fail in the database.
        public bool Active { get; set; } = false;
    }
}
