using BeatSaberSongDownloader.Server.Services.SongDownloader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsss
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void Test1()
        {
            var mockedService = new Moq<SongDownloadService>();
            Assert.Equals(1, 1);
        }
    }
}