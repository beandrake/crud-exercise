using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PassengerManager.Models
{
    // note that for the purpose of this project,
    // this object definition is also defining the associated database schema
    public class Passenger
    {
        // primary key
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}