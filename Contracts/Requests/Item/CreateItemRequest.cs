using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Contracts.Requests
{
    public class CreateItemRequest
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Describiton { get; set; }
        public bool isDamaged { get; set; }
        public bool IsDeleted { get; set; }
    }
}