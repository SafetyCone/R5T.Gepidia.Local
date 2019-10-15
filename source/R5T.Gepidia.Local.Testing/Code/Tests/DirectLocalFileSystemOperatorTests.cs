using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace R5T.Gepidia.Local.Testing
{
    [TestClass]
    public class DirectLocalFileSystemOperatorTests
    {
        private IFileSystemOperator LocalFileSystemOperator { get; } = new LocalFileSystemOperator();


        /// <summary>
        /// Tests that a local directory can be created.
        /// </summary>
        [TestMethod]
        public void CreateDirectory()
        {

        }
    }
}
