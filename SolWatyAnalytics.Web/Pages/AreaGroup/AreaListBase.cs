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
    public class AreaListBase : ComponentBase
    {
        [Inject]
        public IAreaService AreaService { get; set; }
        public IEnumerable<Area> Areas { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Areas = (await AreaService.GetAreas()).ToList();
        }

        
    }
}
