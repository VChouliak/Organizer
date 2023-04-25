using Organizer.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data.Specifications
{
    public class FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification : BaseSpecification<Friend>
    {
        public FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification() : base()
        {
            AddOrderBy(friend => friend.FirstName);
            AddInclude(x => x.PhoneNumbers);
            AddInclude(x => x.Meetings);
        }

        public FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification(int? id) : base(x => x.Id == id)
        {
            AddInclude(x => x.PhoneNumbers);
            AddInclude(x => x.Meetings);
        }
    }
}
