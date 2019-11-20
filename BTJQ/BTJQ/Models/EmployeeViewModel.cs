using System.ComponentModel.DataAnnotations;

namespace BTJQ.Models
{
    public class EmployeeViewModel
    {
        [Key]
        [Display(Name = "#ID")]
        public int EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Skill")]
        public string Skill { get; set; }
        [Display(Name = "Years Experience")]
        public int YearsExperience { get; set; }
    }
}
