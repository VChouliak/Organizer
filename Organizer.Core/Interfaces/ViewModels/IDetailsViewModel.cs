using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.ViewModels
{
    public interface IDetailsViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges {  get; }
    }
}
