using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class Area : BaseEntity
    {
        public Area()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
            UpdatedBy = 0;
        }

        [Required]
        [MinLength(2)]
        [MaxLength(16)]
        public string CodeArea { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(32)]
        public string NameArea { get; set; }
    }
}