using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PassengerManager.Data;
using PassengerManager.Models;

namespace PassengerManager.Controllers
{
    public class PassengersController : Controller
    {
        private readonly OverallContext _context;

        public PassengersController(OverallContext context)
        {
            _context = context;
        }

        // GET: Passengers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Passengers.ToListAsync());
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .SingleOrDefaultAsync(m => m.ID == id);

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Passengers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
         * Based on testing, the Bind below seems to be restricting which column data is sent.
         * For example, try replacing with Bind("ID")
         * explanation: https://www.codeproject.com/tips/1032266/mvc-attributes
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,PhoneNumber")] Passenger passenger)
        {
            try
            {
                if (PassengerIsDuplicate(passenger))
                {
                    // fail message

                    // return to previous view
                    return View(passenger);
                }


                if (ModelState.IsValid)
                {
                    _context.Add(passenger);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(passenger);
        }

        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.SingleOrDefaultAsync(m => m.ID == id);

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,PhoneNumber")] Passenger passenger)
        {
            if (id != passenger.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassengerExists(passenger.ID))
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
            return View(passenger);
        }

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .SingleOrDefaultAsync(m => m.ID == id);

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.ID == id);
        }


        /*
         * Checks to see if a Passenger object has the same non-ID
         * values as an existing Passenger in the database.
         * Returns true if it finds a match.
         */
        private bool PassengerIsDuplicate(Passenger newPassenger)
        {
            foreach (Passenger currentPassenger in _context.Passengers)
            {
                if (newPassenger.FirstName.Equals(currentPassenger.FirstName) &&
                    newPassenger.LastName.Equals(currentPassenger.LastName) &&
                    newPassenger.PhoneNumber.Equals(currentPassenger.PhoneNumber))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
