using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeatSaberSongDownloader.Server.Attributes
{
    internal class VerifyAccessAttribute : AuthorizeAttribute
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var header1 = context.HttpContext.Request.Headers["AppyWappy"];
            var header2 = context.HttpContext.Request.Headers["YoloHolo"];

            if(string.IsNullOrWhiteSpace(header1)
                || header1 != "b19969af-fae1-4c39-aa6a-da00959e20ca"
                || string.IsNullOrEmpty(header2)
                || header2 != "SiMeHomeyWomey")
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}