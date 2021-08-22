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
    public class ItemService : IItemService
    {

        private readonly ApplicationDbContext _dataContext;

        public ItemService(ApplicationDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
             return await _dataContext.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int itemId)
        {
            return await _dataContext.Items.SingleOrDefaultAsync(x => x.Id == itemId);
        }

        public async Task<bool> UpdateItemAsync(Item itemToUpdate)
        {
            _dataContext.Items.Update(itemToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteItemAsync(int itemId){
            var item = await GetItemByIdAsync(itemId);
            _dataContext.Items.Remove(item);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreateItemAsync(Item item)
        {
            await _dataContext.Items.AddAsync(item);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }


    }
}