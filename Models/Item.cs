using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;


namespace LabManAPI.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Describiton { get; set; }
        public bool IsDamaged { get; set; }
        public bool IsDeleted { get; set; }

    }
}