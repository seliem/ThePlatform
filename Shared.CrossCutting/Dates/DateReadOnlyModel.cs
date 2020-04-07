using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.CrossCutting.Dates
{
    public class DateReadOnlyModel
    {
        public string GregorianDate { get; set; }

        public string GregorianDayOfWeek { get; set; }

        public string HijriDate { get; set; }

        public string HijriDayOfWeek { get; set; }

        public string DateFormat { get; set; }

        public string Time { get; set; }

        public string TimeFormat { get; set; }
    }
}
