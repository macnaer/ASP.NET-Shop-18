using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shop18.Data;
using Shop18.Models;
using Shop18.Models.ViewModels;
using Shop18.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shop18.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext  _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(u => u.Category),
                Categories = _db.Category
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(ENV.SessionCart);
            }

            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category)
               .Where(u => u.Id == id).FirstOrDefault(),
                InCart = false
            };

            foreach(var item in shoppingCartList)
            {
                if(item.ProductId == id)
                {
                    DetailsVM.InCart = true;
                }
            }

            return View(DetailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(ENV.SessionCart);
            }

            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(ENV.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(ENV.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(ENV.SessionCart);
            }

            var removeItem = shoppingCartList.SingleOrDefault(u => u.ProductId == id);
            if(removeItem != null)
            {
                shoppingCartList.Remove(removeItem);
            }

            HttpContext.Session.Set(ENV.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
