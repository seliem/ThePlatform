using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace Shared.CrossCutting.Dates
{
    public static class DateUtils
    {
        private const int startGreg = 1900;
        private const int endGreg = 2100;

        private static string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy","dddd، dd MMMM، yyyy"};

        private static CultureInfo arCul;
        private static CultureInfo enCul;
        private static UmAlQuraCalendar h;
        private static GregorianCalendar g;

        static DateUtils()
        {
            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            h = new UmAlQuraCalendar();
            g = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            arCul.DateTimeFormat.Calendar = h;
        }

        public static bool IsHijri(string hijri)
        {
            if (hijri.Length <= 0)
            {
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri, allFormats,
                     arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch 
            {
                return false;
            }
        }

        public static bool IsGreg(string greg)
        {
            if (greg.Length <= 0)
            {
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static string FormatHijri(string hdate, string format)
        {
            if (hdate.Length <= 0)
            {
                return "";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hdate,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, arCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string FormatGreg(string gdate, string format)
        {
            if (gdate.Length <= 0)
            {
                return "";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(gdate, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, enCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string GDateNow(string format = "yyyy-MM-dd")
        {
            try
            {
                return DateTime.Now.ToString(format, enCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string HDateNow(string format = "yyyy-MM-dd")
        {
            try
            {
                return DateTime.Now.ToString(format, arCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string HijriToGreg(string hijri, string format = "yyyy-MM-dd")
        {
            if (hijri.Length <= 0)
            {
                return "";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, enCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string GregToHijri(string greg, string format = "yyyy-MM-dd")
        {
            if (greg.Length <= 0)
            {
                return "";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, arCul.DateTimeFormat);
            }
            catch 
            {
                return "";
            }
        }

        public static string GTimeStamp()
        {
            return GDateNow("yyyyMMddHHmmss");
        }

        public static string HTimeStamp()
        {
            return HDateNow("yyyyMMddHHmmss");
        }

        public static int Compare(string d1, string d2, bool ignoreTime = true)
        {
            try
            {
                DateTime date1 = DateTime.ParseExact(d1, allFormats,
                    arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                DateTime date2 = DateTime.ParseExact(d2, allFormats,
                    arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                if (ignoreTime)
                {
                    return DateTime.Compare(date1.Date, date2.Date);
                }
                 return DateTime.Compare(date1, date2);
            }
            catch 
            {
                return -1;
            }
        }// Compare

        public static int Compare(DateModel source, DateModel destination, bool ignoreTime = true)
        {
            var d1 = ToDate(source);
            var d2 = ToDate(destination);
            if (ignoreTime)
            {
                return DateTime.Compare(d1.Date, d2.Date);
            }
            return DateTime.Compare(d1, d2);
        }// Compare

        public static int Compare(DateTime source, DateTime destination, bool ignoreTime = true)
        {
            if (ignoreTime)
            {
                return DateTime.Compare(source.Date, destination.Date);
            }
            return DateTime.Compare(source, destination);
        }// Compare

        public static DateReadOnlyModel ToModel(DateTime? date, string format = "yyyy-MM-dd")
        {
            try
            {
                if (date != null && date != DateTime.MinValue)
                {
                    var model = new DateReadOnlyModel();

                    model.DateFormat = format;
                    model.GregorianDate = date.Value.ToString(format, enCul.DateTimeFormat);
                    model.GregorianDayOfWeek = date.Value.ToString("dddd", enCul.DateTimeFormat);
                    model.HijriDate = date.Value.ToString(format, arCul.DateTimeFormat);
                    model.HijriDayOfWeek = date.Value.ToString("dddd", arCul.DateTimeFormat);
                    model.Time = date.Value.ToString("HH:mm", enCul.DateTimeFormat);
                    model.TimeFormat = "HH:mm";

                    return model;
                }

                return null;
            }
            catch 
            {
                return null;
            }
        }

        public static DateTime ToDate(DateModel model)
        {
            try
            {
                if (model != null)
                {
                    DateTime date = DateTime.MinValue;
                    if (model.CalendarType == CalendarType.Hijri || IsHijri(model.Date))
                    {
                        var dt = HijriToGreg(model.Date);
                        DateTime.TryParseExact(dt, allFormats, enCul.DateTimeFormat,
                            DateTimeStyles.AllowWhiteSpaces, out date);

                    }
                    else
                    {
                        DateTime.TryParseExact(model.Date, allFormats, enCul.DateTimeFormat,
                                    DateTimeStyles.AllowWhiteSpaces, out date);
                    }

                    return date;
                }
                return DateTime.MinValue;
            }
            catch 
            {
                return DateTime.MinValue;
            }
        }

        public static string NumarizeDate(string date)
        {
            if (date != null && (date.Contains('-') || date.Contains('/')))
            {
                string[] dateArr = null;
                if (date.Contains('-'))
                    dateArr = date.Split('-');

                if (date.Contains('/'))
                    dateArr = date.Split('/');

                if (dateArr != null && dateArr.Length == 3)
                    return $"{dateArr[0]}{dateArr[1]}{dateArr[2]}";
            }
            return date;
        }
    }// DateUtils
}
