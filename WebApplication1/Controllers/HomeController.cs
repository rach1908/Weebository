﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Animerch.Models;
using Animerch.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace Animerch.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var TopUsers = context.User.Include(x => x.Transactions).Select(cl => new {
                cl.UserName,
                AmountSpent = cl.Transactions.Sum(c => c.Price)
            }
            ).OrderByDescending(x => x.AmountSpent).Take(3);

            var TopMerch = context.Merchandise.Include(x => x.Transactions).Select(cl => new
            {
                cl.Name,
                AmountSpent = cl.Transactions.Sum(c => c.Price)
            });

            ViewData["TopMerch"] = TopMerch;
            ViewData["TopUsers"] = TopUsers;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
