// initially adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
// attributes adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace PassengerManager.Models
{
    // this object definition is also defining the associated database schema
    public class Passenger
    {
        // ID is the primary key (because it is named ID)
        // The below line tells the database to generate a unique ID when it adds a Passenger
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(Passenger), "TestString")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(Passenger), "TestString")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        // not requiring this because people without phones deserve cruises, too
        [StringLength(50)]
        [CustomValidation(typeof(Passenger), "TestString")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }


        ////////////////////////////////////////////////////////
        ////
        ////             Validation Tests
        ////
        ////////////////////////////////////////////////////////


        /* 
         * This method does custom checks to validate Passenger's strings.
         * Called by the CustomValidation attribute (see above)
         * 
         * NOTE: for help with Regex syntax: http://regexr.com/
         *       paste the Regex inside and hover over its components to get translations
         */
        public static ValidationResult TestString(string stringToTest, ValidationContext pValidationContext)
        {
            // No reason to test null; if it's a problem, another validation will catch it.
            if (stringToTest == null)
            {
                return ValidationResult.Success;
            }
            
            // don't allow leading or trailing white space
            if (!Regex.IsMatch(stringToTest, @"^\S(.*\S)?$"))
            {
                return new ValidationResult("Cannot begin or end with a white space.", new List<string> { pValidationContext.MemberName });
            }

            // don't allow consecutive white space
            if (Regex.IsMatch(stringToTest, @"^.*\s{2,}.*$"))
            {
                return new ValidationResult("Cannot contain consecutive white space.", new List<string> { pValidationContext.MemberName });
            }

            return ValidationResult.Success;
        }



        /* 
         * Developer Notes:
         * 
         * To ensure a user-friendly experience, there are some additional
         * checks I want to make on data:
         *      - no leading or trailing white space (DONE)
         *      - no multiple white spaces in a row (DONE)
         *      - ensure that every combination of LastName, FirstName,
         *        and PhoneNumber are unique
         * 
         * Initially, I tried to solve this in the controller; when a
         * Passenger was created by the user, I would check the data.
         * If I found something I didn't like, I would bounce back
         * an error message and return before the data could be added
         * to the database.
         * 
         * While this was simple enough with data creation, it was a
         * different story with updating.  The tutorial I used to
         * learn this framework offered two possible templates for the
         * Edit action, and neither of them seemed to work my checks.
         * 
         * I did a lot of research to try to find a way to make these
         * work together, but nothing I tried succeeded.  I looked up
         * documentation on various methods being invoked in the Edit
         * action, and while I was able to understand some, the one
         * doing the heavy lifting (TryUpdateModelAsync) was a black
         * box.  Even stepping through code in debug mode wouldn't
         * allow me to see what was going on inside it.
         * 
         * Eventually I found out that the approach I was taking wasn't
         * the best practice - the controller was intended to be used
         * almost exclusively for flow control, and that the model should
         * be responsible for validating itself.  I started looking into
         * Validation attributes.
         * 
         * For my purposes, there were two that jumped out: RegularExpression
         * and CustomValidationAttributes.  I had never used regular
         * expressions before, but I was able to dig up one that matched
         * strings without leading or trailing spaces.  Using the official
         * documentation, I was able to write a line of code that compiled,
         * and some quick tests verified that the regular expression was
         * working properly. I added in a fitting error message and applied
         * it to all of the user-modifiable variables.
         * 
         * However, I discoverd that you can't use multiple RegularExpression
         * attributes on a single variable, meaning the other solutions would
         * have to come from elsewhere: CustomValidationAttribute.
         * (that said, this is probably for the best anyway; an army of
         * regular expressions copied-and-pasted above every string
         * doesn't scale well)
         * 
         * The documentation I found on this attribute was minimal.  The
         * most accessible example I could find
         * (https://weblogs.asp.net/peterblum/the-customvalidationattribute)
         * worked, but it exposed an interesting quirk in the framework
         * in the form of a crash.  It took me awhile to debug, but
         * eventually stepping through code showed me that if a user
         * enters only white space for a string, the database itself
         * will store it as white space, but the CustomValidationAttribute
         * was passing it as a null string.  Once I knew the cause of the
         * crash, I solved it by just returning if null was detected.
         * 
         * The next step will be to see what it takes to identify duplicate
         * entries.  CustomValidationAttributes might not do the trick
         * if there isn't a way to get user data from multiple columns
         * at once.
         */
    }
}

// REFERENCE:
// syntax for RegularExpression attribute validation (without CustomValidation)
// [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Cannot begin or end with white space.")]