using System;
using UberProblemStatement.BusinessRules;
using UberProblemStatement.DTO;
using UberProblemStatement.Interfaces;

namespace UberProblemStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            
            IDriverDTO driverDTO = new DriverDTO();
            IRules rules = new Rules();
            IFareParameterDTO fareParameterDTO = new FareParameterDTO();
            int iterator = 1;
            while(iterator != 0)
            {
                Console.WriteLine("Please enter Customer type");
                Console.WriteLine("1. Silver type");
                Console.WriteLine("2. Gold type");
                Console.WriteLine("3. Platinum type");

                // Take integer as an input for assigning customer type  1. Silver 2. Gold 3.Platinum
                int type = Int32.Parse(Console.ReadLine());

                // Assigning driver to customer based on its type           
                driverDTO = rules.MappingCustomerToDriver(type);

                Console.WriteLine("Your Ride details:");
                Console.WriteLine("Driver Name:- " + driverDTO.Name);
                Console.WriteLine("Driver Rating:- " + driverDTO.Rating);

                Console.WriteLine("****************************** Fare parameters ******************************");
                // Assining fare parameters from console to fareParameterDTO object
                Console.WriteLine("Please Enter Distance Travelled:- ");
                fareParameterDTO.Distance = Double.Parse(Console.ReadLine());

                Console.WriteLine("Please Enter Travel Time:- ");
                fareParameterDTO.TravelTime = Double.Parse(Console.ReadLine());

                Console.WriteLine("Please Enter Waiting Time:- ");
                fareParameterDTO.WaitingTime = Double.Parse(Console.ReadLine());

                Console.WriteLine("Please Enter Surge:- ");
                fareParameterDTO.Surge = Double.Parse(Console.ReadLine());

                Console.WriteLine("Please Enter Booking Cancelled(Y/N):- ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    fareParameterDTO.RideCancelled = true;
                }
                else
                {
                    fareParameterDTO.RideCancelled = false;
                }
                // Calculation of fare based on the given business rules
                int fare = rules.FareCharge(fareParameterDTO);

                if (fareParameterDTO.RideCancelled == true)
                {
                    Console.WriteLine("Cancellation fare is {0}", fare);
                }
                else
                {
                    Console.WriteLine("Your fare is {0}", fare);
                    Console.WriteLine("Please rate your trip from 1 to 5 with 5 being completely satisfied");
                    // Input driver rating  from console
                    int lastTripRating = Int32.Parse(Console.ReadLine());
                    // Updating or Removing driver details in SQL Database
                    driverDTO = rules.DriverUpdate(driverDTO, lastTripRating);
                }

                Console.WriteLine("Thank You");
                Console.WriteLine("****************************************************");
                Console.WriteLine("press 0 to exit");
                Console.WriteLine("Press 1 to continue booking with uber");
                iterator = Int32.Parse(Console.ReadLine());
                
            }
        }
    }
}
