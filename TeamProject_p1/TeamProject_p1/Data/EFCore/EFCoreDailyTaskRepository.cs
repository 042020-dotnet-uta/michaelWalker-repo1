using TeamProject_p1.Models;

namespace TeamProject_p1.Data.EFCore
{
  public class EFCoreDailyTaskRepository : EFCoreRepository<DailyTask, ProjectDbContext>
  {
    public EFCoreDailyTaskRepository(ProjectDbContext context) : base(context)
    {

    }

    
  }
}