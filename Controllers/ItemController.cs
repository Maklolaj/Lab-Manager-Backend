using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Contracts;
using LabManAPI.Services;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LabManAPI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace LabManAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        private readonly IGeneralExtensions _extensions;

        public ItemController(IItemService itemService, IGeneralExtensions extensions)
        {
            _itemService = itemService;
            _extensions = extensions;
        }

        [HttpGet(ApiRoutes.Item.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var isAdmin = await _extensions.isAdmin(await HttpContext.GetTokenAsync("access_token"));
            if (!isAdmin)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User is not allowed");
            }

            return Ok(await _itemService.GetItemsAsync());
        }

        [HttpPost(ApiRoutes.Item.Create)]
        public async Task<IActionResult> Create([FromBody] CreateItemRequest itemRequest)
        {
            var isAdmin = await _extensions.isAdmin(await HttpContext.GetTokenAsync("access_token"));
            if (!isAdmin)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User is not allowed");
            }

            var item = new Item
            {
                Name = itemRequest.Name,
                Manufacturer = itemRequest.Manufacturer,
                ProductionDate = itemRequest.ProductionDate,
                Description = itemRequest.Description,
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
            var isAdmin = await _extensions.isAdmin(await HttpContext.GetTokenAsync("access_token"));
            if (!isAdmin)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User is not allowed");
            }

            var deleted = await _itemService.DeleteItemAsync(itemId);

            if (deleted)
                return NoContent();

            return NotFound();
        }


        [HttpPut(ApiRoutes.Item.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateItemRequest request, [FromRoute] int itemId)
        {
            var isAdmin = await _extensions.isAdmin(await HttpContext.GetTokenAsync("access_token"));
            if (!isAdmin)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User is not allowed");
            }

            var item = await _itemService.GetItemByIdAsync(itemId);

            item.Name = request.Name;
            item.Manufacturer = request.Manufacturer;
            item.ProductionDate = request.ProductionDate;
            item.Description = request.Description;

            if (await _itemService.UpdateItemAsync(item))
                return Ok(item);

            return NotFound();


        }


    }
}

