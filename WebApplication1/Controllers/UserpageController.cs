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
        public SignInManager<User> signInManager;
        public UserManager<User> userManager;
        public List<Merchandise> Merchandises { get; private set; }
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

        // GET: Userpage/Edit/5
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


            return View(transactionItem);
        }

        // POST: Userpage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Price,Amount,MerchandiseId")] Transaction transaction)
        {
            if (id != transaction.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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