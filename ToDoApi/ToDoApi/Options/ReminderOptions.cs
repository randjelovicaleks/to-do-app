using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Utils
{
    public class ReminderOptions
    {
        public const string Reminder = "Reminder";

        public string SendGridKey { get; set; }
        public int Interval { get; set; }
        public string EmailFrom { get; set; }
        public string NameFrom { get; set; }
        public string Subject { get; set; }
        public string NameTo { get; set; }
        public string ToDoListLink { get; set; }
    }
}
