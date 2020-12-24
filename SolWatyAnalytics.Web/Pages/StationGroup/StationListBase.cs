using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;

namespace SolWatyAnalytics.Web.Pages
{
    public class StationListBase : ComponentBase
    {
        [Inject]
        public IStationService StationService { get; set; }

        public IEnumerable<Station> Stations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Stations = (await StationService.GetStations()).ToList();
        }

        protected async Task ObjDeleted()
        {
            Stations = (await StationService.GetStations()).ToList();
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
