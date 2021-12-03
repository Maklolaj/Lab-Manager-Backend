using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LabManAPI.Models
{
    public class Fault
    {
        [Key]
        public int Id { get; set; }

        public Item Item { get; set; }

        public IdentityUser User { get; set; }

        public string Description { get; set; }

        public DateTime reportTIme { get; set; }

        public bool IsDeleted { get; set; }

    }
}