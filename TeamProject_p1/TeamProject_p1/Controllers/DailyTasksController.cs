using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamProject_p1.Data;
using TeamProject_p1.Models;
using TeamProject_p1.Data.EFCore;

namespace TeamProject_p1.Controllers
{
  public class DailyTasksController : DBController<DailyTask, EFCoreDailyTaskRepository>
  {
    IRepository<DailyTask> dailyRepo;
    public DailyTasksController(EFCoreDailyTaskRepository repository) : base(repository)
    {
      dailyRepo = repository;
    }

    // GET: DailyTasks
    public async Task<IActionResult> Index()
    {
      return View(await dailyRepo.GetAll());
    }

    // // GET: DailyTasks/Details/5
    // public async Task<IActionResult> Details(int? id)
    // {
    //   if (id == null)
    //   {
    //     return NotFound();
    //   }

    //   var dailyTask = await _context.DailyTasks
    //       .FirstOrDefaultAsync(m => m.DailyTaskId == id);
    //   if (dailyTask == null)
    //   {
    //     return NotFound();
    //   }

    //   return View(dailyTask);
    // }

    // // GET: DailyTasks/Create
    // public IActionResult Create()
    // {
    //   return View();
    // }

    // // POST: DailyTasks/Create
    // // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create([Bind("DailyTaskId,Description")] DailyTask dailyTask)
    // {
    //   if (ModelState.IsValid)
    //   {
    //     _context.Add(dailyTask);
    //     await _context.SaveChangesAsync();
    //     return RedirectToAction(nameof(Index));
    //   }
    //   return View(dailyTask);
    // }

    // // GET: DailyTasks/Edit/5
    // public async Task<IActionResult> Edit(int? id)
    // {
    //   if (id == null)
    //   {
    //     return NotFound();
    //   }

    //   var dailyTask = await _context.DailyTasks.FindAsync(id);
    //   if (dailyTask == null)
    //   {
    //     return NotFound();
    //   }
    //   return View(dailyTask);
    // }

    // // POST: DailyTasks/Edit/5
    // // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(int id, [Bind("DailyTaskId,Description")] DailyTask dailyTask)
    // {
    //   if (id != dailyTask.DailyTaskId)
    //   {
    //     return NotFound();
    //   }

    //   if (ModelState.IsValid)
    //   {
    //     try
    //     {
    //       _context.Update(dailyTask);
    //       await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //       if (!DailyTaskExists(dailyTask.DailyTaskId))
    //       {
    //         return NotFound();
    //       }
    //       else
    //       {
    //         throw;
    //       }
    //     }
    //     return RedirectToAction(nameof(Index));
    //   }
    //   return View(dailyTask);
    // }

    // // GET: DailyTasks/Delete/5
    // public async Task<IActionResult> Delete(int? id)
    // {
    //   if (id == null)
    //   {
    //     return NotFound();
    //   }

    //   var dailyTask = await _context.DailyTasks
    //       .FirstOrDefaultAsync(m => m.DailyTaskId == id);
    //   if (dailyTask == null)
    //   {
    //     return NotFound();
    //   }

    //   return View(dailyTask);
    // }

    // // POST: DailyTasks/Delete/5
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //   var dailyTask = await _context.DailyTasks.FindAsync(id);
    //   _context.DailyTasks.Remove(dailyTask);
    //   await _context.SaveChangesAsync();
    //   return RedirectToAction(nameof(Index));
    // }

    // private bool DailyTaskExists(int id)
    // {
    //   return _context.DailyTasks.Any(e => e.DailyTaskId == id);
    // }
  }
}
