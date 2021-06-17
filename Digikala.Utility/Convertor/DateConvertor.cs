using System;
using System.Globalization;
using MD.PersianDateTime.Core;

namespace Digikala.Utility.Convertor
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static string GetYearShamsi(this DateTime value)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(value).ToString("0000");
        }

        public static string ToShamsiByTime(this DateTime value)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + "\r \n" + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00");
        }

        public static DateTime ToMiladi(this string value)
        {
            var date = value.Split("/");

            return new DateTime(int.Parse(date[0].ToString()),
                int.Parse(date[1].ToString()),
                int.Parse(date[2].ToString()),
                new PersianCalendar());
        }

        /// <summary>
        /// فرمت های موجود را بیان کردم
        /// </summary>
        /// <param name="dateValue">format "/"</param>
        /// <param name="timeValue">format ":"</param>
        /// <returns></returns>
        public static DateTime ToMiladiByTime(string dateValue, string timeValue)
        {
            var date = dateValue.Split("/");
            var time = timeValue.Split(":");

            return new DateTime(
                int.Parse(date[0]),
                int.Parse(date[1]),
                int.Parse(date[2]),
                int.Parse(time[0]),
                int.Parse(time[1]),
                second: 00,
                calendar: new PersianCalendar());
        }

        /// <summary>
        /// این بخش به هماره فرمت های فارسی میباشد
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ConvertMiladiToShamsi(this DateTime date, string format)
        {
            var persianDateTime = new PersianDateTime(date);
            return persianDateTime.ToString(format: format);
        }


    }
}
