using System;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models;
using Gfk.Mvc.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> List(string searchString, string category, string licance, string foot, string identificationNumber, bool? kvkk, int page = 1, int pageSize = 12)
        {
            //Begining of the query
            var query = _dbContext.Players.AsQueryable();

            // Searching and filtering conditions
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString)
                                         || p.Surname.Contains(searchString)
                                         || p.Email.Contains(searchString)
                                         || p.IdentificationNumber.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }
            if (!string.IsNullOrEmpty(licance))
            {
                query = query.Where(p => p.Licance == licance);
            }
            if (kvkk.HasValue)
            {
                query = query.Where(p => p.Kvkk == kvkk.Value);
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var items = await query.OrderBy(p => p.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var result = new PlayerPaginationViewModel<PlayerEntity>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                SearchString = searchString,
                Category = category,
                Licance = licance,
                Foot = foot,
                IdentificationNumber = identificationNumber,
                Kvkk = kvkk
            };

            return View(result);
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
                    uptadetAt = playerDetail.UptadetAt,
                    kvkk = playerDetail.Kvkk,
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

