using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Pages
{
    public class StationxAnalyticsListBase : ComponentBase
    {
        [Inject]
        public IStationXService StationXService { get; set; }
        public IEnumerable<StationX> StationXs { get; set; }


        protected NavigationManager NavigationManager;
        protected HttpClient Http;    //test
        protected HubConnection hubConnection;
        //StationX[] stationXs;


        protected override async Task OnInitializedAsync()
        {
            ////await Task.Run(LoadStationX);
            //StationXs = (await StationXService.GetStationX()).ToList();

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/broadcastHub"))
                .Build();

            hubConnection.On("ReceiveMessage", () =>
            {
                CallLoadData();
                StateHasChanged();
            });

            await hubConnection.StartAsync();

            await LoadData();
        }

        private void CallLoadData()
        {
            Task.Run(async () =>
            {
                await LoadData();
            });
        }

        protected async Task LoadData()
        {
            StationXs = (await StationXService.GetStationXs()).ToList();
            StateHasChanged();
        }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }

    }
}
