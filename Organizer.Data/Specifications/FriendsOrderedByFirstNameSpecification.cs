using Organizer.Core.Interfaces.Specification;
using Organizer.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data.Specifications
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
