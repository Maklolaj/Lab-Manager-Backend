using LabManAPI.Data;
using LabManAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LabManAPI.Contracts.Responses;
using System.Linq;
using System;

namespace LabManAPI.Services
{
    public class ReservationService : IReservationService
    {

        private readonly ApplicationDbContext _dataContext;

        public ReservationService(ApplicationDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public async Task<List<Reservation>> GetReservationsAsync()
        {
            return await _dataContext.Reservations.Include(x => x.Item).ToListAsync();
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(string userId)
        {
            return await _dataContext.Reservations.Where(x => x.User.Id == userId).Include(x => x.Item).ToListAsync();
        }

        public async Task<Reservation> GetRservationByIdAsync(int reservationId)
        {
            return await _dataContext.Reservations.SingleOrDefaultAsync(x => x.Id == reservationId);
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservationToUpdate)
        {
            _dataContext.Reservations.Update(reservationToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteReservationAsync(int reservationId)
        {
            var item = await GetRservationByIdAsync(reservationId);
            _dataContext.Reservations.Remove(item);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreateReservationAsync(Reservation reservation)
        {
            var exisitngReservation = await _dataContext.Reservations.Where(x => x.Item.Id == reservation.Item.Id && x.StartDate == reservation.StartDate).ToListAsync();
            if (!exisitngReservation.Any())
            {
                await _dataContext.Reservations.AddAsync(reservation);
                var created = await _dataContext.SaveChangesAsync();
                return created > 0;
            }
            return false;

        }

        public async Task<List<ReservationsFromDateResponse>> GetReservationsWithCorrespondingDate(DateTime startRange, DateTime endRange, int itemId)
        {
            var reservations = new List<ReservationsFromDateResponse>();

            var resetaionsThatDay = await _dataContext.Reservations
            .Where(x => x.StartDate > startRange && x.EndDate < endRange && x.Item.Id == itemId).ToListAsync();

            var resetaionsDayBefore = await _dataContext.Reservations
            .Where(x => x.StartDate > startRange.AddDays(-1) && x.EndDate < endRange.AddDays(-1) && x.Item.Id == itemId).ToListAsync();

            var resetaionsDayAfter = await _dataContext.Reservations
           .Where(x => x.StartDate > startRange.AddDays(1) && x.EndDate < endRange.AddDays(1) && x.Item.Id == itemId).ToListAsync();

            reservations.AddRange(new List<ReservationsFromDateResponse>
            {   new ReservationsFromDateResponse{
                    Day = $"{startRange.Year}-{startRange.Month}-{startRange.AddDays(-1).Day}",
                    Reservations = CreateDailyRservationForItem(resetaionsDayBefore),
                },
                new ReservationsFromDateResponse{
                    Day = $"{startRange.Year}-{startRange.Month}-{startRange.Day}",
                    Reservations = CreateDailyRservationForItem(resetaionsThatDay),
                },
                new ReservationsFromDateResponse{
                    Day = $"{startRange.Year}-{startRange.Month}-{startRange.AddDays(1).Day}",
                    Reservations = CreateDailyRservationForItem(resetaionsDayAfter),
                }
            });

            return reservations;
        }

        public List<Reservation> CreateDailyRservationForItem(List<Reservation> resList)
        {

            var result = new List<Reservation>();
            result.AddRange(new List<Reservation>
            {
                new Reservation{},
                new Reservation{},
                new Reservation{},
                new Reservation{},
                new Reservation{},
                new Reservation{},

            });

            foreach (Reservation value in resList)
            {
                switch ((value.StartDate.Hour, value.EndDate.Hour))
                {
                    case (8, 10):
                        result[0] = value;
                        break;

                    case (10, 12):
                        result[1] = value;
                        break;

                    case (12, 14):
                        result[2] = value;
                        break;

                    case (14, 16):
                        result[3] = value;
                        break;

                    case (16, 18):
                        result[4] = value;
                        break;

                    case (18, 20):
                        result[5] = value;
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

    }
}