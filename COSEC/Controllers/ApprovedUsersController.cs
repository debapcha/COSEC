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
    public class ApprovedUsersController : Controller
    {
        private readonly CosecContext _context;

        public ApprovedUsersController(CosecContext context)
        {
            _context = context;
        }

        // GET: ApprovedUsers
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

            var users = from s in _context.ApprovedUsers
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

            int pageSize = 5;
            return View(await PaginatedList<ApprovedUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await _context.ApprovedUsers.ToListAsync());
        }

        // GET: ApprovedUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvedUser = await _context.ApprovedUsers.FindAsync(id);
            if (approvedUser == null)
            {
                return NotFound();
            }
            return View(approvedUser);
        }

        // POST: ApprovedUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,USERID,EVENTDATETIME")] ApprovedUser approvedUser)
        {
            if (id != approvedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvedUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovedUserExists(approvedUser.Id))
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
            return View(approvedUser);
        }


        public async Task<IActionResult> Approve(int id)
        {
            DBAccess db = new DBAccess();
            var user = await _context.ApprovedUsers.FindAsync(id);
            db.ApproveFinalUser(id);

            _context.ApprovedUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            DBAccess db = new DBAccess();
            var user = await _context.ApprovedUsers.FindAsync(id);
            db.RejectFinalUser(id);

            _context.ApprovedUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovedUserExists(int id)
        {
            return _context.ApprovedUsers.Any(e => e.Id == id);
        }
    }
}
