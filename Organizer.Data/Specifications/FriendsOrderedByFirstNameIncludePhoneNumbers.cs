using Organizer.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data.Specifications
{
    public class FriendsOrderedByFirstNameIncludePhoneNumbers : BaseSpecification<Friend>
    {
        public FriendsOrderedByFirstNameIncludePhoneNumbers() : base()
        {
            AddOrderBy(friend => friend.FirstName);
            AddInclude(x=>x.PhoneNumbers);
        }

        public FriendsOrderedByFirstNameIncludePhoneNumbers(int? id) : base(x => x.Id == id)
        {
            AddInclude(x => x.PhoneNumbers);
        }
    }
}
