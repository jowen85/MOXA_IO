using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Library.Model
{
    /// <summary>
    /// The remote file system and its properties. Implements INotifyPropertyChanged such that the view can be updated of changed properties.
    /// </summary>
    public class FileSystemModel : ModelBase
    {

        public FileSystemModel()
        {
            RemoteFilesList = new List<string>();
        }

        #region PropertyChanged Event Properties

        private List<string> _remoteFilesList;
        public List<string> RemoteFilesList
        {
            get => _remoteFilesList;
            set
            {
                _remoteFilesList = value;
                FirePropertyChanged();
            }
        }

        #endregion



    }
}
