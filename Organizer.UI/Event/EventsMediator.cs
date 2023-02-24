using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Event
{
    // TODO: Replace with EventAggregator Pattern
    public sealed class EventsMediator
    {
        private static readonly EventsMediator _instance = new EventsMediator();
        private EventsMediator()
        {
            
        }
        public static EventsMediator Instance { get {  return _instance; } }

        public event EventHandler<LookupViewEventArgs> PropertyChanged;
        
        public void OnPropertyChanged(object sender, LookupViewEventArgs args)
        {
            var propertyChangeDelegate = PropertyChanged as EventHandler<LookupViewEventArgs>;
            if (propertyChangeDelegate != null)
            {              
                propertyChangeDelegate(sender, args);
            }
        }
    }
}
