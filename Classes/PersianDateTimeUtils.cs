using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
namespace PCLOR.Classes
{
    public static class PersianDateTimeUtils
    {
        /// <summary>
        /// تعیین اعتبار تاریخ شمسی
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        public static bool IsValidPersianDate(int persianYear, int persianMonth, int persianDay)
        {
            if (persianDay > 31 || persianDay <= 0)
            {
                return false;
            }

            if (persianMonth > 12 || persianMonth <= 0)
            {
                return false;
            }

            if (persianMonth <= 6 && persianDay > 31)
            {
                return false;
            }

            if (persianMonth >= 7 && persianDay > 30)
            {
                return false;
            }

            if (persianMonth == 12)
            {
                var persianCalendar = new PersianCalendar();
                var isLeapYear = persianCalendar.IsLeapYear(persianYear);

                if (isLeapYear && persianDay > 30)
                {
                    return false;
                }

                if (!isLeapYear && persianDay > 29)
                {
                    return false;
                }
            }

            return true;
        }

       

       

        
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateString(this DateTime? dt)
        {
            return dt == null ? string.Empty : ToLongPersianDateString(dt.Value);
        }

        
       
        

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395، 10:20:02 ق.ظ
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateTimeString(this DateTime? dt)
        {
            return dt == null ? string.Empty : ToLongPersianDateTimeString(dt.Value);
        }

        
        
      
       
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        public static Tuple<int, int, int> ToPersianYearMonthDay(this DateTime? gregorianDate)
        {
            return gregorianDate == null ? null : ToPersianYearMonthDay(gregorianDate.Value);
        }

        
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        public static Tuple<int, int, int> ToPersianYearMonthDay(this DateTime gregorianDate)
        {
            var persianCalendar = new PersianCalendar();
            var persianYear = persianCalendar.GetYear(gregorianDate);
            var persianMonth = persianCalendar.GetMonth(gregorianDate);
            var persianDay = persianCalendar.GetDayOfMonth(gregorianDate);
            return new Tuple<int, int, int>(persianYear, persianMonth, persianDay);
        }

       

       
       
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateString(this DateTime? dt)
        {
            return dt == null ? string.Empty : ToShortPersianDateString(dt.Value);
        }

        
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21 10:20
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateTimeString(this DateTime? dt)
        {
            return dt == null ? string.Empty : ToShortPersianDateTimeString(dt.Value);
        }

        

        

        private static int? getDay(string part)
        {
            var day = part.toNumber();
            if (!day.Item1) return null;
            var pDay = day.Item2;
            if (pDay == 0 || pDay > 31) return null;
            return pDay;
        }

        private static int? getMonth(string part)
        {
            var month = part.toNumber();
            if (!month.Item1) return null;
            var pMonth = month.Item2;
            if (pMonth == 0 || pMonth > 12) return null;
            return pMonth;
        }

        private static int? getYear(string part)
        {
            var year = part.toNumber();
            if (!year.Item1) return null;
            var pYear = year.Item2;
            if (part.Length == 2) pYear += 1300;
            return pYear;
        }

        private static Tuple<bool, int> toNumber(this string data)
        {
            int number;
            bool result = int.TryParse(data, out number);
            return new Tuple<bool, int>(result, number);
        }
    }
}
