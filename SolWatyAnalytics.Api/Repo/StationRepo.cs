using Microsoft.EntityFrameworkCore;
using Npgsql;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.BLL.DAL;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Extensions.Options;
using SolWatyAnalytics.BLL.ViewModel;

namespace SolWatyAnalytics.Api.Repo
{
    public class StationRepo : IRepoTask<Station>
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connStr;

        public StationRepo(AppDbContext appDbContext, IOptions<ConnectionStringList> connStr)
        {
            _appDbContext = appDbContext;
            _connStr = connStr.Value.LocalPgSQL;
        }

        internal IDbConnection Connection
        {
            get {
                return new NpgsqlConnection(_connStr);
            }
        }

        public async Task<Station> Add(Station itemObj)
        {
            Station station = new Station
            {
                NameStation = itemObj.NameStation,
                AreaCode = itemObj.AreaCode,
                StationUid = itemObj.StationUid,
            };

            await _appDbContext.Station.AddAsync(station);
            await _appDbContext.SaveChangesAsync();
            return station;
        }

        #region public async Task<IEnumerable<Station>> FindAll()
        public async Task<IEnumerable<Station>> FindAll()
        {
            #region using EF Core Lamda
            //var result = _appDbContext.Station
            //                //.Include(e => e.Area)
            //                .Where(x => x.IsDeleted != true)
            //                .ToListAsync();

            //return await result; 
            #endregion

            string sQuery = $@"
                                        SELECT id AS {nameof(Station.Id)}, 
                                                    name_station AS {nameof(Station.NameStation)},
                                                    area_code AS {nameof(Station.AreaCode)},
                                                    station_uid AS {nameof(Station.StationUid)},
                                                    created_at AS {nameof(Station.CreatedAt)}, 
                                                    updated_at AS {nameof(Station.UpdatedAt)},
                                                    updated_by AS {nameof(Station.UpdatedBy)}
                                        FROM station
                                        WHERE is_deleted != true";

            var result = _appDbContext.Station.FromSqlRaw(sQuery).ToListAsync();
            return await result;
        }
        #endregion

        #region public async Task<Station> FindByID(int id)
        public async Task<Station> FindByID(int id)
        {
            #region using EF Core Lamda
            //var result = _appDbContext.Station
            //                //.Include(e => e.Area)
            //                .Where(x => x.IsDeleted != true)
            //                .FirstOrDefaultAsync(x => x.Id == id); 
            #endregion

            var Id = new NpgsqlParameter("Id", id);                                                 // var Id2 = new SqlParameter("Id2", id);
                                                                                                    // var Id3 = new SqlParameter("Id3", id);
            string sQuery = $@"
                                        SELECT {nameof(Station.Id)}, 
                                                    {nameof(Station.NameStation)},
                                                    {nameof(Station.AreaCode)},
                                                    {nameof(Station.StationUid)},
                                                    {nameof(Station.CreatedAt)}, 
                                                    {nameof(Station.UpdatedAt)},
                                                    {nameof(Station.UpdatedBy)}
                                        FROM {nameof(Station)}
                                        WHERE {nameof(Station.Id)} = @{nameof(id)}
                                        AND {nameof(Station.IsDeleted)} != true";

            var result = _appDbContext.Station.FromSqlRaw(sQuery, Id).FirstOrDefaultAsync();  // var propinsi = _context.Propinsi.FromSql(query, Id, Id2, Id3).FirstOrDefault();

            return await result;
        } 
        #endregion

        public async Task<IEnumerable<StationListVM>> FindAllDapper()
        {
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT s.id, 
                                                    s.name_station AS {nameof(StationListVM.NameStation)},
                                                    s.area_code AS {nameof(StationListVM.AreaCode)},
                                                    a.name_area AS {nameof(StationListVM.NameArea)},
                                                    s.station_uid AS {nameof(StationListVM.StationUid)}
                                        FROM station s
                                            LEFT JOIN area a ON s.area_code = a.code_area
                                        WHERE s.is_deleted != true
                                        ";
                conn.Open();
                var result = await conn.QueryAsync<StationListVM>(sQuery);
                conn.Close();
                conn.Dispose();

                return result.ToList();
            }
        }

        public async Task<StationListVM> FindByIDDapper(int id)
        {
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT s.id, 
                                                    s.name_station AS {nameof(StationListVM.NameStation)},
                                                    s.area_code AS {nameof(StationListVM.AreaCode)},
                                                    a.name_area AS {nameof(StationListVM.NameArea)},
                                                    s.station_uid AS {nameof(StationListVM.StationUid)}
                                        FROM station s
                                            LEFT JOIN area a ON s.area_code = a.code_area
                                        WHERE s.is_deleted != true
                                            AND s.id = @ID
                                        ";
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<StationListVM>(sQuery, new { @ID = id });
                conn.Close();
                conn.Dispose();

                return result;
            }
        }

        public async void Remove(int id)
        {
            var result = await _appDbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
            {
                _appDbContext.Customer.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Station> Delete(int id)
        {
            var result = await _appDbContext.Station.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var itemObj = new Station();
                result.UpdatedAt = itemObj.UpdatedAt;            
                result.UpdatedBy = itemObj.UpdatedBy;
                result.IsDeleted = true;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return result;
        }

        public async Task<Station> Update(Station itemObj)
        {
            var result = await _appDbContext.Station.FirstOrDefaultAsync(x => x.Id == itemObj.Id);

            if (result != null)
            {
                result.NameStation = itemObj.NameStation;
                result.AreaCode = itemObj.AreaCode;
                result.StationUid = itemObj.StationUid;
                result.UpdatedAt = itemObj.UpdatedAt;
                result.UpdatedBy = itemObj.UpdatedBy;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<IEnumerable<Station>> Search(string name)
        {
            IQueryable<Station> query = _appDbContext.Station;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.NameStation.Contains(name)
                                //|| x.Area.NameArea.Contains(name)
                                || x.StationUid.Contains(name));
            }

            return await query.ToListAsync();
        }

        public bool ObjExists(int id)
        {
            return _appDbContext.Station.Any(x => x.Id == id);
        }
    }
}