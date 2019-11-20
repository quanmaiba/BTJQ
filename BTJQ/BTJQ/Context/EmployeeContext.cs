using BTJQ.Models;
using BTJQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BTJQ.Context
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }
        public DbSet<tblSkill> tblSkills { get; set; }
        public DbSet<tblEmployee> tblEmployees { get; set; }
        public DbSet<EmployeeViewModel> EmployeeViewModel { get; set; }
        public DbSet<EmployeeCreateModel> EmployeeCreateModel { get; set; }
        public DbSet<EmployeeEditModel> EmployeeEditModel { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
    }
}
