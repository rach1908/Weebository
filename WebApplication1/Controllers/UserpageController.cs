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

            //var userMerchandiseList = (from merch in context.Merchandise
            //               join transaction in context.Transaction on merch.ID equals transaction.Merchandise.ID
            //               join user in context.User on transaction.User.Id equals user.Id
            //               where user.Id == userID
            //               select merch).ToList();


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

        


        public IActionResult Friends()
        {
            return View();
        }
    }

}