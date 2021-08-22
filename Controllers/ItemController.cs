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
            var item = new Item {
                name = itemRequest.name,
                describiton = itemRequest.describiton
            };

            await _itemService.CreateItemAsync(item);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Item.Get.Replace("{itemId}", item.Id.ToString());

            var response = new ItemResponse 
            {
                Id = item.Id,
                name = item.name,
            };

            return Created(locationUrl, response);
        }

         [HttpDelete(ApiRoutes.Item.Delete)]
        public async Task<IActionResult> Delete([FromRoute]int itemId)
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

            item.name = request.name;
            item.describiton = request.describiton;
            item.isDamaged = request.isDamaged;
            
            if (await _itemService.UpdateItemAsync(item))
                return Ok(item);

            return NotFound();


        }


    }
}

