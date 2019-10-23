using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using R5T.Lombardy;
using R5T.Lombardy.Gepidia.Extensions;
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

        /// <summary>
        /// Uses <see cref="Directory.EnumerateFileSystemEntries(string, string, SearchOption)"/> to get directory file-system entries.
        /// This method does not return directory-indicated directory paths.
        /// </summary>
        /// <remarks>
        /// This method involves just one call to the file-system via the <see cref="Directory"/> API.
        /// </remarks>
        public static IEnumerable<string> EnumerateFileSystemEntryPathsSimple(string directoryPath, bool recursive = false)
        {
            var searchOption = SearchOptionHelper.RecursiveToSearchOption(recursive);

            var enumeratedPaths = Directory.EnumerateFileSystemEntries(directoryPath, SearchPatternHelper.All, searchOption);
            return enumeratedPaths;
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// </summary>
        /// <remarks>
        /// This method involves two calls to the file-system via <see cref="Directory.EnumerateDirectories(string, string, SearchOption)"/> and <see cref="Directory.EnumerateFiles(string, string, SearchOption)"/>.
        /// </remarks>
        public static IEnumerable<string> EnumerateFileSystemEntryPaths(IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            var directoryExists = stringlyTypedPathOperator.ExistsDirectoryPath(directoryPath);
            if(!directoryExists)
            {
                return Enumerable.Empty<string>();
            }

            var searchOption = SearchOptionHelper.RecursiveToSearchOption(recursive);

            var subDirectoryPaths = Directory.EnumerateDirectories(directoryPath, SearchPatternHelper.All, SearchOption.AllDirectories);
            var filePaths = Directory.EnumerateFiles(directoryPath, SearchPatternHelper.All, SearchOption.AllDirectories);

            var allPaths = new List<string>();
            foreach (var subDirectoryPath in subDirectoryPaths)
            {
                // Directory paths are NOT directory-indicated coming from the Directory.EnumerateDirectories() API.
                var outputDirectoryPath = stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(subDirectoryPath);

                allPaths.Add(outputDirectoryPath);
            }

            // File paths ARE file-indicated (not directory-indicated) from from the Directory.EnumerateFiles() API.
            allPaths.AddRange(filePaths);

            // Sort file paths to return them in order.
            allPaths.Sort();

            return allPaths;
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-system entries in sorted order.
        /// </summary>
        /// <remarks>
        /// This method involves two calls to the file-system via <see cref="LocalFileSystem.EnumerateFileSystemEntryPaths(IStringlyTypedPathOperator, string, bool)"/>, then for N directories and M files, N + M calls to the file-system to get the last-modified time for each entry.
        /// The N + M calls to determine if the entry is a directory or file is avoided by relying on <see cref="LocalFileSystem.EnumerateFileSystemEntryPaths(IStringlyTypedPathOperator, string, bool)"/> to provided directory-indicated paths.
        /// </remarks>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            var fileSystemEntryPaths = LocalFileSystem.EnumerateFileSystemEntryPaths(stringlyTypedPathOperator, directoryPath, recursive);
            foreach (var entryPath in fileSystemEntryPaths)
            {
                var entryType = stringlyTypedPathOperator.GetFileSystemEntryType(entryPath);
                var lastModifiedUTC = LocalFileSystem.GetFileLastModifiedTimeUTC(entryPath); // Ok to query the local file system multiple times, should be quick enough.

                var entry = FileSystemEntry.New(entryPath, entryType, lastModifiedUTC);
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
