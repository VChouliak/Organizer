using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Events
{
    public class AfterDetailDeletedEventArgs : EventArgs
    {
        public AfterDetailDeletedEventArgs()
        {
            
        }

        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
