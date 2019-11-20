using System;

namespace BTJQ.Models.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public bool Sex { get; set; }

        public int ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; }
    }
}
