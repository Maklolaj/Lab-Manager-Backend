using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Contracts.Requests
{
    public class CreateItemRequest
    {
        public string name { get; set; }
        public string describiton { get; set; }
    }
}