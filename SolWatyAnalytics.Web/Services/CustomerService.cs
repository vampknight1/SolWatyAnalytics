using Microsoft.AspNetCore.Components;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _httpClient.GetJsonAsync<Customer>($"api/Customer/{id}");
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _httpClient.GetJsonAsync<Customer[]>("api/Customer");
        }
    }
}