using System;
using Tools;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataImmune.Models;
using DataImmune;
using Microsoft.Extensions.Options;

namespace DataImmune.Controllers
{
    public class BasicDBOperations : Controller
    {
        public string Cs1 { get; set; }
        public BasicDBOperations(string connectionString)
        {
            Cs1 = connectionString;
        }
        public List<Application> GetApplications()
        {
            using (IDbConnection db = new SqlConnection(Cs1))
            {
                return db.Query<Application>
                    ("Select * from Application").ToList();
            }
        }

        public List<AppText> GetTextByKey(int languageId, string key)
        {
            using (IDbConnection db = new SqlConnection(Cs1))
            {
                return db.Query<AppText>
                    ("Select * from Text where LanguageId = " + languageId + " and [Key] = '" + key + "'").ToList();
            }
        }
    }
}
