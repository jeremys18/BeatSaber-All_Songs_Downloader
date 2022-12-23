using BeatSaberSongDownloader.Data.Models.BareModels;
using BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetAllSongs.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeatSaberSongDownloader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private IMediator _mediator;

        public SongController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("allsongs")]
        [HttpGet]
        public async Task<IActionResult> GetAllSongsInfoAsync(string basePath)
        {
            // return song info list
            var result = await _mediator.Send(new GetAllSongsQuery { SongBasePath = basePath});
            return Ok(result);
        }

        [Route("{songId}")]
        [HttpGet]
        public Song GetSongFile(string songId)
        {
            // return song info for specific song id
            return new Song { BeatSaverDownloadUrl = "", FileName = "Hola", Id = "1d", Name = "Hola" };
        }
    }
}
