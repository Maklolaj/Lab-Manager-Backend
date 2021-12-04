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
            return await _dataContext.Faults.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Fault> GetFaultByIdAsync(int faultId)
        {
            return await _dataContext.Faults.SingleOrDefaultAsync(x => x.Id == faultId);
        }

        public async Task<bool> CreateFaultAsync(Fault fault)
        {
            await _dataContext.Faults.AddAsync(fault);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteFaultAsync(int faultId)
        {
            var fault = await GetFaultByIdAsync(faultId);
            fault.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
            return fault.IsDeleted;
        }



    }

}