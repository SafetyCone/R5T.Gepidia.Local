using System;
using System.Collections.Generic;
using System.IO;


namespace R5T.Gepidia.Local
{
    public class FileSystemOperator : IFileSystemOperator
    {
        private ILocalFileSystemOperator LocalFileSystemOperator { get; }


        public FileSystemOperator(ILocalFileSystemOperator localFileSystemOperator)
        {
            this.LocalFileSystemOperator = localFileSystemOperator;
        }

        public bool ExistsFile(string filePath)
        {
            var output = this.LocalFileSystemOperator.ExistsFile(filePath);
            return output;
        }

        public bool ExistsDirectory(string directoryPath)
        {
            var output = this.LocalFileSystemOperator.ExistsDirectory(directoryPath);
            return output;
        }

        public FileSystemEntryType GetFileSystemEntryType(string path)
        {
            var output = this.LocalFileSystemOperator.GetFileSystemEntryType(path);
            return output;
        }

        public void DeleteFile(string filePath)
        {
            this.LocalFileSystemOperator.DeleteFile(filePath);
        }

        public void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            this.LocalFileSystemOperator.DeleteDirectory(directoryPath, recursive);
        }

        public Stream CreateFile(string filePath, bool overwrite = true)
        {
            var output = this.LocalFileSystemOperator.CreateFile(filePath, overwrite);
            return output;
        }

        public Stream OpenFile(string filePath)
        {
            var output = this.LocalFileSystemOperator.OpenFile(filePath);
            return output;
        }

        public Stream ReadFile(string filePath)
        {
            var output = this.LocalFileSystemOperator.ReadFile(filePath);
            return output;
        }

        public void CreateDirectory(string directoryPath)
        {
            this.LocalFileSystemOperator.CreateDirectory(directoryPath);
        }

        public IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false)
        {
            var output = this.LocalFileSystemOperator.EnumerateFileSystemEntryPaths(directoryPath, recursive);
            return output;
        }

        public IEnumerable<string> EnumerateDirectories(string directoryPath)
        {
            var output = this.LocalFileSystemOperator.EnumerateDirectories(directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            var output = this.LocalFileSystemOperator.EnumerateFiles(directoryPath);
            return output;
        }

        public IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var output = this.LocalFileSystemOperator.EnumerateFileSystemEntries(directoryPath, recursive);
            return output;
        }

        public DateTime GetDirectoryLastModifiedTimeUTC(string directoryPath)
        {
            var output = this.LocalFileSystemOperator.GetDirectoryLastModifiedTimeUTC(directoryPath);
            return output;
        }

        public DateTime GetFileLastModifiedTimeUTC(string filePath)
        {
            var output = this.LocalFileSystemOperator.GetFileLastModifiedTimeUTC(filePath);
            return output;
        }

        public void ChangePermissions(string path, short mode)
        {
            this.LocalFileSystemOperator.ChangePermissions(path, mode);
        }

        public void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            this.LocalFileSystemOperator.CopyFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            this.LocalFileSystemOperator.CopyDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }

        public void Copy(Stream source, string destinationFilePath, bool overwrite = true)
        {
            this.LocalFileSystemOperator.Copy(source, destinationFilePath, overwrite);
        }

        public void Copy(string sourceFilePath, Stream destination)
        {
            this.LocalFileSystemOperator.Copy(sourceFilePath, destination);
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            this.LocalFileSystemOperator.MoveFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            this.LocalFileSystemOperator.MoveDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }
    }
}
