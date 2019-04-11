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

        //public List<Merchandise> Merchandises { get; private set; }

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

        public IActionResult Merchandise()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactionList = context.Transaction.Where(x => x.User.Id.ToString() == userID).Include("Merchandise");

            return View(transactionList);
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

            var transactionItem = context.Transaction.Where(x => x.ID == id).Include("Merchandise").FirstOrDefault();

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public async Task<IActionResult> EditTransaction(int? id)
=======
        public async Task<IActionResult> Edit(int id, [Bind("ID,Price,Amount,MerchandiseId")] Transaction transaction)
>>>>>>> 4fb25d3fab951cedea2fa9adf92cc77ad9baa832
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