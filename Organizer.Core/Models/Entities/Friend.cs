using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Organizer.Core.Models.Entities
{
    public class Friend : BaseModel
    {
        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
        }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }      
        public int ProgrammingLanguageId { get; set; }      
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
    }
}
