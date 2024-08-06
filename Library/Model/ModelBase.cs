using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.Model
{
    public abstract class ModelBase : INotifyPropertyChanged, IException
    {
        private event PropertyChangedEventHandler ModelPropertyChanged;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => ModelPropertyChanged += value;
            remove => ModelPropertyChanged -= value;
        }

        internal void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            ModelPropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private Exception _fatalException;
        public Exception FatalException
        {
            get => _fatalException;
            set
            {
                _fatalException = value;
                FirePropertyChanged();
            }
        }

        private Exception _nonFatalException;
        public Exception NonFatalException
        {
            get => _nonFatalException;
            set
            {
                _nonFatalException = value;
                FirePropertyChanged();
            }
        }
    }
}