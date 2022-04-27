using System;
using System.Collections.Generic;
using System.IO;

using R5T.Lombardy;

using R5T.T0064;


namespace R5T.Gepidia.Local
{
    [ServiceImplementationMarker]
    public class LocalFileSystemOperator : ILocalFileSystemOperator, IServiceImplementation
    {
        private IStringlyTypedPathOperator StringlyTypedPathOperator { get; }


        public LocalFileSystemOperator(IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;
        }

        public void ChangePermissions(string path, short mode)
        {
            LocalFileSystem.ChangePermissions(path, mode);
        }

        public void Copy(Stream source, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.Copy(source, destinationFilePath, overwrite);
        }

        public void Copy(string sourceFilePath, Stream destination)
        {
            LocalFileSystem.Copy(sourceFilePath, destination);
        }

        public void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            LocalFileSystem.CopyDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }

        public void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.CopyFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public void CreateDirectory(string directoryPath)
        {
            LocalFileSystem.CreateDirectory(directoryPath);
        }

        public Stream CreateFile(string filePath, bool overwrite = true)
        {
            var output = LocalFileSystem.CreateFile(filePath, overwrite);
            return output;
        }

        public void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            LocalFileSystem.DeleteDirectory(directoryPath, recursive);
        }

        public void DeleteFile(string filePath)
        {
            LocalFileSystem.DeleteFile(filePath);
        }

        public IEnumerable<string> EnumerateDirectories(string directoryPath)
        {
            var output = LocalFileSystem.EnumerateDirectories(directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            var output = LocalFileSystem.EnumerateFiles(directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false)
        {
            var output = LocalFileSystem.EnumerateFileSystemEntryPaths(this.StringlyTypedPathOperator, directoryPath, recursive);
            return output;
        }

        public IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var output = LocalFileSystem.EnumerateFileSystemEntries(this.StringlyTypedPathOperator, directoryPath, recursive);
            return output;
        }

        public bool ExistsDirectory(string directoryPath)
        {
            var output = LocalFileSystem.ExistsDirectory(directoryPath);
            return output;
        }

        public bool ExistsFile(string filePath)
        {
            var output = LocalFileSystem.ExistsFile(filePath);
            return output;
        }

        public FileSystemEntryType GetFileSystemEntryType(string path)
        {
            var output = LocalFileSystem.GetFileSystemEntryType(path);
            return output;
        }

        public DateTime GetDirectoryLastModifiedTimeUTC(string path)
        {
            var output = LocalFileSystem.GetDirectoryLastModifiedTimeUTC(path);
            return output;
        }

        public DateTime GetFileLastModifiedTimeUTC(string path)
        {
            var output = LocalFileSystem.GetFileLastModifiedTimeUTC(path);
            return output;
        }

        public void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            LocalFileSystem.MoveDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.MoveFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public Stream OpenFile(string filePath)
        {
            var output = LocalFileSystem.OpenFile(filePath);
            return output;
        }

        public Stream ReadFile(string filePath)
        {
            var output = LocalFileSystem.ReadFile(filePath);
            return output;
        }
    }
}
