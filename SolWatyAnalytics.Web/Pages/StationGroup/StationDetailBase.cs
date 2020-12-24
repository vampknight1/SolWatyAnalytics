using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages
{
    public class StationDetailBase : ComponentBase
    {
        public Station Station { get; set; } = new Station();

        [Inject]
        public IStationService StationService { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Station =  await StationService.GetStation(int.Parse(Id));
        }
    }
}