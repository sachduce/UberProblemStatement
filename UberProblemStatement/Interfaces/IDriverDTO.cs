using System;
using System.Collections.Generic;
using System.Text;

namespace UberProblemStatement.Interfaces
{
    public interface IDriverDTO
    {
        int Id { get; set;}
        string Name { get; set; }
        int NumberOfTrips { get; set; }
        string Contact { get; set; }
        float Rating { get; set; }
    }
}
