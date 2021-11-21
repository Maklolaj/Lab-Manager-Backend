using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Contracts;
using LabManAPI.Services;
using System;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;

namespace LabManAPI.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet(ApiRoutes.Item.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _itemService.GetItemsAsync());
        }

        [HttpPost(ApiRoutes.Item.Create)]
        public async Task<IActionResult> Create([FromBody] CreateItemRequest itemRequest)
        {
            var item = new Item
            {
                Name = itemRequest.Name,
                Manufacturer = itemRequest.Manufacturer,
                ProductionDate = itemRequest.ProductionDate,
                Describiton = itemRequest.Describiton,
                IsDamaged = false,
                IsDeleted = false,
            };

            await _itemService.CreateItemAsync(item);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Item.Get.Replace("{itemId}", item.Id.ToString());

            var response = new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
            };

            return Created(locationUrl, response);
        }

        [HttpDelete(ApiRoutes.Item.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int itemId)
        {
            var deleted = await _itemService.DeleteItemAsync(itemId);

            if (deleted)
                return NoContent();

            return NotFound();
        }


        [HttpPut(ApiRoutes.Item.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateItemRequest request, [FromRoute] int itemId)
        {

            var item = await _itemService.GetItemByIdAsync(itemId);

            item.Name = request.Name;
            item.Manufacturer = request.Manufacturer;
            item.ProductionDate = request.ProductionDate;
            item.Describiton = request.Describiton;
            item.IsDamaged = request.IsDamaged;
            item.IsDeleted = request.IsDeleted;

            if (await _itemService.UpdateItemAsync(item))
                return Ok(item);

            return NotFound();


        }


    }
}

