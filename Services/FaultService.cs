using LabManAPI.Data;
using LabManAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LabManAPI.Services
{
    public class FaultService : IFaultService
    {
        private readonly ApplicationDbContext _dataContext;

        public FaultService(ApplicationDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public async Task<List<Fault>> GetFaultsAsync()
        {

        }

        public async Task<bool> CreateFaultAsync(Fault fault)
        {

        }

        public async Task<bool> DeleteReservationAsync(int faultId)
        {

        }



    }

}