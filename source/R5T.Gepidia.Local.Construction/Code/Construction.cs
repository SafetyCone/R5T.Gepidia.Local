using System;
using System.IO;

using R5T.Magyar;
using R5T.Magyar.Extensions;


namespace R5T.Gepidia.Local.Construction
{
    public static class Construction
    {
        public static void SubMain()
        {
            //Construction.TestGUIDs();
            Construction.TestDirectoryEnumerateEntries();
        }

        private static void TestDirectoryEnumerateEntries()
        {
            var path = @"C:\Temp";

            var enumeratedPaths = Directory.EnumerateFileSystemEntries(path, SearchPatternHelper.All, SearchOption.TopDirectoryOnly);
            foreach (var enumeratedPath in enumeratedPaths)
            {
                Console.WriteLine(enumeratedPath);
            }
        }

        private static void TestGUIDs()
        {
            var guid = Guid.NewGuid();

            var uppercaseRepresentation = guid.ToStringUppercase();

            Console.WriteLine(uppercaseRepresentation);
        }
    }
}
