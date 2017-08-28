using PassengerManager.Models;
using System;
using System.Linq;

namespace PassengerManager.Data
{
    public static class DbInitializer
    {
        public static void Initialize(OverallContext context)
        {
            context.Database.EnsureCreated();

            // If data already exists, no need to initialize
            if (context.Passengers.Any())
            {
                return;
            }

            // otherwise make some test passengers
            var passengerGroup = new Passenger[]
            {
                new Passenger{FirstName="Carson",LastName="Alexander",PhoneNumber="555-123-4556"},
                new Passenger{FirstName="Meredith",LastName="Alonso",PhoneNumber="555-845-6734"},
                new Passenger{FirstName="Arturo",LastName="Anand",PhoneNumber="555-425-8023"},
                new Passenger{FirstName="Gytis",LastName="Barzdukas",PhoneNumber="555-342-7886"},
                new Passenger{FirstName="Yan",LastName="Li",PhoneNumber="555-950-7100"},
                new Passenger{FirstName="Peggy",LastName="Justice",PhoneNumber="555-512-0056"},
                new Passenger{FirstName="Laura",LastName="Norman",PhoneNumber="555-329-9945"},
                new Passenger{FirstName="Nino",LastName="Olivetto",PhoneNumber="555-867-7821"},
            };

            // add the new passengers to the list
            foreach (Passenger person in passengerGroup)
            {
                context.Passengers.Add(person);
            }

            // "Persists all updates to the data source and resets change tracking in the object context."
            // From what I gather, this propagates the changes in the C# object to the actual database.
            // It's at this point that our Passengers will be given their database-generated ID's.
            context.SaveChanges();
        }
    }
}