using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.BLL.ViewModel;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages.StationGroup
{
    public class StationDisplayBase : ComponentBase
    {
        [Parameter]
        public StationListVM StationListVM { get; set; }

        [Parameter]
        public Station Station { get; set; }

        [Parameter]
        public EventCallback<int> OnObjDeleted { get; set; }

        [Inject]
        public IStationService StationService { get; set; }

        [Inject]
        public IAreaService AreaService { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();

        protected PMan.Components.ConfirmModalBase DeleteConfirmation { get; set; }

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task Confirmation_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await StationService.DeleteStation(Station.Id);
                await OnObjDeleted.InvokeAsync(Station.Id);
            }
        }
    }
}