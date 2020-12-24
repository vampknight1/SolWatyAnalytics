using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public class AreaService : IAreaService
    {
        private readonly HttpClient _httpClient;

        public AreaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Area> AddArea(Area itemObj)
        {
            return await _httpClient.PostJsonAsync<Area>("api/Area", itemObj);
        }

        public async Task DeleteArea(int id)
        {
            await _httpClient.DeleteAsync($"api/Area/{id}");
        }

        public async Task<Area> GetArea(int id)
        {
            return await _httpClient.GetJsonAsync<Area>($"api/Area/{id}");
        }

        public async Task<IEnumerable<Area>> GetAreas()
        {
            return await _httpClient.GetJsonAsync<Area[]>("api/Area");
        }

        public async Task<Area> UpdateArea(Area itemObj)
        {
            return await _httpClient.PutJsonAsync<Area>("api/Area", itemObj);
        }
    }
}