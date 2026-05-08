using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Data
{
    public static class InMemoryDataStore
    {
        public static List<Room> Rooms { get; set; } = new List<Room>
        {
            new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Aula Magna", BuildingCode = "A", Floor = 0, Capacity = 100, HasProjector = true, IsActive = true },
            new Room { Id = 3, Name = "Sala 202", BuildingCode = "B", Floor = 2, Capacity = 20, HasProjector = false, IsActive = true },
            new Room { Id = 4, Name = "Sala 305", BuildingCode = "C", Floor = 3, Capacity = 10, HasProjector = true, IsActive = false },
            new Room { Id = 5, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 30, HasProjector = true, IsActive = true }
        };

        public static List<Reservation> Reservations { get; set; } = new List<Reservation>
        {
            new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Szkolenie BHP", Date = new DateOnly(2026, 5, 10), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0), Status = "confirmed" },
            new Reservation { Id = 2, RoomId = 2, OrganizerName = "Anna Nowak", Topic = "Wykład gościnny", Date = new DateOnly(2026, 5, 10), StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0), Status = "planned" },
            new Reservation { Id = 3, RoomId = 3, OrganizerName = "Piotr Wiśniewski", Topic = "Konsultacje", Date = new DateOnly(2026, 5, 11), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(11, 0, 0), Status = "confirmed" },
            new Reservation { Id = 4, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Szkolenie PPOŻ", Date = new DateOnly(2026, 5, 12), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = "cancelled" }
        };
    }
}