using LabManAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabManAPI.Contracts.Responses;
using System;

namespace LabManAPI.Services
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetReservationsAsync();

        Task<Reservation> GetRservationByIdAsync(int reservationId);

        Task<bool> UpdateReservationAsync(Reservation reservationToUpdate);

        Task<bool> DeleteReservationAsync(int reservationId);

        Task<bool> CreateReservationAsync(Reservation reservation);

        Task<List<ReservationsFromDateResponse>> GetReservationsWithCorrespondingDate(DateTime startRange, DateTime endRange);

    }
}
