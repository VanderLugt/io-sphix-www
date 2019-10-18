
using System;

namespace Sphix.Utility.DateTimeDifference
{
    public class UDateTimeDifference : IUDateTimeDifference
    {
        public bool CheckTimeIsExpiredDays(DateTime maxDateTime, DateTime minDateTime, int comparingValueInDays)
        {
            TimeSpan diff = maxDateTime - minDateTime;
            if (diff.Days > comparingValueInDays)
            {
                return true;
            }
            return false;
        }
        public bool CheckTimeIsExpiredMinutes(DateTime maxDateTime, DateTime minDateTime, int comparingValueInMinutes)
        {
            TimeSpan diff = maxDateTime - minDateTime;
            if (diff.Minutes > comparingValueInMinutes)
            {
                return true;
            }
            return false;
        }
        public bool CheckTimeIsExpiredHours(DateTime maxDateTime, DateTime minDateTime, int comparingValueInHours)
        {
            TimeSpan diff = maxDateTime - minDateTime;
            if (diff.Hours > comparingValueInHours)
            {
                return true;
            }
            return false;
        }
    }
   
   
}
