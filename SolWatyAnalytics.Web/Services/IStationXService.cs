using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public interface IStationXService
    {
        Task<IEnumerable<StationX>> GetStationXs();
    }
}
