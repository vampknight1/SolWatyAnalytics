using System;
using System.Collections.Generic;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class Klhksents : BaseEntity
    {
        //public new string Id { get; set; }
        public string Uid { get; set; }
        public int DateTime { get; set; }
        public int Ph { get; set; }
        public int Cod { get; set; }
        public int Tss { get; set; }
        public int Nh3n { get; set; }
        public int Debit { get; set; }
    }
}
