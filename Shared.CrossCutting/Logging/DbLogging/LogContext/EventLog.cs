using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Logging.LogContext
{
   public class EventLog
    {
       
        public int Id { get; set; }
        public Guid? LogKey { get; set; }
        public string StatusCode { get; set; }
        public string StepName { get; set; }
        public string LogExceptions { get; set; }
        public string ProcessName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Model { get; set; }
        public DateTime LogTime { get; set; }
        public string TakeTime { get; set; }
        [NotMapped]
        public int Index { get; internal set; }
    }
}
