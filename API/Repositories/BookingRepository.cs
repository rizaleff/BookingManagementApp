using API.Contracts;
using API.Data;
using API.Models;
using System;

namespace API.Repositories;

public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
{
    public BookingRepository(BookingManagementDbContext context) : base(context) { }

    public IEnumerable<Booking>? GetBookedToday()
    {
        var bookingToday = _context.Set<Booking>()
            .Where(b => b.StartDate.Date <= DateTime.Today && b.EndDate.Date >= DateTime.Today)
            .ToList();
        return bookingToday;
    }
}
