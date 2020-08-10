using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArticleApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ArticleApp.Controllers
{
    public class HomeController : Controller
    {
        ArticlesContext db;
        public HomeController(ArticlesContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Messages()
        {
            return View(db.Contacts.ToList());
        }
        public IActionResult TeamMembers()
        {
            return View(db.TeamMembers.ToList());
        }
        [Authorize]
        public IActionResult Articles(int Id)
        {
            var cate = db.Categories.Find(Id);
            ViewBag.categ = cate.Name;
            return View(db.Articles.Where(x => x.CaregoryId == Id).OrderByDescending(x=>x.Date).ToList());
        }
        public IActionResult DeleteArticle(int Id)
        {
            db.Articles.Remove(db.Articles.Find(Id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost ]
        public IActionResult SaveContact(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
            db.Contacts.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View("Contact", contact);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
