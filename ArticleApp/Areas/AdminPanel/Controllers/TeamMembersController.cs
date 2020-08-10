using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArticleApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ArticleApp.Controllers
{
    [Area("AdminPanel")]
    public class TeamMembersController : Controller
    {
        private readonly ArticlesContext _context;
        private IHostingEnvironment host;


        public TeamMembersController(ArticlesContext context, IHostingEnvironment hostEnv)
        {
            host = hostEnv;
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamMembers.ToListAsync());
        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                uploadphoto(teamMember);
                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  TeamMember teamMember)
        {
            if (id != teamMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uploadphoto(teamMember);
                    _context.Update(teamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMemberExists(teamMember.Id))
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
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }
        void uploadphoto(TeamMember teamMember)
        {
            if (teamMember.File != null)
            {
                string uploadsFolder = Path.Combine(host.WebRootPath, "Images/TeamMembers");
                string uniqueFileName = Guid.NewGuid() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    teamMember.File.CopyTo(fileStream);
                }
                teamMember.Image = uniqueFileName;
            }
        }
    }
}
