using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public interface IStationService
    {
        Task<IEnumerable<Station>> GetStations();
        Task<Station> GetStation(int id);
        Task<Station> UpdateStation(Station itemObj);
        Task<Station> AddStation(Station itemObj);
        Task DeleteStation(int id);
    }
}