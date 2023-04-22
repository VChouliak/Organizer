using Organizer.Core.Models.Entities;

namespace Organizer.UI.Wrapper
{
    public class ProgrammingLanguageWrapper : ModelWrapper<ProgrammingLanguage>
    {
        public ProgrammingLanguageWrapper(ProgrammingLanguage model) : base(model)
        {
        }

        public int Id { get => Model.Id; }
        public string Name
        {
            get {  return GetValue<string>(); }
            set { SetValue(value);}
        }
    }
}
