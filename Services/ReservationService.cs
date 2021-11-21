using LabManAPI.Data;
using LabManAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


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

    }
}