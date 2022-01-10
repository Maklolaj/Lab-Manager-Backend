
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LabManAPI.Extensions
{
    public interface IGeneralExtensions
    {
        Task<bool> isAdmin(string acessToken);
    }
}