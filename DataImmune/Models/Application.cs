using System;
using System.Collections.Generic;
using System.Text;

namespace DataImmune.Models
{
    public class Application
    {
        public int ID { get; set; }
        public int Deleted { get; set; }
        public string StatusCode { get; set; }
        public int ApplicationTypeId { get; set; }
        public string Name { get; set; }
        public string InternalDescription { get; set; }
        public string ProgramUrl { get; set; }
        public string DbConnectionString { get; set; }

    }
}
