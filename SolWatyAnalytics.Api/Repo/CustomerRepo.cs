using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.BLL.DAL;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Api.Repo
{
    public class CustomerRepo : IRepoTask<Customer>
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connStr;

        public CustomerRepo(AppDbContext appDbContext, IOptions<ConnectionStringList> connStr)
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

        public async Task<Customer> Add(Customer itemObj)
        {
            Customer customer = new Customer
            {
                CompanyName = itemObj.CompanyName,
                Address = itemObj.Address,
                City = itemObj.City,
                ContactName = itemObj.ContactName,
                Email = itemObj.Email,
                Uid = itemObj.Uid
            };
                
            await _appDbContext.Customer.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();
            //return result.Entity;
            return customer;
        }        

        public async Task<IEnumerable<Customer>> FindAll()
        {
            #region using Bare Query & Linq
            //string sQuery = @"SELECT * 
            //                        FROM Customer
            //                        WHERE IsDeleted != FALSE";

            //var result = _appDbContext.Customer.FromSql(sQuery).ToList();
            //return result;

            //var query = (from e in _appDbContext.Customer
            //             where e.IsDeleted != true 
            //             select e).ToList();
            #endregion

            #region using EFCore Lamda
            //var result = _appDbContext.Customer
            //                    .Where(x => x.IsDeleted != true)
            //                    .ToListAsync();

            //return await result;   
            #endregion

            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT id, 
                                                    company_name AS {nameof(Customer.CompanyName)},
                                                    address AS {nameof(Customer.Address)},
                                                    city AS {nameof(Customer.City)},
                                                    contact_name AS {nameof(Customer.ContactName)},
                                                    email AS {nameof(Customer.Email)},
                                                    uid AS {nameof(Customer.Uid)}
                                        FROM customer
                                        WHERE is_deleted != true
                                        ";
                conn.Open();
                var result = await conn.QueryAsync<Customer>(sQuery);
                conn.Close();
                conn.Dispose();

                return result.ToList(); 
            }
        }

        public async Task<Customer> FindByID(int id)
        {
            #region using EFCore Lamda
            //var result = _appDbContext.Customer
            //                .Where(x => x.IsDeleted != true)
            //                .FirstOrDefaultAsync(x => x.Id == id);

            //return await result; 
            #endregion

            using (IDbConnection conn = Connection)
            {
                var sQuery = $@"
                                        SELECT id, 
                                                    company_name AS {nameof(Customer.CompanyName)},
                                                    address AS {nameof(Customer.Address)},
                                                    city AS {nameof(Customer.City)},
                                                    contact_name AS {nameof(Customer.ContactName)},
                                                    email AS {nameof(Customer.Email)},
                                                    uid AS {nameof(Customer.Uid)}
                                        FROM customer
                                        WHERE is_deleted != true
                                        AND id = @ID
                                        ";
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<Customer>(sQuery, new { @ID = id });
                conn.Close();
                conn.Dispose();

                return result;
            }
        }        

        public async Task<Customer> Update(Customer itemObj)
        {
            var result = await _appDbContext.Customer.FirstOrDefaultAsync(x => x.Id == itemObj.Id);

            if (result != null)
            {
                result.CompanyName = itemObj.CompanyName;
                result.Address = itemObj.Address;
                result.City = itemObj.City;
                result.ContactName = itemObj.ContactName;
                result.Email = itemObj.Email;
                result.Uid = itemObj.Uid;
                result.UpdatedAt = itemObj.UpdatedAt;
                result.UpdatedBy = itemObj.UpdatedBy;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
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

        public async Task<Customer> Delete(int id)
        {
            var result = await _appDbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var itemObj = new Customer();
                result.UpdatedAt = itemObj.UpdatedAt;
                result.UpdatedBy = itemObj.UpdatedBy;
                result.IsDeleted = true;

                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<Customer>> Search (string name)
        {
            IQueryable<Customer> query = _appDbContext.Customer;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.CompanyName.Contains(name)
                                || x.ContactName.Contains(name)
                                || x.Email.Contains(name));
            }

            return await query.ToListAsync();
        }

        public bool ObjExists(int id)
        {
            return _appDbContext.Area.Any(x => x.Id == id);
        }
    }
}