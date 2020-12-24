using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages.StationGroup
{
    public class StationEditBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IStationService StationService { get; set; }
        public Station Station { get; set; } = new Station();
        public string PageHeaderText { get; set; }

        [Inject]
        public IAreaService AreaService { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback<int> OnObjDeleted { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            #region Must test this function
            //if (authenticationState.User.IsInRole("Admin"))
            //{
            //} 
            #endregion

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/master/station/edit/{Id}");
                NavigationManager.NavigateTo($"/identity/account/login?returnUrl={returnUrl}");
            }

            int.TryParse(Id, out int objId);

            if (objId != 0)
            {
                PageHeaderText = "Edit Station";
                Station = await StationService.GetStation(int.Parse(Id));
            }
            else
            {
                PageHeaderText = "Add Station";
                Station = new Station
                {
                    //AreaCode = "00",
                    //NameStation = "XXX"
                };
            }
            
            Areas = (await AreaService.GetAreas()).ToList();
        }

        protected async Task HandleValidSubmit()
        {
            Station result = null;

            if (Station.Id != 0)
            {
                result = await StationService.UpdateStation(Station);
            }
            else
            {
                result = await StationService.AddStation(Station);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/master/station");
            }
        }

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
                NavigationManager.NavigateTo("/master/station");
            }
        }
    }
}