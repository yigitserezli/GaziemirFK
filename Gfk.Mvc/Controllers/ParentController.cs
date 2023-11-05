using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models.Entities;
using Gfk.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gfk.Mvc.Controllers
{
    [Authorize]
    public class ParentController : Controller
    {
        public AppDbContext _dbContext { get; }

        private readonly AgeCategoryHelper _ageCategoryHelper;

        public ParentController(AppDbContext db,
            AgeCategoryHelper ageCategoryHelper)
        {
            _dbContext = db;
            _ageCategoryHelper = ageCategoryHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateParent()
        {
            var players = await _dbContext.Players.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateParent([FromForm] ParentEntity parent, [FromRoute] int id)
        {

            var player = await _dbContext.Players.FindAsync(id);


            if(player is null)
            {
                ViewBag.ErrorMessage = "İlgili oyuncu bulunamadı.";
                return View(parent);
            }

            parent.PlayerId = player.Id;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Girdiniz bilgiler hatalıdır.";
                return View(parent);
            }

            _dbContext.Parents.Add(parent);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Player");
        }

        [HttpGet]
        public IActionResult UpdateParent()
        {
            // var player = await _dbContext.Players.FindAsync(id); UPDATE İÇİN GEÇERLİ
            return View();
        }

        [HttpPost]
        public IActionResult UpdateParent([FromForm] int id)
        {
            // var player = await _dbContext.Players.FindAsync(id); UPDATE İÇİN GEÇERLİ
            return View(id);
        }
    }
}

