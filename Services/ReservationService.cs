using LabManAPI.Data;
using LabManAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LabManAPI.Contracts.Responses;
using System.Linq;
using LabManAPI.Migrations;
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
            return await _dataContext.Reservations.ToListAsync();
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
            await _dataContext.Reservations.AddAsync(reservation);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<List<ReservationsFromDateResponse>> GetReservationsWithCorrespondingDate(DateTime startRange, DateTime endRange)
        {
            var reservations = new List<ReservationsFromDateResponse>();

            var resetaionsThatDay = await _dataContext.Reservations
            .Where(x => x.StartDate > startRange && x.EndDate < endRange).ToListAsync();

            var resetaionsDayBefore = await _dataContext.Reservations
            .Where(x => x.StartDate > startRange.AddDays(-1) && x.EndDate < endRange.AddDays(-1)).ToListAsync();

            var resetaionsDayAfter = await _dataContext.Reservations
           .Where(x => x.StartDate > startRange.AddDays(1) && x.EndDate < endRange.AddDays(1)).ToListAsync();

            reservations.AddRange(new List<ReservationsFromDateResponse>
            {   new ReservationsFromDateResponse{
                    Day = $"{startRange.AddDays(-1).Day}-{startRange.Month}-{startRange.Year}",
                    Reservations = resetaionsDayBefore,
                },
                new ReservationsFromDateResponse{
                    Day = $"{startRange.Day}-{startRange.Month}-{startRange.Year}",
                    Reservations = resetaionsThatDay,
                },
                new ReservationsFromDateResponse{
                    Day = $"{startRange.AddDays(1).Day}-{startRange.Month}-{startRange.Year}",
                    Reservations = resetaionsDayAfter,
                }
            });

            return reservations;
        }

    }
}