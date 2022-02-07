using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Contracts;
using LabManAPI.Services;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using LabManAPI.Extensions;

namespace LabManAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IItemService _itemService;

        private readonly IIdentityService _identityService;

        private readonly ILogger<ReservationController> _logger;

        private readonly ILoggerAdapter<ReservationController> _loggerApater;

        private readonly ILoggerFactory _logerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });

        public ReservationController(IReservationService reservationService, IItemService itemService, IIdentityService identityService)
        {
            _reservationService = reservationService;
            _itemService = itemService;
            _identityService = identityService;
            _logger = new Logger<ReservationController>(_logerFactory);
            _loggerApater = new LoggerAdapter<ReservationController>(_logger);
        }

        [HttpGet(ApiRoutes.Reservation.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            _loggerApater.LogInformation("GET ALL RESERVATIONS");

            return Ok(await _reservationService.GetReservationsAsync());
        }

        [HttpPost(ApiRoutes.Reservation.Create)]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Dates are invalid"); //test 
            }

            var item = await _itemService.GetItemByIdAsync(request.ItemId);
            if (item == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Item with id={request.ItemId} does not exist");
            }

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
            }

            var reservation = new Reservation
            {
                StartDate = request.StartDate.AddHours(1), //parsing problem on clint side  
                EndDate = request.EndDate.AddHours(1),  //parsing problem on clint side  
                Item = item,
                User = currentUser,
            };

            if (await _reservationService.CreateReservationAsync(reservation))
            {
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Reservation already exists");

        }


        [HttpDelete(ApiRoutes.Reservation.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int reservationId)
        {

            var deleted = await _reservationService.DeleteReservationAsync(reservationId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPut(ApiRoutes.Reservation.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateReservationRequest request, [FromRoute] int reservationId)
        {
            var reservation = await _reservationService.GetRservationByIdAsync(reservationId);

            reservation.StartDate = request.StartDate;
            reservation.EndDate = request.EndDate;

            if (await _reservationService.UpdateReservationAsync(reservation))
                return Ok(reservation);

            return NotFound();

        }

        [HttpPost(ApiRoutes.Reservation.GetFromDate)]
        public async Task<IActionResult> GetFromDate([FromBody] ReservationFromDateRequest dateRequest)
        {
            // TEST DATES 
            //"2021-11-20 08:08";
            //"2021-11-20 22:08";
            DateTime startDate = DateTime.ParseExact(dateRequest.StartDate, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(dateRequest.EndDate, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            var reservations = await _reservationService.GetReservationsWithCorrespondingDate(startDate, endDate, dateRequest.ItemId);

            return Ok(reservations);
        }

        [HttpGet(ApiRoutes.Reservation.GetFromUser)]
        public async Task<IActionResult> GetFromUser()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
            }

            var reservations = await _reservationService.GetUserReservationsAsync(currentUser.Id);

            return Ok(reservations);
        }

    }
}

