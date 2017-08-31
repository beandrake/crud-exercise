// initially adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
// attributes adapted from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassengerManager.Models
{
    // this object definition is also defining the associated database schema
    public class Passenger
    {
        // ID is the primary key
        // The below line tells the database to generate a unique ID upon insertion
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Cannot begin or end with a space.")]        
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Cannot begin or end with a space.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        // not requiring this because people without phones deserve cruises, too
        [StringLength(50)]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Cannot begin or end with a space.")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}