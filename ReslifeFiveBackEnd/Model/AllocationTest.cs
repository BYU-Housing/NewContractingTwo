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
        public DateTime? DateContracted { get; set; }
        public DateTime? DateCanceled { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? HoldExpiration { get; set; }
        public DateTime? RoomateRequestExpiration { get; set; }
        public int? SwapPreference { get; set; }
        public bool? LockCheckIn { get; set; }
        public bool? FreezeActivity { get; set; }
        public int? GroupId { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        public DateTime? AllocationEndDate { get; set; }
    }
}
