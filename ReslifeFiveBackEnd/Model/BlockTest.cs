using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class BlockTest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? RoomNumber { get; set; }
        public int? OccupantType { get; set; }
        public int? BedspaceCode { get; set; }
        public int? Area {  get; set; }
        public int? Sex { get; set; }
        public int? Community { get; set; }
        public int? Building { get; set; }
        public int? Floor { get; set; }
        public int? Wing { get; set; }
        public int? StairWell { get; set; }
        public int? ApartmentUnit { get; set; }
        public bool? Active { get; set; } = false;
        public bool? ESA { get; set; } = false;
        public bool? Athlete { get; set; } = false;
        public bool? ADA { get; set; } = false;
        public bool? InternationalScholarship { get; set; } = false;
        public bool? Wheatley { get; set; } = false;
        public bool? NoAnimal { get; set; } = false;
        public bool? IsolateQuarantine { get; set; } = false;
    }
}
