using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gfk.Mvc.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gfk.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private readonly AppDbContext _dbContext;

        public PaymentController(AppDbContext context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddPayment()
        {
            var players = await _dbContext.Players.ToListAsync();
            ViewData["Players"] = players;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(PaymentEntity paymentEntity)
        {
            if(ModelState.IsValid)
            {
                var paymentRecord = new PaymentEntity
                {
                    PlayerId = paymentEntity.PlayerId,
                    Year = paymentEntity.Year,
                    Month = paymentEntity.Month,
                    IsPaid = paymentEntity.IsPaid,
                    Note = paymentEntity.Note
                };

                _dbContext.Payments.Add(paymentRecord);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(paymentEntity);
        }

        [HttpGet]
        public async Task<IActionResult> FilterPayments(string category, int? playerId)
        {
            var query = _dbContext.Payments.Include(p => p.Player).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Player.Category == category);
            }

            if (playerId.HasValue)
            {
                query = query.Where(p => p.PlayerId == playerId.Value);
            }

            var payments = await query.ToListAsync();
            ViewData["FilteredPayments"] = payments;

            return View("Index");
        }
    }
}

