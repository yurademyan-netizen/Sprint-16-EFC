using EFC.Data;
using EFC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;

        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new OrderIndexData();

            viewModel.Orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            if (id.HasValue)
            {
                ViewData["OrderID"] = id.Value;

                viewModel.OrderDetails = await _context.OrderDetails
                    .Where(x => x.OrderId == id)
                    .Include(x => x.Product)
                    .ToListAsync();
            }

            return View(viewModel);
        }

        // GET: Orders/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "LastName");
            ViewData["SuperMarketId"] = new SelectList(_context.Supermarkets, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "LastName", model.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.Supermarkets, "Id", "Name", model.SuperMarketId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.Supermarkets, "Id", "Name", order.SuperMarketId);

            // Завантажуємо продукти для випадаючого списку "Додати товар"
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(e => e.Id == order.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.Supermarkets, "Id", "Name", order.SuperMarketId);
            return View(order);
        }

        // --- НОВІ МЕТОДИ ДЛЯ РОБОТИ З ТОВАРАМИ ---

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(int OrderId, int ProductId, double Quantity)
        {
            // Створюємо новий запис деталізації
            var detail = new OrderDetail
            {
                OrderId = OrderId,
                ProductId = ProductId,
                Quantity = Quantity
            };

            // Важливо: ModelState може бути невалідним через навігаційні властивості (Order, Product)
            // Тому ми просто додаємо об'єкт, якщо ID коректні
            if (OrderId > 0 && ProductId > 0 && Quantity > 0)
            {
                _context.OrderDetails.Add(detail);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = OrderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detail = await _context.OrderDetails.FindAsync(id);
            int orderId = 0;
            if (detail != null)
            {
                orderId = detail.OrderId;
                _context.OrderDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }

        // --- ВИДАЛЕННЯ ЗАМОВЛЕННЯ ---

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}