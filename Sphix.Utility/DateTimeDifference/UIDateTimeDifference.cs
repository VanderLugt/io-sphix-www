using System;

namespace Sphix.Utility.DateTimeDifference
{/// <summary>
 /// Days,Minutes,Hours
 /// </summary>
    public interface IUDateTimeDifference
    {
       bool CheckTimeIsExpiredDays(DateTime maxDateTime, DateTime minDateTime, int comparingValue);
        bool CheckTimeIsExpiredMinutes(DateTime maxDateTime, DateTime minDateTime, int comparingValueInMinutes);
        bool CheckTimeIsExpiredHours(DateTime maxDateTime, DateTime minDateTime, int comparingValueInHours);
    }
}
