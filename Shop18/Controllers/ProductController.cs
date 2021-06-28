﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop18.Data;
using Shop18.Models;
using Shop18.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop18.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvirinment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvirinment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _db.Product.Include(u => u.Category);
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvirinment.WebRootPath;
                string upload = webRootPath + ENV.ImagePath;

                string fileName = Guid.NewGuid().ToString();
                string extensions = Path.GetExtension(files[0].FileName);

                if(productVM.Product.Id == 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extensions), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extensions;
                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    var formObject = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    if(files.Count > 0)
                    {
                        var oldFile = Path.Combine(upload, formObject.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extensions), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extensions;
                    }
                    else
                    {
                        productVM.Product.Image = formObject.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }); ;
            return View(productVM);
        }
    }
}
