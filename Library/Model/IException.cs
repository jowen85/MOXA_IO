using System;
using System.ComponentModel;

namespace Library.Model
{
    public interface IException
    {
        #region PropertyChanged Event Properties

        Exception FatalException { get; set; }
        Exception NonFatalException { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
