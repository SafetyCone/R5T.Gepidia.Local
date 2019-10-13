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

        public static Stream CreateFile(string filePath, bool overwrite = true)
        {
            LocalFileSystem.CheckOverwrite(filePath, overwrite);

            var output = File.Create(filePath);
            return output;
        }

        public static void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            Directory.Delete(directoryPath, recursive);
        }

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public static IEnumerable<string> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var searchOption = SearchOptionHelper.RecursiveToSearchOption(recursive);

            var output = Directory.EnumerateFileSystemEntries(directoryPath, SearchPatternHelper.All, searchOption);
            return output;
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

        public static void MoveDirectory(string sourceDirectoryPath, string destinationFilePath)
        {
            Directory.Move(sourceDirectoryPath, destinationFilePath);
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
