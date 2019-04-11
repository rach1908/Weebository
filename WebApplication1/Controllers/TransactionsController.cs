using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Animerch.Data;
using Animerch.Models;
using Microsoft.AspNetCore.Identity;

namespace Animerch.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Transaction.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.ID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Price,Amount,MerchandiseId")] Transaction transaction)
        {
            var user = await _signInManager.UserManager.GetUserAsync(_signInManager.Context.User);

            transaction.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["Error"] = "An error occured. Are you missing a required field?";
                return View(transaction);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.SingleOrDefaultAsync(t => t.ID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View();
        }
    }
}
