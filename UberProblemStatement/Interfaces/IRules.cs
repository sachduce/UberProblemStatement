using System;
using System.Collections.Generic;
using System.Text;

namespace UberProblemStatement.Interfaces
{
    public interface IRules
    {
        int FareCharge(IFareParameterDTO fareParameter);
        IDriverDTO DriverUpdate(IDriverDTO driverDTO,int lastTripRating);
        IDriverDTO MappingCustomerToDriver(int typeOfCustomer);
    }
}
