using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages
{
    public class AreaDetailBase : ComponentBase
    {     
        [Inject]
        public IAreaService AreaService { get; set; }
        public Area Area { get; set; } = new Area();

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Area =  await AreaService.GetArea(int.Parse(Id));
        }
    }
}