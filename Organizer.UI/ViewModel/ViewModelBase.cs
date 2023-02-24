using Organizer.UI.Event;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Organizer.UI.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));            
        }
    }
}
