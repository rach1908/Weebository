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
using Animerch.Models.DataTransferObjects;


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

            var userMerchandiseList = (from merch in context.Merchandise
                                       join transaction in context.Transaction on merch.ID equals transaction.Merchandise.ID
                                       join user in context.User on transaction.User.Id equals user.Id
                                       where user.Id == userID
                                       select new MerchandiseFull
                                       {
                                           ID = transaction.ID,
                                           Name = merch.Name,
                                           Type = merch.Type,
                                           Series = merch.Series,
                                           Manufacturer = merch.Manufacturer,
                                           Amount = transaction.Amount,
                                           Price = transaction.Price
                                       });

            ViewData.Add("mList", userMerchandiseList);

            return View(userMerchandiseList);
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


            var userMerchandiseFull = (from merch in context.Merchandise
                                       join transactions in context.Transaction on merch.ID equals transaction.Merchandise.ID
                                       where transactions.ID == id
                                       select new MerchandiseFull
                                       {
                                           ID = transaction.ID,
                                           Name = merch.Name,
                                           Type = merch.Type,
                                           Series = merch.Series,
                                           Manufacturer = merch.Manufacturer,
                                           Amount = transaction.Amount,
                                           Price = transaction.Price
                                       });

            ViewData.Add("mFull", userMerchandiseFull);

            return View(userMerchandiseFull as MerchandiseFull);
        }

        // POST: Userpage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Price,Amount")] MerchandiseFull transaction)
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
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
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