using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace Pas.Common.Extensions
{
    /// <summary>
    ///     Extension methods for enumerarions
    /// </summary>
    /// <remarks>
    ///     Provide access to attributes or store logic relevant to enumeration item display.
    /// </remarks>
    public static class DateExtensions
    {
        /// <summary>
        ///     Get the next day of week
        /// </summary>
        /// <param name="start"></param>
        /// <param name="day"></param>
        /// <returns>string</returns>
        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime GetLastWeekday(this DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int) day + (int) start.DayOfWeek - 7) % 7;
            return start.AddDays(daysToAdd);
        }


        /// <summary>
        ///     Returns the first day of the week that the specified
        ///     date is in using the current culture.
        /// </summary>
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek)
        {
            var defaultCultureInfo = CultureInfo.CurrentUICulture;
            return dayInWeek.GetFirstDayOfWeek(defaultCultureInfo);
        }

        /// <summary>
        ///     Returns the first day of the week that the specified date
        ///     is in.
        /// </summary>
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            var firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            return firstDayInWeek;
        }

        public static bool IsInWeek(this DateTime date, DateTime checkWeekDate)
        {
            return date >= checkWeekDate.GetFirstDayOfWeek() && date < checkWeekDate.GetFirstDayOfWeek().AddDays(7);
        }

        /// <summary>
        /// "dd/MM/yyyy hh:mm" eg 01/12/2000 05:12 PM
        /// </summary>
        public static string ToDisplay(this DateTime input)
        {
            return input.ToString("dd/MM/yyyy HH:mm");            
        }

        /// <summary>
        /// "dd/MM/yyyy hh:mm" eg 01/12/2000 05:12 PM
        /// </summary>
        public static string ToDisplay(this DateTime? input)
        {
            return input.HasValue ? input.Value.ToDisplay() : String.Empty;
        }

        public static string ToDD_MM_YYYY(this DateTime input)
        {
            return input.ToString("dd_MM_yyyy");
        }

        public static string ToYYYYMMDD(this DateTime input)
        {
            return input.ToString("yyyyMMdd");
        }

        public static string ToYYYYMMDD(this DateTime? input)
        {
            return (input.HasValue) ? input.Value.ToYYYYMMDD() : String.Empty;
        }

        /// <summary>This is for Calendar Control Date Format, eg: '02 May 2020'</summary>
        public static string ToDDMMMYYYY(this DateTime input)
        {
            return (input == null) ? "" : input.ToString("dd MMM yyyy");
        }

        /// <summary>This is for Calendar Control Date Format, eg: '02 May 2020'</summary>
        public static string ToDDMMMYYYY(this DateTime? input)
        {
            return input.HasValue ? input.Value.ToDDMMMYYYY() : string.Empty;
        }

        /// <summary>
        /// To Format DD/MM/YYYY
        /// </summary>
        public static string ToShortDateString(this DateTime? input)
        {
            return input.HasValue ? input.Value.ToShortDateString() : String.Empty;
        }

        /// <summary>Returns the Hour and Minute part of the Time, ie: HH_MM</summary>
        /// <param name="input">DateTime</param>
        /// <returns>Hour and Minutes</returns>
        public static string ToHHMM(this DateTime input)
        {
            return (input == null) ? "" : input.ToString("HH_MM");
        }
    }
}