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

        private UserManager<User> userManager;

        private readonly ApplicationDbContext context;

        public UserpageController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                return RedirectToAction("Account/Login", "Identity");
            }            
        }

        public IActionResult UserMerchandise()
        {
            var transactionList = context.Transaction.Where(x => x.User.Id == userManager.GetUserId(User)).Include("Merchandise");

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
            transaction.UserId = userManager.GetUserId(User);

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

        public async Task<IActionResult> Friends()
        {
            if (User == null)
            {
                return NotFound();
            }
            User user = await userManager.GetUserAsync(User);
            var friends = new List<User>();
            var friendEntries = await context.FriendEntry.Where(FE => FE.UserID == user.Id).ToListAsync();
            ViewData.Add("UserFriendListEntries", friendEntries);
                
            //context.User.Where(U => U.Id context.FriendEntry.);

            foreach (var friendEntry in friendEntries)
            {
                friends.Add(await context.User.FirstAsync(U => U.Id == friendEntry.FriendID));
            }
            user.Friends = friends;

            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await context.User.ToListAsync();
            
            ViewData.Add("Users", users);

            return View();
        }

        public IActionResult OtherUser(string userName)
        {
            return View();
        }

        private bool TransactionExists(int id)
        {
            return context.Transaction.Any(e => e.ID == id);
        }

        [HttpPost]
        public async Task<IActionResult> UsersAddFriend([Bind("UserID,FriendID")]FriendEntry friendEntry)
        {
            if (ModelState.IsValid)
            {
                context.FriendEntry.Add(friendEntry);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            return RedirectToAction(nameof(Users));
        }
    }
}