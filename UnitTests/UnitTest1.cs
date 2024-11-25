using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Moq<SongDownloadService> mockedService = new Moq<SongDownloadService>();
            //call the code
            Assert.Equals(1, 1);    
        }
    }
}
