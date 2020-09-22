using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COSEC.Data;
using COSEC.Models;
using COSEC.Repositories;

namespace COSEC.Controllers
{
    public class UsersController : Controller
    {
        private readonly CosecContext _context;

        public UsersController(CosecContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserIdParm"] = String.IsNullOrEmpty(sortOrder) ? "user_id_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = from s in _context.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.USERID.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "user_id_desc":
                    users = users.OrderByDescending(s => s.USERID);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.EVENTDATETIME);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.EVENTDATETIME);
                    break;
                default:
                    users = users.OrderBy(s => s.USERID);
                    break;
            }

            int pageSize = 4;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,USERID,EVENTDATETIME")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        public async Task<IActionResult> Approve(int id)
        {
            DBAccess db = new DBAccess();
            var user = await _context.Users.FindAsync(id);
            db.ApproveUser(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            DBAccess db = new DBAccess();
            var user = await _context.Users.FindAsync(id);
            db.RejectUser(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
