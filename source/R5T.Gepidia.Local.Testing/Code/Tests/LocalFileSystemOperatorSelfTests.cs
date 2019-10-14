using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using R5T.Gepidia.Test;
using R5T.Lombardy;
using R5T.Magyar;

using GepidiaTestUtilities = R5T.Gepidia.Test.Utilities;


namespace R5T.Gepidia.Local.Testing
{
    [TestClass]
    public class LocalFileSystemOperatorSelfTests : FileSystemOperatorSelfTestFixture
    {
        #region Static

        private static LocalFileSystemOperator LocalFileSystemOperator { get; } = new LocalFileSystemOperator();
        private static StringlyTypedPathOperator LocalStringlyTypedPathOperator { get; } = new StringlyTypedPathOperator();
        private static string LocalRootDirectoryPath { get; } = GepidiaTestUtilities.GetTestingRootDirectoryPath(LocalFileSystemOperatorSelfTests.LocalStringlyTypedPathOperator);

        #endregion


        /// <summary>
        /// Even though the test-fixture will ensure the root directory exists (in order to do its job), ensure the root directory is created here in the derived test-class since the derived test-class is responsible for deleting the root directory.
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            LocalFileSystemOperatorSelfTests.LocalFileSystemOperator.CreateDirectoryOnlyIfNotExists(LocalFileSystemOperatorSelfTests.LocalRootDirectoryPath);
        }

        /// <summary>
        /// The derived test class takes responsiblity for cleaning up the root directory (the test-fixture base-class does not presume it should delete the root directory).
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            LocalFileSystemOperatorSelfTests.LocalFileSystemOperator.DeleteDirectoryOnlyIfExists(LocalFileSystemOperatorSelfTests.LocalRootDirectoryPath);
        }


        public LocalFileSystemOperatorSelfTests()
            : base(LocalFileSystemOperatorSelfTests.LocalFileSystemOperator, LocalFileSystemOperatorSelfTests.LocalRootDirectoryPath, LocalFileSystemOperatorSelfTests.LocalStringlyTypedPathOperator)
        {
        }
    }
}
