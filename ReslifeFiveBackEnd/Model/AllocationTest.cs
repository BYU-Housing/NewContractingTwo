﻿using System;
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
        public bool? SeekingReplacement { get; set; }
        public int? AgreementPeriod { get; set; }
        public decimal? DailyRate { get; set; }
        public int? RoomType { get; set; }
        public string TakenBy { get; set; } = string.Empty;
        public string OnHoldFor { get; set; } = string.Empty;
        public string RequestedRoommate { get; set; } = string.Empty;
        public bool? Sex {  get; set; }
        public bool? Minor { get; set; }
        public bool? EighteenMinus { get; set; }
        public bool? NineteenPlus { get; set; }
        public bool? REQMet { get; set; }
        public bool? REQNotMet { get; set; }
        public int? OccupantType { get; set; }


    }
}
