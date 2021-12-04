using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Contracts.Requests
{
    public class UpdateItemRequest
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Description { get; set; }
    }
}