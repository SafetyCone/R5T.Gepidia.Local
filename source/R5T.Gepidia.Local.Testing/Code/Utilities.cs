using System;

using R5T.Lombardy;
using R5T.Magyar;


namespace R5T.Gepidia.Local.Testing
{
    public static class Utilities
    {
        public static string GetLocalTestingRootDirectoryPath(IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var userDirectoryPath = PathHelper.UserProfileDirectoryPathValue;

            var randomDirectoryName = PathHelper.GetRandomDirectoryName();

            var rootDirectoryPath = stringlyTypedPathOperator.GetDirectoryPath(userDirectoryPath, randomDirectoryName);
            return rootDirectoryPath;
        }
    }
}
