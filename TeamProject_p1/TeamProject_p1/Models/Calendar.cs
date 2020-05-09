using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using TeamProject_p1.Data;

namespace TeamProject_p1.Models
{
    public class Calendar : IEntity
    {
        public int CalendarId { get; set; }
        public DateTime TaskDate { get; set; }
    }
}
