﻿using System.ComponentModel.DataAnnotations;

namespace Organizer.Core.Models
{
    public class Friend
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]    
        public string Email { get; set; }
    }
}
