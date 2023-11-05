using System.Numerics;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models;
using Gfk.Mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public AppDbContext Db { get; }

        private readonly AgeCategoryHelper _ageCategoryHelper;

        public HomeController(AppDbContext db, AgeCategoryHelper ageCategoryHelper)
        {
            Db = db;
            _ageCategoryHelper = ageCategoryHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Landing()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact([FromForm] ContactFormEntity model)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> WelcomeSession()
        {
            //Register sonrası sistem bizi aktivasyon ekranına yönlendirir. Aktivasyon sonrası karşılama mesajı buradan verilecek.

            return View();
        }







    }
}