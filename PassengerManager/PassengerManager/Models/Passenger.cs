using System;
using System.Collections.Generic;

namespace PassengerManager.Models
{
    public class Passenger
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
    }
}