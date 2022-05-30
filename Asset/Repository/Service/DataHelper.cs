using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class DataHelper
    {

        public static IEnumerable<string> HumanizeBusinessHours(IEnumerable<BranchHours> branchHours)
        {
            var obj = new List<string>();
            foreach (var item in branchHours)
            {
                var daysOfWeek = HumanizeDaysOfWeek(item.DaysOfWeek);

                var openTime = HumanizeTime(item.OpenTime);
                var close = HumanizeTime(item.CloseTime);

                var output = $"{daysOfWeek} {openTime} to {close}";
                obj.Add(output);
            }

            return obj;
        }

        private static string HumanizeTime(int openTime)
        {
            var time = TimeSpan.FromHours(openTime);

            return time.ToString("hh':'mm");
        }

        private static string HumanizeDaysOfWeek(int daysOfWeek)
        {
            return Enum.GetName(typeof(DayOfWeek), daysOfWeek);
        }
    }
}
