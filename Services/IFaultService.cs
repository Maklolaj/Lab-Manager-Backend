
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LabManAPI.Services
{
    public interface IFaultService
    {
        Task<List<Fault>> GetFaultsAsync();

        Task<Fault> GetFaultByIdAsync(int faultId);

        Task<bool> CreateFaultAsync(Fault fault);

        Task<bool> DeleteFaultAsync(int faultId);


    }
}