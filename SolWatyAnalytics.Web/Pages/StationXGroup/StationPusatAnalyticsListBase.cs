using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace SolWatyAnalytics.Web.Pages
{
    public class StationPusatAnalyticsListBase : ComponentBase
    {
        [Inject]
        public IStationXService StationXService { get; set; }
        public IEnumerable<StationX> StationXs { get; set; }
        protected string PageHeaderText { get; set; }
        protected string StationUid { get; set; }
        protected NavigationManager NavigationManager;

        protected override async Task OnInitializedAsync()
        {
            // Create a timer with a two second interval.
            Timer aTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            PageHeaderText = "Station Kantor Pusat";
            StationUid = "2000000000014";

            StationXs = (await StationXService.GetStationXs()).ToList();

            await LoadData();
        }

        //private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        //{
        //    Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
        //    InvokeAsync(gridDT.Reload());
        //}

        protected async Task LoadData()
        {
            StationXs = (await StationXService.GetStationXs()).ToList();
            await InvokeAsync(StateHasChanged);
        }

        protected void RowRender(RowRenderEventArgs<StationX> args)
        {
            args.Attributes.Add("style", $"font-weight: {(args.Data.TimeStamp > 20 ? "bold" : "normal")};");
        }

        protected void CellRender(CellRenderEventArgs<StationX> args)
        {
            if (args.Column.Property == "Ph")
            {
                if (args.Data.Ph > 14 || args.Data.Ph < 0)
                {
                    args.Attributes.Add("style", $"background-color: red");
                }
                else if (args.Data.Ph == 0 || args.Data.Ph == 14)
                {
                    args.Attributes.Add("style", $"background-color: yellow");
                }
                else
                {
                    args.Attributes.Add("style", $"background-color: white");
                }
            }

            if (args.Column.Property == "Nh3n")
            {
                if (args.Data.Nh3n > 10000 || args.Data.Nh3n < 0)
                {
                    args.Attributes.Add("style", $"background-color: red");
                }
                else if ((args.Data.Nh3n > 0 && args.Data.Nh3n <= 1000) || (args.Data.Nh3n >= 9000 && args.Data.Nh3n <= 10000))
                {
                    args.Attributes.Add("style", $"background-color: yellow");
                }
                else
                {
                    args.Attributes.Add("style", $"background-color: white");
                }
            }

            if (args.Column.Property == "Cod")
            {
                if (args.Data.Cod > 5000 || args.Data.Cod< 0.1)
                {
                    args.Attributes.Add("style", $"background-color: red");
                }
                else if ((args.Data.Cod > 0.1 && args.Data.Cod <= 500) || (args.Data.Cod >= 4500 && args.Data.Cod <= 5000))
                {
                    args.Attributes.Add("style", $"background-color: yellow");
                }
                else
                {
                    args.Attributes.Add("style", $"background-color: white");
                }
            }

            if (args.Column.Property == "Tss")
            {
                if (args.Data.Tss > 3000 || args.Data.Tss < 0)
                {
                    args.Attributes.Add("style", $"background-color: red");
                }
                else if ((args.Data.Tss > 0 && args.Data.Tss <= 300) || (args.Data.Tss >= 2700 && args.Data.Tss <= 3000))
                {
                    args.Attributes.Add("style", $"background-color: yellow");
                }
                else
                {
                    args.Attributes.Add("style", $"background-color: white");
                }
            }

            if (args.Column.Property == "Debit")
            {
                args.Attributes.Add("style", $"background-color: {(args.Data.Debit > 20 ? "yellow" : "white")};");
            }
        }
    }
}