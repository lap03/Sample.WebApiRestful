using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sample.WebApiRestful.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data
{
    public class DapperHelper : IDapperHelper
    {
        private readonly string connectString = string.Empty;
        public DapperHelper(IConfiguration configuration)
        {
            connectString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task ExcuteNotReturn(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConnection = new SqlConnection(connectString))
            {
                await dbConnection.ExecuteAsync(query, parameters, dbTransaction, commandType: CommandType.Text);
            }
        }

        public async Task<T> ExcuteReturnScalar<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConnection = new SqlConnection(connectString))
            {
                return (T)(await dbConnection.ExecuteScalarAsync(query, parameters, dbTransaction, commandType: CommandType.Text));
            }
        }

        public async Task<IEnumerable<T>> ExecuteSqlReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConnection = new SqlConnection(connectString))
            {
                return await dbConnection.QueryAsync<T>(query, parameters, dbTransaction, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConnection = new SqlConnection(connectString))
            {
                return await dbConnection.QueryAsync<T>(query, parameters, dbTransaction, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
