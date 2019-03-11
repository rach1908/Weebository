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
            var derp = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var derpette = (from merch in context.Merchandise
                           join transaction in context.Transaction on merch.ID equals transaction.Merchandise.ID
                           join user in context.User on transaction.User.Id equals user.Id
                           where user.Id == derp
                           select merch).ToList();

            return View(derpette);
        }

        public IActionResult Friends()
        {
            return View();
        }
    }
}