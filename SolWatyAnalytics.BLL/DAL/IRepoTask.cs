using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolWatyAnalytics.BLL.DAL
{
    public interface IRepoTask<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAll();        
        Task<T> FindByID(int id);
        Task<T> Add(T itemObj);
        Task<T> Update(T itemObj);        
        void Remove(int id);
        Task<T> Delete(int id);
    }
}