using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArticleApp.Models;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ArticleApp.Controllers
{
    [Area("AdminPanel")]
    public class ArticlesController : Controller
    {
        private readonly ArticlesContext _context;
        private IHostingEnvironment host;
        public ArticlesController(ArticlesContext context, IHostingEnvironment hostEnv )
        {
            _context = context;
            host = hostEnv;
        }

       

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var articlesContext = _context.Articles.Include(a => a.Category);
            return View(await articlesContext.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewData["CaregoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Article article)
        {
            if (ModelState.IsValid)
            {
                uploadphoto(article);
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaregoryId"] = new SelectList(_context.Categories, "Id", "Name", article.CaregoryId);
            return View(article);
        }

        

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CaregoryId"] = new SelectList(_context.Categories, "Id", "Name", article.CaregoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uploadphoto(article);
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            ViewData["CaregoryId"] = new SelectList(_context.Categories, "Id", "Id", article.CaregoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
         void uploadphoto(Article article)
        {
            if (article.File != null)
            {
                string uploadsFolder = Path.Combine(host.WebRootPath, "Images/Articles");
                string uniqueFileName = Guid.NewGuid() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath,FileMode.Create))
                {
                    article.File.CopyTo(fileStream);
                }
                article.Image=uniqueFileName;
            }
        }

    }
}
