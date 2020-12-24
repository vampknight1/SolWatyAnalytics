using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages
{
    public class CustomerDetailBase : ComponentBase
    {
        public Customer Customer { get; set; } = new Customer();

        [Inject]
        public ICustomerService CustomersService { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Customer =  await CustomersService.GetCustomer(int.Parse(Id));
        }
    }
}