using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAreas();
        Task<Area> GetArea(int id);
        Task<Area> UpdateArea(Area itemObj);
        Task<Area> AddArea(Area itemObj);
        Task DeleteArea(int id);
    }
}