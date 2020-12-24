using Microsoft.EntityFrameworkCore;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.BLL.DAL;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using Npgsql;

namespace SolWatyAnalytics.Api.Repo
{
    public class AreaRepo : IRepoTask<Area>
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connStr;

        public AreaRepo(AppDbContext appDbContext, IOptions<ConnectionStringList> connStr)
        {
            _appDbContext = appDbContext;
            _connStr = connStr.Value.LocalPgSQL;
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connStr);
            }
        }

        public async Task<Area> Add(Area itemObj)
        {
            Area area = new Area()
            {
                CodeArea = itemObj.CodeArea,
                NameArea = itemObj.NameArea
            };

            await _appDbContext.Area.AddAsync(area);
            await _appDbContext.SaveChangesAsync();
            return area;
        }

        public async Task<IEnumerable<Area>> FindAll()
        {
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT id,
                                                    code_area AS {nameof(Area.CodeArea)},
                                                    name_area AS {nameof(Area.NameArea)}
                                        FROM area
                                        WHERE is_deleted != true
                                        ";
                conn.Open();
                var result = await conn.QueryAsync<Area>(sQuery);
                conn.Close();
                conn.Dispose();

                return result.ToList();
            }
        }

        public async Task<Area> FindByID(int id)
        {
            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT id,
                                                    code_area AS {nameof(Area.CodeArea)},
                                                    name_area AS {nameof(Area.NameArea)}
                                        FROM area
                                        WHERE is_deleted != true
                                        AND id = @ID
                                        ";
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<Area>(sQuery, new { @ID = id });
                conn.Close();
                conn.Dispose();

                return result;
            }
        }

        public async void Remove(int id)
        {
            var result = await _appDbContext.Area.FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
            {
                _appDbContext.Area.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Area> Update(Area itemObj)
        {
            var result = await _appDbContext.Area.FirstOrDefaultAsync(x => x.Id == itemObj.Id);

            if (result != null)
            {
                result.CodeArea = itemObj.CodeArea;
                result.NameArea = itemObj.NameArea;
                result.UpdatedAt = itemObj.UpdatedAt;
                result.UpdatedBy = itemObj.UpdatedBy;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }     

        public async Task<Area> Delete(int id)
        {
            var result = await _appDbContext.Area.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var itemObj = new Area();
                result.UpdatedAt = itemObj.UpdatedAt;
                result.UpdatedBy = itemObj.UpdatedBy;
                result.IsDeleted = true;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<Area>> Search(string name)
        {
            IQueryable<Area> query = _appDbContext.Area;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.NameArea.Contains(name)
                                //|| x.Area.NameArea.Contains(name)
                                || x.CodeArea.Contains(name));
            }

            return await query.ToListAsync();
        }

        public bool ObjExists(int id)
        {
            return _appDbContext.Area.Any(x => x.Id == id);
        }
    }
}