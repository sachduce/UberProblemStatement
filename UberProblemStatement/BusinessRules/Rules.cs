using System;
using System.Collections.Generic;
using System.Text;
using UberProblemStatement.Interfaces;
using System.Data.SqlClient;
using UberProblemStatement.DTO;

namespace UberProblemStatement.BusinessRules
{
    public class Rules: IRules
    {
        // Establishing SQL Connection 
        SqlConnection sqlConnection = new SqlConnection("integrated security=sspi;Server=LOVISH3146494\\SQL2012;database=UberDatabase");

        // Calculation of fare logic based on business rules
        public int FareCharge(IFareParameterDTO fareParameter)
        {
            // Min fare 
            int minFare = 50;
            // Cancellation fare 
            int cancellationFare = 50;
            double waitingFare = 0;
            if (fareParameter.WaitingTime > 4)
            {
                waitingFare = Math.Round(fareParameter.WaitingTime - 4) * 10;
            }
            // ride cancellation fare logic
            if (fareParameter.RideCancelled == true)
            {
                return cancellationFare + (int)waitingFare;
            }
            // Fare Calculation Logic
            else
            {
                double baseFare = fareParameter.Surge* (fareParameter.Distance * 8 + fareParameter.TravelTime * 1);
                int actualFare = (int)(baseFare+waitingFare);
                // returns max of minFare and actualFare
                return actualFare>minFare? actualFare:minFare;
            }
            
        }
 
        //Updating or Removing driver details based on business rules
        public IDriverDTO DriverUpdate(IDriverDTO driverDTO,int lastTripRating)
        {
            // To avoid the edge case when numberOfTrips less than 5 since value is passed by referenece driverDTO.Rating gets updated to 5 even though in db its less than 5
            sqlConnection.Open();
            string query = "select  * from [dbo].[Driver] where Id = '" + driverDTO.Id + "'";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var value = reader.GetValue(4);
                    driverDTO.Rating = (float)Convert.ToDecimal(value);
                }
            }
            reader.Close();
            // Updating value of NumberOfTrips and Rating in driverDTO object
            driverDTO.NumberOfTrips++;
            float totalRating = driverDTO.Rating * (driverDTO.NumberOfTrips - 1);
            driverDTO.Rating = ( totalRating + lastTripRating) / driverDTO.NumberOfTrips;
            // Open SQL Connection
            
            // SQL Query to update rating and NumberOfTrips finding driver based on ID which is primary key
            string query1 = "UPDATE [dbo].[Driver] SET Rating = '" + driverDTO.Rating + "', NumberOfTrips = '" + driverDTO.NumberOfTrips + "' where Id = '" + driverDTO.Id + "'";
            // Creates a SQL Command
            SqlCommand command1 = new SqlCommand(query1, sqlConnection);
            // Driver details updated in SQL Database
             reader= command1.ExecuteReader();
            // Closes the Object
            reader.Close();
            // Deleting driver from uber database if follows the criteria
            if(driverDTO.NumberOfTrips>5 && driverDTO.Rating < 4)
            {
                // Driver deletion query
                string query2 = "Delete from [dbo].[Driver] where Id = '" + driverDTO.Id + "'";
                SqlCommand command2 = new SqlCommand(query2, sqlConnection);
                // Driver details deleted from SQL database
                reader = command2.ExecuteReader();
                reader.Close();
            }
            sqlConnection.Close();
            return driverDTO;
        }

        //Assigning Driver to Customer function based on type
        public IDriverDTO MappingCustomerToDriver(int typeOfCustomer)
        {
            IDriverDTO driverDTO = new DriverDTO();
            
            //Open the connection
            sqlConnection.Open();

            string query = "";
            switch (typeOfCustomer)
            {
                case 1:
                    //Query for Silver Type Customer
                    query = "select Top 1 * from [dbo].[Driver]  order by newid()";
                    break;

                case 2:
                    //Query for Gold Type Customer
                    query = "select Top 1 * from [dbo].[Driver] where Rating >= 4.5 OR NumberOftrips <=5  order by newid()";
                    break;

                case 3:
                    //Query for Platinum Type Customer
                    query = "select Top 1 * from [dbo].[Driver] where Rating >= 4.8  OR NumberOftrips <=5  order by newid()";
                    break;
            }

            // creates a SQL command
            SqlCommand command = new SqlCommand(query, sqlConnection);


            //results from the SQL query is recieved in SQL reader
            SqlDataReader dataReader = command.ExecuteReader();

            //Checks if dataReader has data
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    // Assigning data from dataReader to driverDTO
                    driverDTO.Id = dataReader.GetInt32(0);
                    driverDTO.Name = dataReader.GetString(1);
                    driverDTO.Contact = dataReader.GetString(3);
                    var value = dataReader.GetValue(4);
                    driverDTO.Rating = (float)Convert.ToDecimal(value);
                    driverDTO.NumberOfTrips = dataReader.GetInt32(2);
                    // Assining rating 5 to the drivers having number of trips less than 5
                    if (driverDTO.NumberOfTrips <= 5)
                    {
                        driverDTO.Rating = 5;
                    }
                }    
                dataReader.Close();
            }
            else
            {
                // Assigning driver with maximum rating, if no driver satisfies the required criteria for given type of customer
                query = "select Top 1 * from [dbo].[Driver] where Rating = (select max(Rating) from [dbo].[Driver])  order by newid()";
                SqlCommand commmand2 = new SqlCommand(query, sqlConnection);
                dataReader = commmand2.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        driverDTO.Id = dataReader.GetInt32(0);
                        driverDTO.Name = dataReader.GetString(1);
                        driverDTO.Contact = dataReader.GetString(3);
                        var value = dataReader.GetValue(4);
                        driverDTO.Rating = (float)Convert.ToDecimal(value);
                        driverDTO.NumberOfTrips = dataReader.GetInt32(2);
                    }
                    dataReader.Close();
                }
            }

            //Connection is closed
            sqlConnection.Close();

            return driverDTO;

        }
    }
}
