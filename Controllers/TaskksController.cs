using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    public class TaskksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Taskks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Taskk.ToListAsync();
            return View(tasks);
        }

        // POST: Taskks/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Taskk != null ? 
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Taskk'  is null.");
        }

        // POST: Taskks/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return _context.Taskk != null ?
                        View("Index", await _context.Taskk.Where(j => j.TaskkContent.Contains(SearchPhrase)).ToListAsync()):
                        Problem("Entity set 'ApplicationDbContext.Taskk'  is null.");
        }

        // GET: Taskks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Taskk == null)
            {
                return NotFound();
            }

            var taskk = await _context.Taskk
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskk == null)
            {
                return NotFound();
            }

            return View(taskk);
        }

        // GET: Taskks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Taskks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskkName,TaskkContent, Created")] Taskk taskk)
        {
            if (ModelState.IsValid)
            {
                taskk.Created = DateTime.Now;
                _context.Add(taskk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskk);
        }

        // GET: Taskks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Taskk == null)
            {
                return NotFound();
            }

            var taskk = await _context.Taskk.FindAsync(id);
            if (taskk == null)
            {
                return NotFound();
            }
            return View(taskk);
        }

        // POST: Taskks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskkName,TaskkContent")] Taskk editedTaskk)
        {
            if (id != editedTaskk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalTaskk = await _context.Taskk.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (originalTaskk == null)
                    {
                        return NotFound();
                    }

                    // Copy non-editable fields from the original entity
                    editedTaskk.Created = originalTaskk.Created;

                    _context.Update(editedTaskk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskkExists(editedTaskk.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editedTaskk);
        }


        // GET: Taskks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Taskk == null)
            {
                return NotFound();
            }

            var taskk = await _context.Taskk
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskk == null)
            {
                return NotFound();
            }

            return View(taskk);
        }

        // POST: Taskks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Taskk == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Taskk'  is null.");
            }
            var taskk = await _context.Taskk.FindAsync(id);
            if (taskk != null)
            {
                _context.Taskk.Remove(taskk);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskkExists(int id)
        {
          return (_context.Taskk?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
