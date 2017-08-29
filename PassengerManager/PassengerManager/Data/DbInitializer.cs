/* Want to see your database?  In the above menu:
 * View -> SQL Server Object Explorer
 * It should be right inside the Databases directory.
 * 
 * To find out where your database is on disk:
 *   Right-click PassengerManager1 and select Properties,
 *   then scroll down in the Properties pane.
 *   
 * To view your data:
 *   Open your database's Tables folder, then right-click on
 *   dbo.Passenger, then select View Data.
*/
using PassengerManager.Models;
using System;
using System.Linq;

namespace PassengerManager.Data
{
    public static class DbInitializer
    {
        // this creates or reads from a local SQL database
        public static void Initialize(OverallContext context)
        {
            context.Database.EnsureCreated();

            // NOTE: the rest of this method is only for testing;
            // in production an empty database is valid and should NOT be filled with test data

            // If data already exists, no need to initialize
            if (context.Passengers.Any())
            {
                return;
            }

            // otherwise make some test passengers
            // WARNING: test data is NOT validated by our schema's restrictions
            var passengerGroup = new Passenger[]
            {
                new Passenger{FirstName="Carson",LastName="Alexander",PhoneNumber="555-123-4556"},
                new Passenger{FirstName="Meredith",LastName="Alonso",PhoneNumber="555-845-6734"},
                new Passenger{FirstName="Arturo",LastName="Anand",PhoneNumber="555-425-8023"},
                new Passenger{FirstName="Gytis",LastName="Barzdukas",PhoneNumber="555-342-7886"},
                new Passenger{FirstName="Yan",LastName="Li",PhoneNumber="555-950-7100"},
                new Passenger{FirstName="Peggy",LastName="Justice",PhoneNumber="555-512-0056"},
                new Passenger{FirstName="Laura",LastName="Norman",PhoneNumber="555-329-9945"},
                new Passenger{FirstName="Joe",LastName="Phoneless"},
                new Passenger{FirstName="Sweden",LastName="The Country",PhoneNumber="+46 771 793 336"}
            };

            // add the new passengers to the context list
            foreach (Passenger person in passengerGroup)
            {
                context.Passengers.Add(person);
            }

            // All of the changes to the context are saved to the database now.
            // It's at this point that our first Passengers will be given their database-generated ID's.
            context.SaveChanges();
        }
    }
}