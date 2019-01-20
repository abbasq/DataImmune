using DataImmune.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataImmune.Controllers
{
    public class AnomalyDetectionLong
    {
        private readonly string connectionString;

        public AnomalyDetectionLong(): this("data source=172.16.64.40; Initial Catalog=AvaInvTest; user Id=AvaInvTestWeb ; Password=2UCradux")
        {
        }
        public AnomalyDetectionLong(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private  IEnumerable<long> GetCheckData()
        {
            string query = "SELECT CAST(ApprovedAmountUSD AS BIGINT) FROM  dbo.WebServiceClaim";
            var handler = new GetLongDataQueryHandler(connectionString);
            return handler.Execute(query);            
        }

        private string DetectAnomoly(IEnumerable<long> numbers)
        {

            //check the length of digits

            //check 10s, 100s, 1000s, 10000s, 100000s, 1000000s...

            //ratio of each group total by total records, less then 5% is strange

            //max number in nearest group and differece is 2x max number

            var aggressiveness = 0.01m;

            var numberGroups = GetNumberGroups(numbers);

            var numberRatios = GetNumberRatio(numbers, numberGroups);

            var minRatios = numberRatios.Where(x => x.Value > 0 && x.Value <= aggressiveness);

            var output = "";

            foreach (var item in minRatios)
            {
                output += string.Join(";", numberGroups.Where(x => x.Key == item.Key).FirstOrDefault().Value) + "---";
            }

            return output.ToString();
        }

        private Dictionary<long, decimal> GetNumberRatio(IEnumerable<long> numbers, Dictionary<long, IEnumerable<long>> numberGroups)
        {
            Dictionary<long, decimal> ratios = new Dictionary<long, decimal>();
            var totalRecords = numbers.Count();
            foreach (var item in numberGroups)
            {
                ratios.Add(item.Key, Convert.ToDecimal(item.Value.Count()) / Convert.ToDecimal(totalRecords));
            }

            return ratios;
        }

        private Dictionary<long, IEnumerable<long>> GetNumberGroups(IEnumerable<long> numbers)
        {
            Dictionary<long, IEnumerable<long>> numberGroups = new Dictionary<long, IEnumerable<long>>();
            List<long> list10 = new List<long>();
            List<long> list100 = new List<long>();
            List<long> list1000 = new List<long>();
            List<long> list10000 = new List<long>();
            List<long> list100000 = new List<long>();
            List<long> list1000000 = new List<long>();
            List<long> list10000000 = new List<long>();
            List<long> list100000000 = new List<long>();
            List<long> list1000000000 = new List<long>();
            List<long> list10000000000 = new List<long>();
            List<long> list100000000000 = new List<long>();
            List<long> list1000000000000 = new List<long>();
            foreach (long num in numbers)
            {
                if (num / 10 >= 1 && num / 10 < 10)
                {
                    list10.Add(num);
                }
                if (num / 100 >= 1 && num / 100 < 10)
                {
                    list100.Add(num);
                }
                if (num / 1000 >= 1 && num / 1000 < 10)
                {
                    list1000.Add(num);
                }
                if (num / 10000 >= 1 && num / 10000 < 10)
                {
                    list10000.Add(num);
                }
                if (num / 100000 >= 1 && num / 100000 < 10)
                {
                    list100000.Add(num);
                }
                if (num / 1000000 >= 1 && num / 1000000 < 10)
                {
                    list1000000.Add(num);
                }
                if (num / 10000000 >= 1 && num / 10000000 < 10)
                {
                    list10000000.Add(num);
                }
                if (num / 100000000 >= 1 && num / 100000000 < 10)
                {
                    list100000000.Add(num);
                }
                if (num / 1000000000 >= 1 && num / 1000000000 < 10)
                {
                    list1000000000.Add(num);
                }
                if (num / 10000000000 >= 1 && num / 10000000000 < 10)
                {
                    list10000000000.Add(num);
                }
                if (num / 100000000000 >= 1 && num / 100000000000 < 10)
                {
                    list100000000000.Add(num);
                }
                if (num / 1000000000000 >= 1 && num / 1000000000000 < 10)
                {
                    list1000000000000.Add(num);
                }
            }

            numberGroups.Add(10, list10);
            numberGroups.Add(100, list100);
            numberGroups.Add(1000, list1000);
            numberGroups.Add(10000, list10000);
            numberGroups.Add(100000, list100000);
            numberGroups.Add(1000000, list1000000);
            numberGroups.Add(10000000, list10000000);
            numberGroups.Add(100000000, list100000000);
            numberGroups.Add(1000000000, list1000000000);
            numberGroups.Add(10000000000, list10000000000);
            numberGroups.Add(100000000000, list100000000000);
            numberGroups.Add(1000000000000, list1000000000000);

            return numberGroups;
        }
    }
}
