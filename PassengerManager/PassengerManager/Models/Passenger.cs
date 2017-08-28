using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PassengerManager.Models
{
    // note that for the purpose of this project,
    // this object definition is also defining the associated database schema
    public class Passenger
    {
        public int ID { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}