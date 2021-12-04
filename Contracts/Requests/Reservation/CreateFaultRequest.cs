using System;
using Microsoft.AspNetCore.Identity;
using LabManAPI.Models;

namespace LabManAPI.Contracts.Requests
{
    public class CreateFaultRequest
    {
        public int ItemId { get; set; }

        public string Description { get; set; }

    }
}