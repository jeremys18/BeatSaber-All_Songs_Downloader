using BeatSaberDownloader.Data;
using BeatSaberSongDownloader.Data.Models.BareModels;
using BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetAllSongs.Query;
using BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetSong.Query;
using BeatSaberSongDownloader.Server.Services.SongDownloader;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BeatSaberSongDownloader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private IMediator _mediator;
        private ILogger<SongDownloadService> _logger;

        public SongController(IMediator mediator, ILogger<SongDownloadService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("test")]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Server is running...");
        }

        [Route("testsong")]
        [HttpGet]
        public async Task<IActionResult> TestSong()
        {
            var query = new GetSongQuery { 
                SongId = "2cf84", 
                VersionHash = "37b0e0b00e5e4a82047a2190ac20747e3016e7ea" 
            };
            var fileContent = await _mediator.Send(query);
            return File(fileContent, "application/zip");
        }

        [Route("allsongs")]
        [HttpGet]
        public async Task<IActionResult> GetAllSongsInfoAsync(string basePath)
        {
            // return song info list
            var result = await _mediator.Send(new GetAllSongsQuery { SongBasePath = basePath});
            return Ok(result);
        }

        [Route("{SongId}/{VersionHash}")]
        [HttpGet]
        public async Task<IActionResult> GetSongFile([FromRoute] GetSongQuery query)
        {
            if(!Request.Headers.ContainsKey(Consts.AppTokenHeaderName) || Request.Headers[Consts.AppTokenHeaderName] != Consts.AppTokenValue
                || !Request.Headers.ContainsKey(Consts.YoloHoloHeaderName) || Request.Headers[Consts.YoloHoloHeaderName] != Consts.YoloHoloHeaderValue)
            {
                return Unauthorized();
            }

            // return song info for specific song id
            var fileContent = await _mediator.Send(query);
            if(fileContent == null)
            {
                return NotFound();
            }

            return File(fileContent, "application/zip");
        }
    }
}
