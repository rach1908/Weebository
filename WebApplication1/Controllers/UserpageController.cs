﻿using System;
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

        private async Task<List<FriendEntry>> GetFriendData(string userId)
        {
            var friendEntries = 
                    await context.FriendEntry
                    .Where(FE => FE.UserID == userId || FE.FriendID == userId)
                    .Include(FE => FE.Friend)
                    .Include(FE => FE.User)
                    .ToListAsync();

            return friendEntries;
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
            var user = await GetUser();
            var users = await context.User.ToListAsync();
            if (user != null)
            {
                var friends = await GetFriendData(user.Id);
                ViewData.Add("UserFriends", friends);
            }
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

            var friendEntries = await GetFriendData(user.Id);
            ViewData.Add("UserFriendListEntries", friendEntries);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserAddFriend([Bind("UserID,FriendID,RequestAccepted")]FriendEntry friendEntry, string requestSource)
        {            
            FriendEntry feToUpdate = await context.FriendEntry.SingleOrDefaultAsync(FE => FE.UserID == friendEntry.FriendID && FE.FriendID == friendEntry.UserID);

            if (ModelState.IsValid)
            {
                context.FriendEntry.Add(friendEntry);
                if (feToUpdate != null)
                {
                    feToUpdate.RequestAccepted = true;
                    context.FriendEntry.Update(feToUpdate);
                }
                await context.SaveChangesAsync();
                return RedirectToPage(requestSource);

            }
            return RedirectToAction(requestSource);                
        }

        [HttpPost, ActionName("UserDeleteFriend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDeleteFriend(string userId, string friendId)
        {
            var feToDelete = await context.FriendEntry.FindAsync(userId, friendId);
            var feToDelete2 = await context.FriendEntry.FindAsync(friendId, userId);

            if (feToDelete2 != null)
            {
                context.FriendEntry.Remove(feToDelete2);
            }
            context.FriendEntry.Remove(feToDelete);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Friends));
        }
    }
}