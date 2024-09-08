using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data.Abstract
{
    public interface IDapperHelper
    {
        /// <summary>
        /// Execute query not return
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        Task ExcuteNotReturn(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null);
        Task<T> ExcuteReturnScalar<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteSqlReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null);
    }
}
