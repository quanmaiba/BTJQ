using System.ComponentModel.DataAnnotations;

namespace BTJQ.Models
{
    public class SkillModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
