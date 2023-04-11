using Organizer.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data.Specifications
{
    public class MeetingsIncludeFriendsSpecification : BaseSpecification<Meeting>
    {
        public MeetingsIncludeFriendsSpecification() : base()
        {
            AddInclude(meeting => meeting.Friends);
        }

        public MeetingsIncludeFriendsSpecification(int? id) : base(x => x.Id == id)
        {
            AddInclude(meeting => meeting.Friends);
        }
    }
}
