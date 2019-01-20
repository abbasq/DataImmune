using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataImmune.Handlers
{
    public class GetLongDataQueryHandler
    {
        private string connectionString;

        public GetLongDataQueryHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<long> Execute(string query)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                return conn.Query<long>(query);
            }
        }
    }
}
