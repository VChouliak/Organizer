using System.ComponentModel.DataAnnotations;

namespace Organizer.Core.Models.Entities
{
    public class Friend : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
