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
         * For example, try replacing with Bind("ID").
         * Explanation: https://www.codeproject.com/tips/1032266/mvc-attributes
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,PhoneNumber")] Passenger passenger)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    /*
                     * Note: currently commented out for the sake of consistency.
                     * Only uncomment when this behavior can be duplicated in Edit.
                    if (PassengerIsDuplicate(passenger))
                    {
                        // fail message
                        ModelState.AddModelError("",
                            "A Passenger with this exact data already exists. " +
                            "Passenger not created.");

                        // return to previous view
                        return View(passenger);
                    }
                    */

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
        // Method adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud
        /* This is the tutorial's recommended Edit method
         * Unfortunately, it seems to do everything on its own.
         */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* 
             * Aftering digging through multiple stackoverflow questions and
             * then taking another look at the SingleOrDefaultAsync method's
             * hover description, I believe I understand this line.
             * It's basically just returning the Passenger within the
             * current context that meets the requirement: id == Passenger.id
             * 
             * Did some tests to verify that passengerToUpdate is based on the
             * current model and NOT the user's input.  Changing it doesn't
             * have any lasting effect on the model, either.
             */            
            var passengerToUpdate = await _context.Passengers.SingleOrDefaultAsync(p => p.ID == id);

            /*
             * The line above gets the passenger which needs to be updated.
             * Below, the:    if (await TryUpdateModelAsync...
             * Is where the passenger is actually being updated.
             * This means any checks I want to make need to occur between
             * them.  So here's the place to try. 
             */

            /*
             * I've done a good deal of Googling, but I am having trouble getting
             * access to the input for either checking or manipulation
             * within this method.  For now, I'm commenting out the basic
             * structure of what I'm hoping to put here; maybe I can find
             * a way to get it working later.
             */

            /*
            if (PassengerIsDuplicate(obj.FirstName, obj.LastName, obj.PhoneNumber))
            {
                // fail message
                ModelState.AddModelError("",
                    "Either another passenger with this exact data already exists " +
                    "or this passenger has not been changed." +
                    "Passenger not created.");

                // return to previous view
                return View(passengerToUpdate);
            }
            */


            /*
             * The below function tries to update the model with information
             * provided via the controller, meaning that this is where
             * the user input is coming into play.
             * Unfortunately, it seems like it's all happening in an
             * opaque black box that I don't believe I can interact with.
             * Returns true if update worked.
             */
            if (await TryUpdateModelAsync<Passenger>(
                passengerToUpdate,
                "",
                p => p.FirstName, p => p.LastName, p => p.PhoneNumber))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(passengerToUpdate);
        }



        // POST: Passengers/Edit/5
        // Method adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud
        /*
         * The below is based on the secondary implementation of Edit from the above tutorial.
         * Unedited, it works.
         * But I added a check in (PassengerIsDuplicate).  It simply looks through
         * the current model's list of passengers, and based on what it finds
         * it returns true or false.  No editing of any data!
         * With this check added in, editing a passenger with a unique result
         * will always result in the following error:
         * 
         * InvalidOperationException: The instance of entity type 'Passenger' cannot
         * be tracked because another instance with the same key value for {'ID'} is
         * already being tracked. When attaching existing entities, ensure that only
         * one entity instance with a given key value is attached. Consider using 
         * 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting 
         * key values.
         */
        /*
         * Commenting this out for now and using the recommend EditPost method above.
        [HttpPost, ActionName("Edit")]
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
                        "Either another passenger with this exact data already exists " +
                        "or this passenger has not been changed." +
                        "Passenger not created.");

                    // return to previous view
                    return View(passenger);
                }

                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(passenger);
        }
        */


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
         * The most difficult bug to solve was when I discovered that for
         * some reason, the checks I made to prevent duplicate passengers
         * from being created result in all non-duplicate Edit operations
         * failing due to an InvalidOperationException.
         * 
         * According to the debug output, the Exception is occuring in the Edit
         * method's _context.Update(passenger);
         * 
         * That's odd, because the point of failure is no within the checks
         * I added.  And it's particularly odd because my code shouldn't be
         * changing any data, just program flow.  It checks some values,
         * then returns true or false depending on what it finds.
         * 
         * The really strange part is that if I just have my PassengerIsDuplicate
         * method return false (thus avoiding all the checks),
         * the bug disappears.
         * 
         * Something neat is happening!
         * 
         * Error output:
         * InvalidOperationException: The instance of entity type 'Passenger' cannot be tracked because another instance with the same key value for {'ID'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
         * 
         * It says an entry has the same key, which makes me think that
         * the Update operation is somehow being interpreted as trying to create
         * a duplicate of the data rather than simply changing the values.
         * Not sure that makes sense though.  Also not sure how to change it if
         * it is the case.
         * 
         * I've also been looking into other solutions.  C#'s validation system
         * is likely a better place to perform these checks,
         * so I've been trying to figure out how to create my own validation.
         * 
         * At present, I have removed all of my custom validation from this
         * application.  My instinct says that it would be poor UX to validate
         * data differently depending on where it is being created or updated;
         * inconsistency is very frustrating for users.  So until it can
         * work for the whole model, it's not allowed to work for any of it.
         * 
         * It seems like the innate validation system is the way to go.  From
         * what I've been reading, proper MVC uses the Controller only for flow
         * control.
         * 
         * 
         * 
         */




        /*
         * Checks to see if a Passenger object has the same non-ID
         * values as an existing Passenger in the database.
         * Returns true if it finds a match.
         */
        private bool PassengerIsDuplicate(Passenger newPassenger)
        {
            return PassengerIsDuplicate(newPassenger.LastName,
                                        newPassenger.FirstName,
                                        newPassenger.PhoneNumber);
        }

        private bool PassengerIsDuplicate(string newLastName, string newFirstName, string newPhoneNumber)
        {            
            // check all passengers for a match
            foreach (Passenger currentPassenger in _context.Passengers)
            {
                // if they have the same name...
                if (newFirstName.Equals(currentPassenger.FirstName) &&
                    newLastName.Equals(currentPassenger.LastName))
                {
                    if (newPhoneNumber == null)
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
                            newPhoneNumber.Equals(currentPassenger.PhoneNumber))
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
