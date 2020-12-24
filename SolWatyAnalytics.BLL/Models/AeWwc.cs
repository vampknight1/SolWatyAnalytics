using System;
using System.Collections.Generic;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class AeWwc : BaseEntity
    {
        public new int Id { get; set; }
        public int? ObjectId { get; set; }
        public int? DeviceId { get; set; }
        public int? VarTagId { get; set; }
        public int? Type { get; set; }
        public short? Condition { get; set; }
        public short? MustAck { get; set; }
        public int? AckAccessRights { get; set; }
        public short? IsActive { get; set; }
        public short? Acknowledged { get; set; }
        public long? StartTimeStamp { get; set; }
        public long? EndTimeStamp { get; set; }
        public long? ReceiptTimeStamp { get; set; }
        public long? AckTimeStamp { get; set; }
        public long? NoteEditTimeStamp { get; set; }
        public int ComputerId { get; set; }
        public int? UserId { get; set; }
        public int? AckUserId { get; set; }
        public double? VarTagValue { get; set; }
        public int? TextId { get; set; }
        public string Text { get; set; }
        public string Note { get; set; }
        public int? Priority { get; set; }
        public int? CmdUserId { get; set; }
        public long? LastOccurrenceTimeStamp { get; set; }
        public int? OccurrenceCount { get; set; }
    }
}
