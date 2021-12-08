using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Contracts.Responses
{
    public class AllUsersResponse
    {
        public string userName { get; set; }

        public string userEmail { get; set; }

        public string userPhoneNumber { get; set; }
    }
}