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
        public static DateTime getDateOnQuarterlyBase(string dayOfWeek, int dayIndex)
        {
            DateTime currentMonth = DateTime.UtcNow;
            DateTime _date;

            int _quarter=(currentMonth.Month + 2) / 3;

            _date = new DateTime(currentMonth.Year, (_quarter* 3), 1);

            _date = FindNext(getDayOfWeek(dayOfWeek), _date);
            for (int i = 1; i < dayIndex; i++)
            {
                _date = FindNext(getDayOfWeek(dayOfWeek), _date.AddDays(i));
            }
            return _date;
        }
        public static DateTime getDateOnMonthlyBase(string dayOfWeek,int dayIndex)
        {
            DateTime currentMonth = DateTime.UtcNow;
            var _date = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            
            _date = FindNext(getDayOfWeek(dayOfWeek), _date);
            //DayOfWeek
            for (int i = 1; i < dayIndex; i++)
            {
                _date = FindNext(getDayOfWeek(dayOfWeek), _date.AddDays(i));
            }
            return _date;
        }

        private static DateTime FindNext(DayOfWeek dayOfWeek, DateTime after)
        {
            DateTime day = after;
            while (day.DayOfWeek != dayOfWeek) day = day.AddDays(1);
            return day;
        }
        public static  DayOfWeek getDayOfWeek(string dayName)
        {
            switch (dayName.ToLower())
            {
                case "monday":
                    return DayOfWeek.Monday;
                case "tuesday":
                    return DayOfWeek.Thursday;
                case "wednesday":
                    return DayOfWeek.Wednesday;
                case "thursday":
                    return DayOfWeek.Thursday;
                case "friday":
                    return DayOfWeek.Friday;
                case "saturday":
                    return DayOfWeek.Saturday;
                default:
                     return DayOfWeek.Sunday;
                    // etc boring ....
            }
         
        }
    }
}
