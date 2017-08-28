using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassengerManager.Models
{
    // note that for the purpose of this project,
    // this object definition is also defining the associated database schema
    public class Passenger
    {
        // primary key
        // The below line tells the database to generate a unique ID upon insertion
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        // not requiring this because people without phones deserve cruises, too
        [StringLength(50)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}