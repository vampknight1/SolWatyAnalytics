using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolWatyAnalytics.BLL.DAL
{
    public interface IRepo<T> where T : BaseEntity
    {
        List<T> FindAll();
        T FindByID(int id);
        T Add(T itemObj);
        void Update(T itemObj);
        void ApiUpdate(T itemObj, int id);
        void Remove(int id);
    }
}