using System;
using System.Collections.Generic;
using System.Text;

namespace DataImmune.Models
{
    public class AppText
    {
        public int ID { get; set; }
        public int Deleted { get; set; }
        public string StatusCode { get; set; }
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Subkey { get; set; }
        public string Text { get; set; }

    }
}
