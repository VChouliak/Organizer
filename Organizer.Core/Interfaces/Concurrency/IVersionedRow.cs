using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Organizer.Core.Interfaces.Concurrency
{
    public interface IVersionedRow
    {      
        public Guid Version { get; set; }
    }
}
