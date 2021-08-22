using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Contracts.Requests
{
    public class UpdateItemRequest
    {
        public string name { get; set; }
        public string describiton { get; set; }
        public bool isDamaged { get; set; }
    }
}