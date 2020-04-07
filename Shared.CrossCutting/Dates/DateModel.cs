using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace Shared.CrossCutting.Dates
{
    public class DateModel
    {
        [Required]
        public string Date { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarType CalendarType { get; set; }

        ///// <summary>
        ///// allFormats ={"yyyy/MM/dd","yyyy/M/d","dd/MM/yyyy","d/M/yyyy","dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd","yyyy-M-d","dd-MM-yyyy","d-M-yyyy","dd-M-yyyy","d-MM-yyyy","yyyy MM dd","yyyy M d","dd MM yyyy","d M yyyy","dd M yyyy","d MM yyyy"}
        ///// </summary>
        //public string DateFormat { get; set; }

    }
    public class OptionalDateModel
    {
        public string Date { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarType CalendarType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CalendarType
    {
        Gregorian,
        Hijri
    }
}
