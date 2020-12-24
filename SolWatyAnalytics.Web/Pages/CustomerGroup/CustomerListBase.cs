using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages
{
    public class CustomerListBase : ComponentBase
    {
        [Inject]
        public ICustomerService CustomerService { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Customers = (await CustomerService.GetCustomers()).ToList();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    object p = await JSRuntime.InvokeAsync<object>("TestDataTablesAdd");
        //}

        //void IDisposable.Dispose()
        //{
        //    JSRuntime.InvokeAsync<object>("TestDataTablesRemove", "#datatables");
        //}
    }
}
