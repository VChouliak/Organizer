using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Events
{
    public class OnDeleteFriendEventArgs : EventArgs
    {
        public OnDeleteFriendEventArgs(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
