using System;
using System.Collections.Generic;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class StationX : BaseEntity
    {
        public long TimeStamp { get; set; }
        public DateTime TimeStamp2 { get; set; }
        public double? Cod { get; set; }
        public double? Debit { get; set; }
        public double? Hum { get; set; }
        public double? Nh3n { get; set; }
        public double? Ph { get; set; }
        public double? Temp { get; set; }
        public double? Tss { get; set; }
        public string Uid { get; set; }
    }
}