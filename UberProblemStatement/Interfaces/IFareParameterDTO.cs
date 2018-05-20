using System;
using System.Collections.Generic;
using System.Text;

namespace UberProblemStatement.Interfaces
{
    public interface IFareParameterDTO
    {
        double Distance { get; set; }
        double TravelTime { get; set; }
        double WaitingTime { get; set; }
        bool RideCancelled { get; set; }
        double Surge { get; set; }
        
    }
}
