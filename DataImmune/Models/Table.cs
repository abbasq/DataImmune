using System;
using System.Collections.Generic;
using System.Text;

namespace DataImmune.Models
{
    public class Table
    {
        public string TableName { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableType { get; set; }        
    }
}
