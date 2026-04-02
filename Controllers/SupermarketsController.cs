using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFC.Data;
using EFC.Models;

namespace EFC.Controllers
{
    public class SupermarketsController : Controller
    {
        private readonly ShoppingContext _context;

        public SupermarketsController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Supermarkets
        public async Task<IActionResult> Index(int? pageNum)
        {
            int pageSize = 3;
            var supermarkets = _context.Supermarkets.AsNoTracking();

            return View(await PaginatedList<Supermarket>
                .CreateAsync(supermarkets, pageNum ?? 1, pageSize));
        }

        // GET: Supermarkets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _context.Supermarkets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supermarket == null)
            {
                return NotFound();
            }

            return View(supermarket);
        }

        // GET: Supermarkets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supermarkets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] Supermarket supermarket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supermarket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supermarket);
        }

        // GET: Supermarkets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _context.Supermarkets.FindAsync(id);
            if (supermarket == null)
            {
                return NotFound();
            }
            return View(supermarket);
        }

        // POST: Supermarkets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Supermarket supermarket)
        {
            if (id != supermarket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supermarket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupermarketExists(supermarket.Id))
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
            return View(supermarket);
        }

        // GET: Supermarkets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _context.Supermarkets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supermarket == null)
            {
                return NotFound();
            }

            return View(supermarket);
        }

        // POST: Supermarkets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supermarket = await _context.Supermarkets.FindAsync(id);
            if (supermarket != null)
            {
                _context.Supermarkets.Remove(supermarket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupermarketExists(int id)
        {
            return _context.Supermarkets.Any(e => e.Id == id);
        }
    }
}
