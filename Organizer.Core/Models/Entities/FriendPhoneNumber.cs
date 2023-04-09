using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Models.Entities
{
    public class FriendPhoneNumber : BaseModel
    {
        [Phone]
        [Required]
        public string Number { get; set; }
        public int FriendId { get; set; }
        public Friend Friend { get; set; }
    }
}
