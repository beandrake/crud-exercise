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
                if (ModelState.IsValid)
                {
                    if (PassengerIsDuplicate(passenger))
                    {
                        // fail message
                        ModelState.AddModelError("",
                            "A Passenger with this exact data already exists. " +
                            "Passenger not created.");

                        // return to previous view
                        return View(passenger);
                    }

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
                if (PassengerIsDuplicate(passenger))
                {
                    // fail message
                    ModelState.AddModelError("",
                        "Either this Passenger's data was not changed or " +
                        "a Passenger with this exact data already exists. " +
                        "Passenger not updated.");

                    // return to previous view
                    return View(passenger);
                }

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
         * Development note:
         * Figuring out how to inject some extra validation into the process
         * was a bit challenging.  I started by trying to locate a point in the
         * code where I would be able to access the user's input before it was
         * entered in the database.  It made sense that it would be in a
         * controller, and fortunately it was.
         * 
         * Then I had to figure out where in the process I should put my checks.
         * At first I wanted to add them into ModelState.IsValid, but that proved
         * to be too opaque to easily work with.  In retrospect, I think it would
         * have been a poor choice, because my current understanding is that
         * ModelState.IsValid is really just about verifying that the model
         * conforms to the table's schema.
         * 
         * I eventually decided it made the most sense to put my checks after
         * the ModelState.IsValid check.  By doing this, I get the benefit of
         * knowing these checks have already passed once I do my checks.
         * Additionally, and perhaps more importantly, since the main additional
         * check I implemented was ensuring that each Passenger was unique
         * (not taking into account ID's), it seemed better to have the
         * more trivial ModelState.IsValid check before my order(N) search
         * through all existing Passengers.  That way if ModelState.IsValid is
         * false, we can avoid doing the more expensive check.
         * 
         * Initially I forgot to incorporate the possibility of PhoneNumber
         * being null into my checks.  Fortunately I thought to test this
         * shortly after the initial implementation, so I caught and corrected
         * this crash.
         * 
         * The most difficult bug to solve was when I discovered that while editing a
         * Passenger, adding extra spaces at the end of a Phone Number or name field
         * resulted in an InvalidOperationException.
         * 
         * Still working on it!
         * 
         */




        /*
         * Checks to see if a Passenger object has the same non-ID
         * values as an existing Passenger in the database.
         * Returns true if it finds a match.
         */
        private bool PassengerIsDuplicate(Passenger newPassenger)
        {            
            // check all passengers for a match
            foreach (Passenger currentPassenger in _context.Passengers)
            {
                // if they have the same name...
                if (newPassenger.FirstName.Equals(currentPassenger.FirstName) &&
                    newPassenger.LastName.Equals(currentPassenger.LastName))
                {
                    if (newPassenger.PhoneNumber == null)
                    {
                        // ...and both phone numbers are null, we have a match
                        if (currentPassenger.PhoneNumber == null)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // ... and both non-null phone numbers are the same, we have a match
                        if (currentPassenger.PhoneNumber != null &&
                            newPassenger.PhoneNumber.Equals(currentPassenger.PhoneNumber))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
