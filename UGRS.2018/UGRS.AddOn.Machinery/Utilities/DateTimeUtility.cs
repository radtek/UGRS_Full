using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Utilities
{
    public class DateTimeUtility
    {
        public static int GetTotalDays(DateTime pObjStartDate, DateTime pObjEndDate)
        {
            TimeSpan lObjTimeSpan = pObjStartDate - pObjEndDate;
            int lIntDays = Math.Abs(lObjTimeSpan.Days);
            return lIntDays;
        }

        public static DateTime StringToDateTime(string pStrDate)
        {
            return DateTime.Parse(string.Format("{0}/{1}/{2}", pStrDate.Substring(0, 4), pStrDate.Substring(4, 2), pStrDate.Substring(6, 2)));
        }
    }
}
