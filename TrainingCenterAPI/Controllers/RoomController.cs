using Microsoft.AspNetCore.Mvc;
using TrainingCenterAPI.Data;
using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var rooms = InMemoryDataStore.Rooms.AsQueryable();

            if (minCapacity.HasValue)
                rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
            
            if (hasProjector.HasValue)
                rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
            
            if (activeOnly.HasValue && activeOnly.Value)
                rooms = rooms.Where(r => r.IsActive);

            return Ok(rooms.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with ID {id} not found.");
            
            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public IActionResult GetRoomsByBuilding(string buildingCode)
        {
            var rooms = InMemoryDataStore.Rooms
                .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(rooms);
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] Room room)
        {
            room.Id = InMemoryDataStore.Rooms.Any() ? InMemoryDataStore.Rooms.Max(r => r.Id) + 1 : 1;
            InMemoryDataStore.Rooms.Add(room);

            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var existingRoom = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (existingRoom == null) return NotFound($"Room with ID {id} not found.");

            existingRoom.Name = updatedRoom.Name;
            existingRoom.BuildingCode = updatedRoom.BuildingCode;
            existingRoom.Floor = updatedRoom.Floor;
            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.HasProjector = updatedRoom.HasProjector;
            existingRoom.IsActive = updatedRoom.IsActive;

            return Ok(existingRoom);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with ID {id} not found.");

            bool hasReservations = InMemoryDataStore.Reservations.Any(r => r.RoomId == id);
            if (hasReservations)
            {
                return Conflict("Cannot delete a room that has associated reservations.");
            }

            InMemoryDataStore.Rooms.Remove(room);
            return NoContent();
        }
    }
}