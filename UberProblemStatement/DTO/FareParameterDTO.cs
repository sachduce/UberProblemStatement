using System;
using System.Collections.Generic;
using System.Text;
using UberProblemStatement.Interfaces;

namespace UberProblemStatement.DTO
{
    public class FareParameterDTO : IFareParameterDTO
    {
        public double Distance { get; set; }
        public double TravelTime { get; set; }
        public double WaitingTime { get; set; }
        public bool RideCancelled { get; set; }
        public double Surge { get; set; }
        
    }
}
