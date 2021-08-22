  
using LabManAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LabManAPI.Services
{
    public interface IItemService
    {
         Task<List<Item>> GetItemsAsync();

        Task<Item> GetItemByIdAsync(int itemId);

        Task<bool> UpdateItemAsync(Item itemToUpdate);

        Task<bool> DeleteItemAsync(int itemId);

        Task<bool> CreateItemAsync(Item item);

    }
}