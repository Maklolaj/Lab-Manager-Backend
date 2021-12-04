using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Services;
using LabManAPI.Contracts;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;
using LabManAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace LabManAPI.Controllers
{
    public class FaultController : Controller
    {
        private readonly IFaultService _faultService;

        private readonly IItemService _itemService;

        private readonly IIdentityService _identityService;
        public FaultController(IFaultService faultService, IItemService itemService, IIdentityService identityService)
        {
            _faultService = faultService;
            _itemService = itemService;
            _identityService = identityService;
        }

        [HttpGet(ApiRoutes.Fault.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _faultService.GetFaultsAsync());
        }

        [HttpPost(ApiRoutes.Fault.Create)]
        public async Task<IActionResult> Create([FromBody] CreateFaultRequest fault)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
            }

            var new_fault = new Fault()
            {
                Item = await _itemService.GetItemByIdAsync(fault.ItemId),
                User = currentUser,
                Description = fault.Description,
                reportTIme = DateTime.UtcNow,
                IsDeleted = false,
            };

            await _faultService.CreateFaultAsync(new_fault);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Item.Get.Replace("{faultId}", new_fault.Id.ToString());

            var response = new FaultResponse
            {
                Id = new_fault.Id,
                Description = new_fault.Description,
            };

            return Created(locationUrl, response);
        }

        [HttpDelete(ApiRoutes.Fault.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int faultId)
        {
            var deleted = await _faultService.DeleteFaultAsync(faultId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }

}
