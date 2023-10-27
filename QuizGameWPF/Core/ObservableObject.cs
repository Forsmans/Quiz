using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizGameWPF.Core
{
    //Notifying when a property changes in an observable object
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
