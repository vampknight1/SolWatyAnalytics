using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.BLL.DAL;
using SolWatyAnalytics.BLL.Models;

namespace SolWatyAnalytics.Api.Repo
{
    public class StationXRepo : IRepoTask<StationX>
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connStr;

        public StationXRepo(AppDbContext appDbContext, IOptions<ConnectionStringList> _connStr)
        {
            _appDbContext = appDbContext;
            this._connStr = _connStr.Value.LocalPgSQL;
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connStr);
            }
        }

        public async Task<StationX> Add(StationX itemObj)
        {
            var result = await _appDbContext.StationX.AddAsync(itemObj);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public Task<StationX> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StationX>> FindAll()
        {
            //return await _appDbContext.StationX.ToListAsync();
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                    SELECT 
                                                ""TimeStamp"",
                                                ""TimeStamp2"",
                                                ph AS {nameof(StationX.Ph)},
                                                nh3n AS {nameof(StationX.Nh3n)},
                                                cod AS {nameof(StationX.Cod)},
                                                tss AS {nameof(StationX.Tss)},
                                                debit AS {nameof(StationX.Debit)},
                                                uid AS {nameof(StationX.Uid)}
                                    FROM station_x
                                    ORDER BY ""TimeStamp"" DESC
                                    ";
                conn.Open();
                var result = await conn.QueryAsync<StationX>(sQuery);
                conn.Close();
                conn.Dispose();

                return result.ToList();
            }
        }

        public async Task<StationX> FindByID(int id)
        {
            //return await _appDbContext.StationX.FirstOrDefaultAsync(x => x.TimeStamp == id);
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                    SELECT 
                                                ""TimeStamp"",
                                                ""TimeStamp2"",
                                                ph AS {nameof(StationX.Ph)},
                                                nh3n AS {nameof(StationX.Nh3n)},
                                                cod AS {nameof(StationX.Cod)},
                                                tss AS {nameof(StationX.Tss)},
                                                debit AS {nameof(StationX.Debit)},
                                                uid AS {nameof(StationX.Uid)}
                                    FROM station_x
                                    WHERE ""TimeStamp"" = @ID
                                    ";
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<StationX>(sQuery, new { @ID = id });
                conn.Close();
                conn.Dispose();

                return result;
            }
        }

        public async void Remove(int id)
        {
            var result = await _appDbContext.StationX.FirstOrDefaultAsync(x => x.TimeStamp == id);
            if (result != null)
            {
                _appDbContext.StationX.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<StationX> Update(StationX itemObj)
        {
            var result = await _appDbContext.StationX.FirstOrDefaultAsync(x => x.TimeStamp == itemObj.TimeStamp);

            if (result != null)
            {
                result.Cod = itemObj.Cod;
                result.Debit = itemObj.Debit;
                result.Nh3n = itemObj.Nh3n;
                result.Ph = itemObj.Ph;
                result.Tss = itemObj.Tss;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        //public StationX GetChanges()
        //{
        //    var result = new StationX();
        //    var options =  new ChangeStreamOptions
        //}
    }
}
