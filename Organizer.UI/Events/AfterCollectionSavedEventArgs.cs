using System;

namespace Organizer.UI.Events
{
    public class AfterCollectionSavedEventArgs : EventArgs
    {
        public string VieModelName { get; set; }
    }
}
