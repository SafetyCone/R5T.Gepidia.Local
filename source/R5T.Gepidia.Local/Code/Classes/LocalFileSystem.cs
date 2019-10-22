using System;
using System.Collections.Generic;
using System.IO;

using R5T.Magyar;


namespace R5T.Gepidia.Local
{
    public static class LocalFileSystem
    {
        public static void ChangePermissions(string path, short mode)
        {
            // NEED.
            throw new NotImplementedException();
        }

        public static void Copy(Stream source, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.CheckOverwrite(destinationFilePath, overwrite);

            using (var destination = File.OpenWrite(destinationFilePath))
            {
                source.CopyTo(destination);
            }
        }

        public static void Copy(string sourceFilePath, Stream destination)
        {
            using (var source = File.OpenRead(sourceFilePath))
            {
                source.CopyTo(destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Adapted from: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        /// </remarks>
        public static void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            // Get the subdirectories for the specified directory.
            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);

            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found:\n{sourceDirectoryPath}");
            }

            var sourceSubDirectories = sourceDirectory.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationDirectoryPath))
            {
                Directory.CreateDirectory(destinationDirectoryPath);
            }

            // Get the files in the directory and copy them to the new location.
            var files = sourceDirectory.GetFiles();
            foreach (var file in files)
            {
                string destinationFilePath = Path.Combine(destinationDirectoryPath, file.Name);
                file.CopyTo(destinationFilePath, true); // Overwrite files.
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (var sourceSubDirectory in sourceSubDirectories)
            {
                string destinationSubDirectoryPath = Path.Combine(destinationDirectoryPath, sourceSubDirectory.Name);
                LocalFileSystem.CopyDirectory(sourceSubDirectory.FullName, destinationSubDirectoryPath);
            }
        }

        public static void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.CheckOverwrite(destinationFilePath, overwrite);

            File.Copy(sourceFilePath, destinationFilePath, true);
        }

        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath); // Idempotent. No exception thrown.
        }

        public static void CreateDirectoryOnlyIfNotExists(string directoryPath)
        {
            LocalFileSystem.CreateDirectory(directoryPath);
        }

        public static Stream CreateFile(string filePath, bool overwrite = true)
        {
            LocalFileSystem.CheckOverwrite(filePath, overwrite);

            var output = File.Create(filePath);
            return output;
        }

        public static void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            if(!Directory.Exists(directoryPath))
            {
                return;
            }

            Directory.Delete(directoryPath, recursive); // Not idempotent.
        }

        public static void DeleteDirectoryOnlyIfExists(string directoryPath, bool recursive = true)
        {
            LocalFileSystem.DeleteDirectory(directoryPath, recursive); // Idempotent, ok.
        }

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath); // Idempotent, ok.
        }

        public static void DeleteFileOnlyIfExists(string filePath)
        {
            File.Delete(filePath); // Idempotent, ok.
        }

        public static IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false)
        {
            var searchOption = SearchOptionHelper.RecursiveToSearchOption(recursive);

            var output = Directory.EnumerateFileSystemEntries(directoryPath, SearchPatternHelper.All, searchOption);
            return output;
        }

        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var searchOption = SearchOptionHelper.RecursiveToSearchOption(recursive);

            // Ok to query the local file system multiple times, should be quick enough.
            var fileSystemEntryPaths = LocalFileSystem.EnumerateFileSystemEntryPaths(directoryPath, recursive);
            foreach (var path in fileSystemEntryPaths)
            {
                var type = LocalFileSystem.GetFileSystemEntryType(path);
                var lastModifiedUTC = LocalFileSystem.GetFileLastModifiedTimeUTC(path);

                var entry = FileSystemEntry.New(path, type, lastModifiedUTC);
                yield return entry;
            }
        }

        public static IEnumerable<string> EnumerateDirectories(string directoryPath)
        {
            var output = Directory.EnumerateDirectories(directoryPath, SearchPatternHelper.All, SearchOption.TopDirectoryOnly);
            return output;
        }

        public static IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            var output = Directory.EnumerateFiles(directoryPath, SearchPatternHelper.All, SearchOption.TopDirectoryOnly);
            return output;
        }

        public static bool ExistsDirectory(string directoryPath)
        {
            var output = Directory.Exists(directoryPath);
            return output;
        }

        public static bool ExistsFile(string filePath)
        {
            var output = File.Exists(filePath);
            return output;
        }

        /// <summary>
        /// Determines if a directory exists at the location specified by the path.
        /// Directory must exist, and the location must be a directory (not a file with the same path).
        /// To determine if a path itself is *directory indicated*, use a stringly-typed path operator.
        /// </summary>
        public static bool IsDirectory(string path)
        {
            var output = Directory.Exists(path); // Ask if the directory exists. The key is that if a file exists at the path, then a directory does not.
            return output;
        }

        /// <summary>
        /// Determines if a file exists at the location specified by the path.
        /// File must exist, and the location must be a file (not a directory with the same path).
        /// To determine if a path itself is *file indicated*, use a stringly-typed path operator.
        /// </summary>
        public static bool IsFile(string path)
        {
            var output = File.Exists(path); // Ask if the file exists. The key is that if a directory exists at the path, then a file does not.
            return output;
        }

        public static FileSystemEntryType GetFileSystemEntryType(string path)
        {
            var isDirectory = LocalFileSystem.IsDirectory(path);
            if(isDirectory)
            {
                return FileSystemEntryType.Directory;
            }
            else
            {
                return FileSystemEntryType.File;
            }
        }

        public static DateTime GetDirectoryLastModifiedTimeUTC(string path)
        {
            var output = Directory.GetLastWriteTimeUtc(path);
            return output;
        }

        public static DateTime GetFileLastModifiedTimeUTC(string path)
        {
            var output = File.GetLastWriteTimeUtc(path);
            return output;
        }

        public static DateTime GetFileLastModifiedTime(string path)
        {
            var isDirectory = LocalFileSystem.IsDirectory(path);
            if(isDirectory)
            {
                var output = LocalFileSystem.GetDirectoryLastModifiedTimeUTC(path);
                return output;
            }
            else
            {
                var output = LocalFileSystem.GetFileLastModifiedTimeUTC(path);
                return output;
            }
        }

        public static void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            Directory.Move(sourceDirectoryPath, destinationDirectoryPath);
        }

        public static void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            LocalFileSystem.CheckOverwrite(destinationFilePath, overwrite);

            File.Move(sourceFilePath, destinationFilePath);
        }

        public static Stream OpenFile(string filePath)
        {
            var output = File.OpenWrite(filePath);
            return output;
        }

        public static Stream ReadFile(string filePath)
        {
            var output = File.OpenRead(filePath);
            return output;
        }


        #region Miscellaneous

        public static void CheckOverwrite(string filePath, bool overwrite)
        {
            if (!overwrite && File.Exists(filePath))
            {
                var exception = CommonFileSystem.GetCannotOverwriteFileIOException(filePath);
                throw exception;
            }
        }

        #endregion
    }
}
