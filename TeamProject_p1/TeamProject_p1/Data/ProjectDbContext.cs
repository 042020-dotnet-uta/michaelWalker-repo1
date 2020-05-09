using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamProject_p1.Models;

namespace TeamProject_p1.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        { }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<Calendar> CalendarDates { get; set; }

        public void AddNewTask(DailyTask dailyTask)
        {
            this.Add(dailyTask);
            this.SaveChanges();
        }

        public DailyTask GetTaskDetails(int id)
        {
            var dailyTask = this.DailyTasks
                .AsNoTracking()
                .FirstOrDefault(m => m.DailyTaskId == id);

            return dailyTask;
        }
        public IEnumerable<DailyTask> GetAllTasks()
        {
            var tasks = this.DailyTasks.AsNoTracking().ToList();
            return tasks;
        }

        public void DeleteDailyTask(int id)
        {
            var dailyTask = this.DailyTasks.Find(id);
            this.DailyTasks.Remove(dailyTask);
            this.SaveChanges();
        }

        public void UpdateDailyTask(DailyTask dailyTask)
        {
            try
            {
                this.Update(dailyTask);
                this.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyTaskExists(dailyTask.DailyTaskId))
                {
                    AddNewTask(dailyTask);
                }
                else
                {
                    throw;
                }
            }
        }
        private bool DailyTaskExists(int id)
        {
            return this.DailyTasks.Any(e => e.DailyTaskId == id);

        }
    }
}
