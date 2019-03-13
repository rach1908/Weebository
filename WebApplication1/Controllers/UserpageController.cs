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
        public List<Merchandise> Merchandises { get; private set; }
        private readonly ApplicationDbContext context;
        public UserpageController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
                     
            return View();
        }

        public IActionResult Merchandise()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userMerchandiseList = (from merch in context.Merchandise
                           join transaction in context.Transaction on merch.ID equals transaction.Merchandise.ID
                           join user in context.User on transaction.User.Id equals user.Id
                           where user.Id == userID
                           select merch).ToList();

            return View(userMerchandiseList);
        }

        public IActionResult MerchandiseAdd()
        {
            var merchandiseList = context.Merchandise.ToList();

            return View(merchandiseList);
        }

        public IActionResult MerchandiseAddEntry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MerchandiseAddEntry([Bind("Price,Amount,ID,User,Merchandise")]Transaction transaction)
        {
            //find brugeren i databasen
            //sætter du brugeren ind i transaction : transation.user = user;
            
            if (ModelState.IsValid)
            {                
                context.Add(transaction);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        public IActionResult Friends()
        {
            return View();
        }
    }
}