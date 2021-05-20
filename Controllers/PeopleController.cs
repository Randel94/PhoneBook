using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PBContext _context;

        public PeopleController(PBContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SNameSortParm"] = sortOrder == "SName" ? "sname_desc" : "SName";
            ViewData["CurrentFilter"] = searchString;

            var people = from p in _context.People.Include(x => x.PhoneNumbers)
                         select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                people = people.Where(s => s.Name.Contains(searchString)
                                       || s.SName.Contains(searchString)
                                       || s.BDate.ToString().Contains(searchString)
                                       || s.PhoneNumbers.Contains(new PhoneNumber { Number = searchString}));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    people = people.OrderByDescending(s => s.Name);
                    break;
                case "SName":
                    people = people.OrderBy(s => s.SName);
                    break;
                case "sname_desc":
                    people = people.OrderByDescending(s => s.SName);
                    break;
                default:
                    people = people.OrderBy(s => s.Name);
                    break;
            }

            var person = await people
                .AsNoTracking()
                .ToListAsync();

            return View(person);
        }


        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(x => x.PhoneNumbers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SName,BDate")] Person person, [Bind("Number")] PhoneNumber phoneNumber)
        {
            if (ModelState.IsValid)
            {
                person.PhoneNumbers = new List<PhoneNumber>();
                person.PhoneNumbers.Add(phoneNumber);
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personToUpdate = await _context.People.FirstOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<Person>(
                personToUpdate,
                "",
                s => s.Name, s => s.SName, s => s.BDate))

            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personToUpdate);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(x => x.PhoneNumbers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Не удалось удалить, " +
                    "попробуйте снова.";
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
