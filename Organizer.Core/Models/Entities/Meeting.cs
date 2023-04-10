using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Organizer.Core.Models.Entities
{
    public class Meeting : BaseModel
    {
        public Meeting()
        {
            Friends = new Collection<Friend>();
        }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ICollection<Friend> Friends{ get; set; }
    }
}
