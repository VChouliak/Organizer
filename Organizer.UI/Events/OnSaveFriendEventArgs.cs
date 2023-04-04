using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Events
{
    public class OnSaveFriendEventArgs : EventArgs
    {
        public OnSaveFriendEventArgs(int id, string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;
        }

        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
