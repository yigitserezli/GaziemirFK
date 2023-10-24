using Gfk.Mvc.Helpers;
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
        public IActionResult Landing()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var player = Db.Players.ToList();
            return View(player);
        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var players = await Db.Players.ToListAsync();
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] PlayerEntity player)
		{
            var playersList = await Db.Players.ToListAsync();
            ViewBag.Players = playersList;

            string ageCategory = _ageCategoryHelper.DetermineAgeCategory(player.BornDate);

            player.Category = ageCategory;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen  gerekli alanların hepsinin doldurulduğundan emin olun!";
                return View(player);
            }

            Db.Players.Add(player);
            await Db.SaveChangesAsync();

            ViewBag.SuccessMessage = "Oyuncu başarılı şekilde kaydedilmiştir.";

            return RedirectToAction("Index");
		}

        [HttpGet]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var playerDetail = await Db.Players.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Player = playerDetail;

            if (playerDetail == null)
            {
                ViewBag.ErrorMessage = "Oyuncu bulunamadı.";
                return View();
            }

            return View(playerDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Delete ([FromRoute] int id)
        {
            var player = await Db.Players.FindAsync(id);
            if(player == null)
            {
                return NotFound();
            }

            Db.Players.Remove(player);
            await Db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
	}
}