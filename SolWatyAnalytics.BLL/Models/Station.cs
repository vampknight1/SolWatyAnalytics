using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class Station : BaseEntity
    {
        public Station()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
            UpdatedBy = 0;
        }

        [Required(ErrorMessage ="*")]
        [MinLength(2)]
        [MaxLength(32)]
        public string NameStation { get; set; }

        [Required(ErrorMessage = "*")]
        public string AreaCode { get; set; }

        [Required(ErrorMessage = "*")]
        [MinLength(13, ErrorMessage = "MIN Length is 13 Char")]
        [MaxLength(13, ErrorMessage = "MAX Length is 13 Char")]
        public string StationUid { get; set; }

        //public virtual string NameArea { get; set; } = String.Empty;
    }
}
