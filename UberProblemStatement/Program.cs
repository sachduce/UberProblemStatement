using System;
using System.Linq;
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
                int type = inputCustomerType();

                // Assigning driver to customer based on its type           
                driverDTO = rules.MappingCustomerToDriver(type);

                Console.WriteLine("Your Ride details:");
                Console.WriteLine("Driver Name:- " + driverDTO.Name);
                Console.WriteLine("Driver Rating:- " + driverDTO.Rating);

                Console.WriteLine("****************************** Fare parameters ******************************");
                // Assining fare parameters from console to fareParameterDTO object
                Console.WriteLine("Please Enter Distance Travelled:- ");
                
                fareParameterDTO.Distance = inputDouble();
                

                Console.WriteLine("Please Enter Travel Time:- ");
                fareParameterDTO.TravelTime = inputDouble();

                Console.WriteLine("Please Enter Waiting Time:- ");
                fareParameterDTO.WaitingTime = inputDouble();

                Console.WriteLine("Please Enter Surge:- ");
                fareParameterDTO.Surge = inputDouble();

                Console.WriteLine("Enter Y to cancel booking or pres any other key to continue:- ");
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
                    int lastTripRating = inputRating();
                    // Updating or Removing driver details in SQL Database
                    driverDTO = rules.DriverUpdate(driverDTO, lastTripRating);
                }

                Console.WriteLine("Thank You");
                Console.WriteLine("****************************************************");
                Console.WriteLine("press 0 to exit");
                Console.WriteLine("Press 1 to continue booking with uber");
                iterator = inputIterator();           
            }
           
        }
        public static double inputDouble()
        {
            String input = Console.ReadLine();
            while (true)
            {
                if (input == "")
                {
                    Console.Write("You have not enter any value, please enter a value \n");
                    input = Console.ReadLine();
                    continue;
                }
                if(input[0] == '.' || input.Count(x => x == '.') > 1)
                {
                    Console.Write("Value cannot start with a dot or contain multiple dots \n");
                    input = Console.ReadLine();
                    continue;
                }
                bool isDigit = true;
                foreach (char item in input)
                {
                    if (!char.IsDigit(item) && item != '.')
                    {
                        Console.Write("Please enter a number....\n");
                        isDigit = false;
                        break;
                    }
                }
                if (!isDigit)
                {
                    input = Console.ReadLine();
                    continue;
                }
                
                break;
            }
            return Double.Parse(input);
        }
        public static int inputCustomerType()
        {
            string x = Console.ReadLine();
            while (true)
            {
                if (x.Any(i => !char.IsDigit(i)))
                {
                    Console.WriteLine("Please enter a number");
                    x = Console.ReadLine();
                    continue;
                }
                if (Int32.Parse(x) < 1 || Int32.Parse(x) > 3)
                {
                    Console.WriteLine("Please enter a number between 1 to 3");
                    x = Console.ReadLine();
                    continue;
                }
                break;
            }   
            return Int32.Parse(x);
        }
        public static int inputRating()
        {
            string x = Console.ReadLine();
            while (true)
            {
                if (x.Any(i => !char.IsDigit(i)))
                {
                    Console.WriteLine("Please enter a number");
                    x = Console.ReadLine();
                    continue;
                }
                if (Int32.Parse(x) < 1 || Int32.Parse(x) > 5)
                {
                    Console.WriteLine("Please enter a number between 1 to 5");
                    x = Console.ReadLine();
                    continue;
                }
                break;
            }
            return Int32.Parse(x);
        }
        public static int inputIterator()
        {
            string x = Console.ReadLine();
            while (true)
            {
                if (x.Any(i => !char.IsDigit(i)))
                {
                    Console.WriteLine("Please enter a number");
                    x = Console.ReadLine();
                    continue;
                }
                if (Int32.Parse(x) < 0 || Int32.Parse(x) > 1)
                {
                    Console.WriteLine("Please enter  0 or 1");
                    x = Console.ReadLine();
                    continue;
                }
                break;
            }
            return Int32.Parse(x);
        }

    }
}

