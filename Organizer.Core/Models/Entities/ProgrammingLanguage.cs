using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Models.Entities
{
    public class ProgrammingLanguage : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
