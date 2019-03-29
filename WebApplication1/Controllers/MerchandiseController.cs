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
    public class MerchandiseController : Controller
    {
        public List<Merchandise> Merchandises { get; private set; }
        private readonly ApplicationDbContext context;
        public MerchandiseController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var merchandiseList = context.Merchandise.ToList();

            return View(merchandiseList);
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
        public async Task<IActionResult> Create([Bind("Price, Amount")] Transaction transaction)
        {
            
            return null;
            
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
    }
}
