using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SolWatyAnalytics.BLL.Models
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
            UpdatedBy = 0;
        }        
        public string CompanyName { get; set; }
        public string Uid { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }        
    }
}
