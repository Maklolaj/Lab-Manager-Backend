using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManAPI.Models;

namespace LabManAPI.Contracts.Responses
{
    public class ReservationsFromDateResponse
    {
        public string Day { get; set; }

        public List<Reservation> Reservations { get; set; }

    }
}