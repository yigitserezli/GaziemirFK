using System;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        public AppDbContext _dbContext { get; }

        private readonly AgeCategoryHelper _ageCategoryHelper;

        public PlayerController(AppDbContext db,
            AgeCategoryHelper ageCategoryHelper)
        {
            _dbContext = db;
            _ageCategoryHelper = ageCategoryHelper;
        }

        [HttpGet]
        public IActionResult List()
        {
            var player = _dbContext.Players.ToList();
            return View(player);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePlayer()
        {
            var players = await _dbContext.Players.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromForm] PlayerEntity player)
        {
            var playersList = await _dbContext.Players.ToListAsync();
            ViewBag.Player = playersList;

            string ageCategory = _ageCategoryHelper.DetermineAgeCategory(player.BornDate);
            player.Category = ageCategory;

            player.UptadetAt = DateTime.UtcNow;


            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen gerekli alanların tamamının doldurulduğundan emin olun!";
                return View(player);
            }
           
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction("CreateParent", "Parent", new { id = player.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var playerDetail = await _dbContext.Players.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Player = playerDetail;

            if (playerDetail == null)
            {
                ViewBag.ErrorMessage = "Oyuncu bulunamadı.";
                return View();
            }

            return View(playerDetail);
        }

        [HttpGet]
        public async Task<JsonResult> DetailJson([FromRoute] int id)
        {
            var playerDetail = await _dbContext.Players.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Player = playerDetail;

            if (playerDetail is not null)
            {
                return Json(new
                {
                    name = playerDetail.Name,
                    surname = playerDetail.Surname,
                    phone = playerDetail.Phone,
                    email = playerDetail.Email,
                    identification = playerDetail.IdentificationNumber,
                    category = playerDetail.Category,
                    bornDate = playerDetail.BornDate,
                    note = playerDetail.Note,
                    licance = playerDetail.Licance,
                    address = playerDetail.Address,
                    foot = playerDetail.Foot,
                    uptadetAt = playerDetail.UptadetAt,
                    mailPermission = playerDetail.MailPermission,
                    smsPermission = playerDetail.SmsPermission
                });
            }

            return Json(playerDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var player = await _dbContext.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _dbContext.Players.Remove(player);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        
    }
}

