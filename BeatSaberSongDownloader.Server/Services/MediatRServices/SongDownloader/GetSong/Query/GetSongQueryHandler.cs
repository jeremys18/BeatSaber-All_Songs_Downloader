﻿using BeatSaberDownloader.Data;
using BeatSaberDownloader.Data.Repositories;
using BeatSaberSongDownloader.Server.Handlers;
using MediatR;

namespace BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetSong.Query
{
    public class GetSongQueryHandler : IRequestHandler<GetSongQuery, byte[]?>
    {
        private IConfiguration _configuration;

        public GetSongQueryHandler(IConfiguration configuration)
        {
            _configuration= configuration;
        }

        public async Task<byte[]?> Handle(GetSongQuery query, CancellationToken cancellationToken)
        {
            byte[]? result;
            try
            {
                var songInfo = new BeatSaverRepository(_configuration).GetSongById(query.SongId);
                var verNumber = songInfo.versions.IndexOf(songInfo.versions.First(y => y.hash == query.VersionHash)) + 1;
                var fileName = TextHandler.GetValidFileName(Consts.SaveFolderPath, songInfo, verNumber);
                result = await File.ReadAllBytesAsync(fileName);
            }
            catch(Exception ex)
            {
                result = null;
            }
            
            
            return result;
        }
    }
}
