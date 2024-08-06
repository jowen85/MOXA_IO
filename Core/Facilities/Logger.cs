using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Core.Variables;

namespace Core.Facilities
{
    public class Logger : ILogger
    {
        #region Variables
        private readonly string FilePath = @GlobalVar.systemCFG.FileLogger.FilePath;

        Dictionary<int, FileInfo> Filedetails;

        Queue<FileInfo> FileData;

        public struct FileInfo
        {
            public string FolderName;
            public string FileName;
            public string Header;
            public string Extension;
            public string Message;
        }

        Thread ClearQueue;

        readonly object m_Lock = new object();
        readonly object Lock = new object();

        FileInfo _FileInfo;
        FileInfo _FileLogInfo;

        #endregion Variables

        #region Constructor

        public Logger()
        {
            Filedetails = new Dictionary<int, FileInfo>();
            FileData = new Queue<FileInfo>();
            _FileInfo = new FileInfo();
            _FileLogInfo = new FileInfo();

            LoadFileLocation();

            ClearQueue = new Thread(QueueData);
            ClearQueue.Start();
        }

        #endregion Constructor

        #region Threading

        void QueueData()
        {
            while (true)
            {
                lock (m_Lock)
                {
                    if (FileData.Count > 0)
                    {
                        _FileInfo = FileData.Dequeue();
                        Log(_FileInfo.FolderName, _FileInfo.FileName, _FileInfo.Header, _FileInfo.Extension, _FileInfo.Message);
                    }
                }

                Thread.Sleep(1);
            }
        }

        #endregion

        #region Method

        public void LogData(string Message, int FileList)
        {
            lock (Lock)
            {
                _FileLogInfo = Filedetails[FileList];
                _FileLogInfo.Message = Message;
                FileData.Enqueue(_FileLogInfo);
            }
        }

        void LoadFileLocation()
        {
            for (int i = 0; i < GlobalVar.systemCFG.FileLoggerFiles.Count; i++)
            {
                Filedetails.Add(i, new FileInfo
                {
                    FolderName = GlobalVar.systemCFG.FileLoggerFiles[i].FolderName,
                    FileName = GlobalVar.systemCFG.FileLoggerFiles[i].FileName,
                    Header = GlobalVar.systemCFG.FileLoggerFiles[i].Header.Replace('|', ','),
                    Extension = GlobalVar.systemCFG.FileLoggerFiles[i].FileExtension
                });
            }
        }

        private void Log(string Folder, string FileName, string Header, string extension, string Message)
        {
            string strPath = FilePath + Path.DirectorySeparatorChar + Folder;
            string Filename = FileName + DateTime.Now.ToString("_yyyy_MM_dd") + extension;

            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + Filename))
            {
                using (FileStream stream = new FileStream(strPath + Path.DirectorySeparatorChar + Filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Date" + "," + "Time" + "," + Header);
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MMM-dd") + "," + DateTime.Now.ToString("HH:mm:ss") + "," + Message);
                }
            }
            else
            {
                using (FileStream stream = new FileStream(strPath + Path.DirectorySeparatorChar + Filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MMM-dd") + "," + DateTime.Now.ToString("HH:mm:ss") + "," + Message);
                }
            }
        }

        public void Dispose()
        {
            ClearQueue.Abort();
        }

        #endregion
    }
}
