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
        private readonly SignInManager<User> signInManager;

        private readonly UserManager<User> userManager;

        private readonly ApplicationDbContext context;

        public UserpageController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;            
        }

        private bool TransactionExists(int id)
        {
            return context.Transaction.Any(e => e.ID == id);
        }

        private async Task<User> GetUser()
        {         
            return await userManager.GetUserAsync(User);
        }

        private async Task<List<User>> GetFriends(string userId)
        {
            var friends = new List<User>();

            friends.AddRange(await context.FriendEntry.Where(FE => FE.FriendID == userId).Include(U => U.User).Select(U => U.User).ToListAsync());

            return friends;
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

        public async Task<IActionResult> Users()
        {
            User user = await GetUser();
            user.Friends = await GetFriends(user.Id);
            var users = await context.User.ToListAsync();
            ViewData.Add("Users", users);

            return View();
        }  

        public async Task<IActionResult> Friends()
        {
            if (User == null)
            {
                return NotFound();
            }
            User user = await GetUser();
            user.Friends = await GetFriends(user.Id);
            var friendEntries = await context.FriendEntry.Where(FE => FE.UserID == user.Id && FE.FriendID == user.Id).ToListAsync();
            ViewData.Add("UserFriendListEntries", friendEntries);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserAddFriend([Bind("UserID,FriendID")]FriendEntry friendEntry)
        {
            if (ModelState.IsValid)
            {
                context.FriendEntry.Add(friendEntry);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost, ActionName("UserDeleteFriend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDeleteFriend(string friendId, string userId)
        {
            var FeToDelete = await context.FriendEntry.FindAsync(userId, friendId);
            context.FriendEntry.Remove(FeToDelete);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Friends));
        }

        [HttpPost, ActionName("UserAcceptFriend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserAcceptFriend(string userId, string friendId)
        {
            var friendEntryToUpdate = await context.FriendEntry.FirstAsync(FE => FE.UserID == userId && FE.FriendID == friendId);
            context.FriendEntry.Update(friendEntryToUpdate);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Friends));
        }
    }
}