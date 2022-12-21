using BeatSaberSongDownloader.Data.Models.BareModels;
using BeatSaberSongDownloader.Server.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BeatSaberSongDownloader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        [Route("allsongs")]
        [HttpGet]
        [VerifyAccess]
        public List<Song> GetAllSongsInfo()
        {
            // return song info list
            return new List<Song> { new Song { BeatSaverDownloadUrl = "", FileName = "Hola", Id = "1d", Name = "Hola" } };
        }

        [Route("{songId}")]
        [HttpGet]
        [VerifyAccess]
        public Song GetSongFile(string songId)
        {
            // return song info for specific song id
            return new Song { BeatSaverDownloadUrl = "", FileName = "Hola", Id = "1d", Name = "Hola" };
        }
    }
}
