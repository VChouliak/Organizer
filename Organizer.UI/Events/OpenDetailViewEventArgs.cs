using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Events
{
    public class OpenDetailViewEventArgs : EventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
