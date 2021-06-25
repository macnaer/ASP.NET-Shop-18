using Microsoft.AspNetCore.Mvc;
using Shop18.Data;
using Shop18.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop18.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _db.Category;
            return View(categoryList);
        }


        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = _db.Category.Find(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = _db.Category.Find(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Category.Find(id);

            if(category == null)
            {
                return NotFound();
            }
            else
            {
                _db.Category.Remove(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
