using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.Utility
{
  public static  class SphixHelper
    {
        public static DateTime setDateFromDayName(string dayName, DateTime date)
        {
            DateTime todayDate = DateTime.UtcNow;
            var days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday",
                          "Saturday", "Sunday" };
            int _dayIndex = Array.IndexOf(days, dayName);
            int _todayDayIndex = Array.IndexOf(days, todayDate.DayOfWeek.ToString());
            
                if (_todayDayIndex > _dayIndex)
                {
                    if ((_todayDayIndex-_dayIndex)>1)
                    {
                        _dayIndex = (7 - (_todayDayIndex - _dayIndex));
                        return date.AddDays(_dayIndex);
                    }
                    else
                    {
                        return date.AddDays(7);
                    }

                }
                else if( _dayIndex> _todayDayIndex)
                {

                 return date.AddDays(_dayIndex - 1);
                
                }
                else
                {
                  return date;
                }
        }
    }
}
