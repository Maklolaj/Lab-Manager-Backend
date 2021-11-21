using System;
using Microsoft.AspNetCore.Identity;
using LabManAPI.Models;

namespace LabManAPI.Contracts.Requests
{
    public class CreateReservationRequest
    {
        public int ItemId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}