using Organizer.Core.Models.Entities;
using Organizer.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Organizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {

        }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int ProgrammingLanguageId
        {
            get
            {
                return GetValue<int>();
            }
            set
            {
                SetValue(value);
            }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, String.Empty, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Firstname is required";
                    }
                    break;

                case nameof(Email):
                    string regexEmail = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|de)$";
                    if (!Regex.IsMatch(Email, regexEmail, RegexOptions.IgnoreCase))
                    {
                        yield return "Invalid Email Format"; // TODO: adjust message
                    }
                    break;
            }
        }
    }
}
