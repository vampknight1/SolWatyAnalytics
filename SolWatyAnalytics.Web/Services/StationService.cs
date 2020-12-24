using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using SolWatyAnalytics.BLL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public class StationService : IStationService
    {
        private readonly HttpClient _httpClient;

        public StationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Station> AddStation(Station itemObj)
        {
            return await _httpClient.PostJsonAsync<Station>("api/Station", itemObj);
        }

        public async Task DeleteStation(int id)
        {
            await _httpClient.DeleteAsync($"api/Station/{id}");
        }

        public async Task<Station> GetStation(int id)
        {
            return await _httpClient.GetJsonAsync<Station>($"api/Station/{id}");
        }

        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _httpClient.GetJsonAsync<Station[]>("api/Station");
        }

        public async Task<Station> UpdateStation(Station itemObj)
        {
            return await _httpClient.PutJsonAsync<Station>("api/Station", itemObj);
        }

        //public async Task<IEnumerable<StationListVM>> GetStationsVM()
        //{
        //    return await _httpClient.GetJsonAsync<StationListVM[]>("api/Station");
        //}
    }
}
