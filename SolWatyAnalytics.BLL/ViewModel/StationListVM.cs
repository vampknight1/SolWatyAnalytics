using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolWatyAnalytics.BLL.ViewModel
{
    public partial class StationListVM : Station
    {
        public StationListVM()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
            UpdatedBy = 0;
        }

        public virtual string NameArea { get; set; } = string.Empty;
    }
}
