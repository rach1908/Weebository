using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animerch.Data;
using Animerch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Animerch.Controllers
{    
    public class UserpageController : Controller
    {
        private SignInManager<User> signInManager;

        private readonly ApplicationDbContext context;

        public UserpageController(ApplicationDbContext context, SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            if (signInManager.IsSignedIn(User))
            {              
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        public IActionResult UserMerchandise()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactionList = context.Transaction.Where(x => x.User.Id.ToString() == userID).Include("Merchandise");

            return View(transactionList);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merch = context.Merchandise.Find(id);
            if (merch == null)
            {
                return NotFound();
            }

            var merchItem = context.Merchandise.Where(x => x.ID == id).FirstOrDefault();

            ViewData.Add("selectedMerch", merchItem);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Price, Amount, MerchandiseId")] Transaction transaction)
        {
            transaction.UserId = (await signInManager.UserManager.GetUserAsync(signInManager.Context.User)).Id;


            if (ModelState.IsValid)
            {
                context.Add(transaction);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(UserMerchandise));
            }
            return RedirectToAction("Create/" + transaction.MerchandiseId);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            context.Transaction.Where(x => x.ID == id).Include("Merchandise").FirstOrDefault();

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await context.Transaction.SingleOrDefaultAsync(t => t.ID == id);

            if (await TryUpdateModelAsync(transaction, "", t => t.Price, t => t.Amount))
            {
                try
                {
                    context.Update(transaction);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Merchandise");
            }
            return RedirectToAction("Edit/" + id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await context.Transaction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            context.Transaction.Where(x => x.ID == id).Include("Merchandise").FirstOrDefault();


            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await context.Transaction.FindAsync(id);
            context.Transaction.Remove(transaction);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(UserMerchandise));
        }

        public IActionResult Friends()
        {
            return View();
        }

        private bool TransactionExists(int id)
        {
            return context.Transaction.Any(e => e.ID == id);
        }
    }
}