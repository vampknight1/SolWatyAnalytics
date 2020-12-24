using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages.AreaGroup
{
    public class AreaEditBase : ComponentBase
    {
        [Inject]
        public IAreaService AreaService { get; set; }
        public Area Area { get; set; } = new Area();
        public string PageHeaderText { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }
        public EventCallback OnValidSubmit { get; set; }

        protected async override Task OnInitializedAsync()
        {
            int.TryParse(Id, out int objId);

            if (objId != 0)
            {
                PageHeaderText = "Edit Area";
                Area = await AreaService.GetArea(int.Parse(Id));
            }
            else
            {
                PageHeaderText = "Add Area";
                Area = new Area
                {
                    //CodeArea = "00",
                    //NameArea = "XXX"
                };
            }

        }

        protected async Task HandleValidSubmit()
        {
            Area result = null;

            if (Area.Id != 0)
            {
                result = await AreaService.UpdateArea(Area);
            }
            else
            {
                result = await AreaService.AddArea(Area);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/master/area");
            }
        }

        protected async Task Delete_Click()
        {
            await AreaService.DeleteArea(Area.Id);
            NavigationManager.NavigateTo("/master/area");
        }
    }
}