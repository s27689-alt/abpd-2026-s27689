using Microsoft.AspNetCore.Mvc;
using TrainingCenterAPI.Data;
using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var reservations = InMemoryDataStore.Reservations.AsQueryable();

            if (date.HasValue)
                reservations = reservations.Where(r => r.Date == date.Value);
            
            if (!string.IsNullOrEmpty(status))
                reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            
            if (roomId.HasValue)
                reservations = reservations.Where(r => r.RoomId == roomId.Value);

            return Ok(reservations.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound($"Reservation with ID {id} not found.");
            
            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null) return BadRequest("The specified room does not exist.");
            if (!room.IsActive) return BadRequest("Cannot reserve an inactive room.");

            bool isConflict = InMemoryDataStore.Reservations.Any(r => 
                r.RoomId == reservation.RoomId && 
                r.Date == reservation.Date && 
                r.StartTime < reservation.EndTime && 
                r.EndTime > reservation.StartTime);

            if (isConflict) return Conflict("This reservation overlaps with an existing one for the same room and date.");

            reservation.Id = InMemoryDataStore.Reservations.Any() ? InMemoryDataStore.Reservations.Max(r => r.Id) + 1 : 1;
            InMemoryDataStore.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            var existingReservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (existingReservation == null) return NotFound($"Reservation with ID {id} not found.");

            var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
            if (room == null) return BadRequest("The specified room does not exist.");
            if (!room.IsActive) return BadRequest("Cannot reserve an inactive room.");

            bool isConflict = InMemoryDataStore.Reservations.Any(r => 
                r.Id != id &&
                r.RoomId == updatedReservation.RoomId && 
                r.Date == updatedReservation.Date && 
                r.StartTime < updatedReservation.EndTime && 
                r.EndTime > updatedReservation.StartTime);

            if (isConflict) return Conflict("This update creates a time conflict with an existing reservation.");

            existingReservation.RoomId = updatedReservation.RoomId;
            existingReservation.OrganizerName = updatedReservation.OrganizerName;
            existingReservation.Topic = updatedReservation.Topic;
            existingReservation.Date = updatedReservation.Date;
            existingReservation.StartTime = updatedReservation.StartTime;
            existingReservation.EndTime = updatedReservation.EndTime;
            existingReservation.Status = updatedReservation.Status;

            return Ok(existingReservation);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound($"Reservation with ID {id} not found.");

            InMemoryDataStore.Reservations.Remove(reservation);
            return NoContent();
        }
    }
}