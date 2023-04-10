using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Interfaces.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges {  get; }
    }
}
