using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Event
{
    public class LookupViewEventArgs : EventArgs
    {
        public LookupViewEventArgs(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
