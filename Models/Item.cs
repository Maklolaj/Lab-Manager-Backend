using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;


namespace LabManAPI.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string describiton { get; set; }
        public bool isDamaged { get; set; }
    }
}