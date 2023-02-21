using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Core.Specifications
{
    public class FriendsOrderedByFirstNameSpecification : BaseSpecification<Friend>
    {
        public FriendsOrderedByFirstNameSpecification() : base()
        {
            AddOrderBy(friend => friend.FirstName);
        }

        public FriendsOrderedByFirstNameSpecification(int id) : base(x => x.Id == id)
        {
        }
    }
}
