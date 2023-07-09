using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KaloKalo.Functions
{
    public class RegionHelper
    {
        static private string theDate = "";

        public static string NextSunday()
        {
            var date = DateTime.Now;
            var nextSunday = date.AddDays(7 - (int)date.DayOfWeek);
            return nextSunday.ToString();
        }
        
        public static string FirstDayOfNextMonth()
        {
            DateTime dt = DateTime.Now.AddMonths(1);
            DateTime dayone = new DateTime(dt.Year, dt.Month, 1);
            return dayone.ToString();
        }

        public static bool CheckTimeFormat(string timeInput)
        {
            //Check if string is 8 characters long
            if (timeInput.Length == 8)
            {
                if (Char.IsDigit(char.Parse(timeInput.Substring(0, 1))) && Char.IsDigit(char.Parse(timeInput.Substring(1, 1))) && timeInput.Substring(2, 1) == ":" && Char.IsDigit(char.Parse(timeInput.Substring(3, 1))) && Char.IsDigit(char.Parse(timeInput.Substring(4, 1))) && timeInput.Substring(5, 1) == " " && (timeInput.Substring(6, 1) == "a" || timeInput.Substring(6, 1) == "p") && timeInput.EndsWith("m"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // function to get the full month name
        public static string GetMonthFullName(int month)
        {
            return CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName
                (month);
        }

        public static string GetDayOfTheWeek()
        {
            return DateTime.Now.ToString("dddd");
        }

        public static string GetFullDateWithTimeAMPM()
        {
            return GetDateToday() + " | " + GetTimeNowWithAMPM();
        }

        public static string GetFullDateWithTime()
        {
            return GetDateToday() + " | " + GetTimeNow();
        }

        public string ConvertDate(string prefFormat)
        {
            DateTime newDate = DateTime.ParseExact(theDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return newDate.ToString(prefFormat);
        }

        public static string GetDateToday()
        {
            StringBuilder sb = new StringBuilder("" + (DateTime.Now.Day.ToString().Length < 2 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString()));
            sb.Append("-" + (DateTime.Now.Month.ToString().Length < 2 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()));
            sb.Append("-" + DateTime.Now.Year);
            return sb.ToString();
        }

        public static string GetTimeNow()
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.Hour.ToString().Length < 2 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString());
            sb.Append(":" + (DateTime.Now.Minute.ToString().Length < 2 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString()));
            return sb.ToString();
        }

        public static string GetTimeNowWithAMPM()
        {
            DateTime dateTime = new DateTime();
            return dateTime.ToString("hh:mm tt");
        }

        public static string GetCountryFromAddress()
        {
            return RegionInfo.CurrentRegion.DisplayName;
        }

        public static DateTime CookieExpirationPeriod()
        {
            return DateTime.Now.AddDays(365);
        }
    }
}
