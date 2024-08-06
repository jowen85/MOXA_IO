using System;
using System.Linq;
using System.Text.RegularExpressions;
using PolarisCommunication;
using System.IO;
using System.Configuration;
using PolarisCommunication.CustomExceptions;
using PolarisDemonstration.Model;

namespace PolarisDemonstration.Controller
{
    /// <summary>
    /// Sets up methods for managing files on the remote Polaris System.
    /// </summary>
    public class FileSystemController
    {
        private IFileSystem _fileSystem;
        private readonly FileSystemModel _fileSystemModel;

        private readonly string _gCodeDirectory = ConfigurationManager.AppSettings["GCodeDirectory"];

        public FileSystemController(FileSystemModel fileSystemModel)
        {
            _fileSystemModel = fileSystemModel;
        }

        /// <summary>
        /// Connects the underlying FileSystem on the Polaris System.
        /// </summary>
        public void ConnectFileSystem(string ipAddress)
        {
            try
            {
                _fileSystem = FileSystem.GetInstance();
                _fileSystem.Connect(ipAddress);

                // Assign the event handlers for the FileSystem
                _fileSystem.FileListChanged += FileListChange;
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException || e is FormatException || e is PolarisServerUnreachableException)
                {
                    _fileSystemModel.FatalException = new FormatException("An invalid IP address (" + ipAddress + ") was specified in the configuration file app.config. Please correct and try again.");
                }
                else
                {
                    _fileSystemModel.FatalException = e;
                }
            }
        }

        /// <summary>
        /// Updates the list of files in our GCode directory, for use with the GCode process.
        /// </summary>
        internal void UpdateRemoteFilesList()
        {
            CreateGCodeFileList(); 
        }

        /// <summary>
        /// Updates the RemoteFilesList property of the RemoteFileSystemModel based on files found on the remote Polaris system.
        /// </summary>
        private void CreateGCodeFileList()
        {
            // Remove any files that don't have the proper extensions
            var gCodeFileList = _fileSystem.GetSortedRemoteFileList(_gCodeDirectory, new Regex(@".gc")).Select(fileInfo => fileInfo.FileName).ToList();   

            // Replace our list with the new one, forcing the PropertyChanged event to fire in our Model class.
            _fileSystemModel.RemoteFilesList = gCodeFileList;
        }

        /// <summary>
        /// Uploads the chosen file to the remote Polaris server.
        /// </summary>
        /// <param name="localFileName"> Filename of the local file. </param>
        /// <param name="remoteFileName"> Filename of the remote file. </param>
        internal void UploadFileToPolarisSystem(string localFileName, string remoteFileName)
        {
            try
            {
                _fileSystem.RemoteDirectory = _gCodeDirectory;
                _fileSystem.RemoteFile = remoteFileName;
                _fileSystem.UploadFile(localFileName);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is InvalidOperationException)
                {
                    _fileSystemModel.NonFatalException = e;
                }
                else { throw; }
            }
        }

        /// <summary>
        /// Handles the file list change event. 
        /// Used by the delete and upload file events to refresh the remote file list following completion.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PolarisCommunication.FileListChangedEventArgs"/> instance containing the event data</param>
        private void FileListChange(object sender, FileListChangedEventArgs e)
        {
            // Ensure the FileList that was changed is in fact from the directory we are concerned with in this application
            if (e.Directory.Equals(_gCodeDirectory))
            {
                CreateGCodeFileList();
            }
        }

        /// <summary>
        /// Disconnects the underlying FileSystem.
        /// </summary>
        internal void DisconnectFileSystem()
        {
            _fileSystem?.Disconnect();
        }

        public FileSystemModel RemoteFileSystem => _fileSystemModel;
    }
}
