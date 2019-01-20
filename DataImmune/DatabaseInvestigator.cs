using System;
using Tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataImmune.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DataImmune
{
    public class DatabaseInvestigator
    {
        public string Cs1 { get; set; }
        public DatabaseInvestigator(string connectionString)
        {
            Cs1 = connectionString;
        }
        public List<BirchClassicUser> GetBirchUsers(Application application)
        {
            string query = "Select TOP 10 * from ";
            using (IDbConnection db = new SqlConnection(application.DbConnectionString))
            {
                switch (application.ApplicationTypeId)
                {
                    case 1:
                        query = query + "dbo.[User]";
                        break;
                    case 2:
                        query = query + "tblUser";
                        break;
                    default:
                        break;
                }
                return db.Query<BirchClassicUser>
                    (query).ToList();
            }
        }
        public List<Table> PeekDataBase (string dbName)
        {
            string query = @"SELECT TABLE_NAME AS TableName, TABLE_CATALOG AS TableCatalog, TABLE_SCHEMA AS TableSchema, TABLE_TYPE AS TableType
                             FROM INFORMATION_SCHEMA.TABLES
                             WHERE TABLE_TYPE = 'BASE TABLE'
                             ORDER BY TableName";
            using (IDbConnection db = new SqlConnection(Cs1))
            {
                return db.Query<Table>
                    (query).ToList();
            }            
        }

        public List<DataColumn> PeekTable(string tableName)
        {
            string query = "SELECT * FROM information_schema.columns WHERE table_name = '" + tableName + "'"; //Get query from Graham
            using (IDbConnection db = new SqlConnection(Cs1))
            {
                return db.Query<DataColumn>
                    (query).ToList();
            }
        }

        public List<object> AnonymousTest(string foo)
        {
            string query = "SELECT * FROM tblUser";
            var p = new { ID = 1, Name = "abc" };
            using (IDbConnection db = new SqlConnection(Cs1))
            {
                return db.Query<object>
                    (query).ToList();
            }
        }
        
        
    }
}
