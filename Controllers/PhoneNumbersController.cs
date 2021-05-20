using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class PhoneNumbersController : Controller
    {
        private readonly PBContext _context;

        public PhoneNumbersController(PBContext context)
        {
            _context = context;
        }

        /*
        // GET: PhoneNumbers
        public async Task<IActionResult> Index()
        {
            var pBContext = _context.PhoneNumbers.Include(p => p.Person);
            return View(await pBContext.ToListAsync());
        }
        */
        // GET: PhoneNumbers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneNumber = await _context.PhoneNumbers
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phoneNumber == null)
            {
                return NotFound();
            }

            return View(phoneNumber);
        }

        /*
        // GET: PhoneNumbers/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name");
            return View();
        }

        // POST: PhoneNumbers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number")] PhoneNumber phoneNumber, int? Id)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phoneNumber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name", phoneNumber.PersonId);
            return View(phoneNumber);
        }
        */

        //Добавление номера телефона.
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                return View(new PhoneNumber { PersonId = id.Value });
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number", "PersonId")] PhoneNumber phoneNumber)
        {
            if (ModelState.IsValid)
            {
                _context.PhoneNumbers.Add(phoneNumber);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "People", new { id = phoneNumber.PersonId });
            }

            return View(phoneNumber);
        }

        // GET: PhoneNumbers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneNumber = await _context.PhoneNumbers.FindAsync(id);
            if (phoneNumber == null)
            {
                return NotFound();
            }
            //ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name", phoneNumber.PersonId);
            return View(phoneNumber);
        }
        /*
        // POST: PhoneNumbers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Number")] PhoneNumber phoneNumber)
        {
            if (id != phoneNumber.Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phoneNumber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhoneNumberExists(phoneNumber.Number))
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
            ViewData["PersonId"] = new SelectList(_context.People, "Number", phoneNumber.Number);
            return View(phoneNumber);
        }
        */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var numberToUpdate = await _context.PhoneNumbers.FirstOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<PhoneNumber>(
                numberToUpdate,
                "",
                s => s.Number))

            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "People", new { id = numberToUpdate.PersonId });
            }
            return View(numberToUpdate);
        }

        // GET: PhoneNumbers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneNumber = await _context.PhoneNumbers
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phoneNumber == null)
            {
                return NotFound();
            }

            return View(phoneNumber);
        }

        // POST: PhoneNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phoneNumber = await _context.PhoneNumbers.FindAsync(id);
            _context.PhoneNumbers.Remove(phoneNumber);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "People", new { id = phoneNumber.PersonId });
        }

        private bool PhoneNumberExists(int id)
        {
            return _context.PhoneNumbers.Any(e => e.Id == id);
        }
    }
}
