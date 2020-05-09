using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using TeamProject_p1.Data;


namespace TeamProject_p1.Models
{
    public class DailyTask : IEntity
    {
        public int DailyTaskId { get; set; }

        public Calendar CalendarItem { get; set; }
        public string Description { get; set; }
    }
}