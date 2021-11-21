using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LabManAPI.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public Item Item { get; set; }

        public IdentityUser User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}