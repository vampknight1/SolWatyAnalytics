using Microsoft.AspNetCore.Components;
using Npgsql;
using SolWatyAnalytics.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Services
{
    public class StationXService : IStationXService
    {
        private readonly HttpClient _httpClient;

        public StationXService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<StationX>> GetStationXs()
        {
            return await _httpClient.GetJsonAsync<StationX[]>("api/StationX");
        }

        //private string _connStr = configuration.GetValue<string>("ConnectionString:localPgSQL");
        private string _connStr = @"server=127.0.0.1;user id=postgres;password=P05tgre5123!;database=r_wwc;";
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connStr);
            }
        }

        public DataTable GetStationInfoList()
        {
            var dt = new DataTable();

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
                                    WHERE ""TimeStamp2""::date = '2020-09-08'
                                    ORDER BY ""TimeStamp"" DESC
                                    ";
                conn.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sQuery, _connStr);
                da.Fill(dt);
                conn.Close();
                conn.Dispose();

                return dt;
            }
        }

        public DataTable GetStationInfoListbyDate(string tanggal)
        {
            var dt = new DataTable();

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
                                    WHERE ""TimeStamp2""::date = @selectDate
                                    ORDER BY ""TimeStamp"" DESC
                                    ";
                conn.Open();
                //var result = conn.QueryFirstOrDefault<StationX>(sQuery);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sQuery, _connStr);
                da.SelectCommand.Parameters.AddWithValue("@selectDate", tanggal);
                da.Fill(dt);
                conn.Close();
                conn.Dispose();

                return dt;
            }
        }
    }
}