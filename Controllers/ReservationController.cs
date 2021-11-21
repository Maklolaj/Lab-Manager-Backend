using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Contracts;
using LabManAPI.Services;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;

namespace LabManAPI.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IItemService _itemService;

        public ReservationController(IReservationService reservationService, IItemService itemService)
        {
            _reservationService = reservationService;
            _itemService = itemService;
        }

        [HttpGet(ApiRoutes.Reservation.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _reservationService.GetReservationsAsync());
        }

        [HttpGet(ApiRoutes.Reservation.Create)]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            var item = await _itemService.GetItemByIdAsync(request.ItemId);

            var reservation = new Reservation
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Item = item
            };

            await _reservationService.CreateReservationAsync(reservation);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Reservation.Get.Replace("{reservationId}", reservation.Id.ToString());

            var response = new ReservationResponse
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
            };

            return Created(locationUrl, response);
        }

    }
}

