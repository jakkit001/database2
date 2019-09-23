using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database2.Data;
using database2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace database2.Controllers
{
    public class bookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public bookController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.books.ToListAsync();
            return View(model);
        }

        [Authorize]
        public IActionResult addbook()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addbook(book model)
        {
            if (ModelState.IsValid)
            {
                var oldbook = await _context.books.AnyAsync(b => b.name == model.name);
                if (oldbook)
                {
                    return View();

                }
                else
                {
                    _context.books.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("index");
                }

            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Bookedit(int id)
        {
                var book = await _context.books.FirstOrDefaultAsync(prayuth => prayuth.id == id);
                if (book == null)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    return View(book);
                }
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> editbook(book model)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.books.FirstOrDefaultAsync(prayuth => prayuth.id == model.id);
                if (book != null)
                {
                    book.name = model.name;
                    book.price = model.price;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("index");
            }
            return View(model);
        }

        public async Task<IActionResult> Bookdelete(int id)
        {
            var book = await _context.books.FirstOrDefaultAsync(prayuth => prayuth.id == id);
            if (book != null)
            {
                _context.books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index");
            }
        }
    }