using LabManAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabManAPI.Services
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetReservationsAsync();

        Task<Reservation> GetRservationByIdAsync(int reservationId);

        Task<bool> UpdateReservationAsync(Reservation reservationToUpdate);

        Task<bool> DeleteReservationAsync(int reservationId);

        Task<bool> CreateReservationAsync(Reservation reservation);
    }
}
