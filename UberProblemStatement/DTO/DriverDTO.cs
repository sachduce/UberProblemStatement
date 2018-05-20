using System;
using System.Collections.Generic;
using System.Text;
using UberProblemStatement.Interfaces;

namespace UberProblemStatement.DTO
{
    public class DriverDTO: IDriverDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfTrips { get; set; }
        public string Contact { get; set; }
        public float Rating { get; set; }
    }
}
