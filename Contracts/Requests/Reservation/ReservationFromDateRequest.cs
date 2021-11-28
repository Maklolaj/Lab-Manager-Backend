using System;

namespace LabManAPI.Contracts.Requests
{
    public class ReservationFromDateRequest
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int ItemId { get; set; }
    }
}




