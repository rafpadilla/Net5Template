using Net5Template.Application.Services.Services.Images.Commands;
using Net5Template.Application.Services.Services.Images.Queries;
using Net5Template.Core.Bus;
using Net5Template.Core.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers.Services
{
    //You need a valid token (generated eg. in Front API) token SecretKey should be identical as used by FrontEnd, if not always validation will be wrong
    //TODO:folders should exists before upload images
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//If you need to upload images without authorization comment this line
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRouteTemplate.ROUTE_ENTITY)]
    [ApiController]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [ApiExplorerSettings(GroupName = "Services")]
    public class ImageController : Net5TemplateControllerBase
    {
        public ImageController(ILogger<ImageController> logger, ICommandBus command, IQueryBus query)
                 : base(logger, command, query)
        {
        }
        /// <summary>
        /// Save image anonymously
        /// </summary>
        /// <remarks>
        /// Profile = 0 -> "images/profile/" -> save image for user profile<br/>
        /// Uploads = 1 -> "images/uploads/" -> save any other image<br/>
        /// </remarks>
        /// <param name="file"></param>
        /// <param name="imageSaveLocation"></param>
        /// <returns>Image Guid (uuid)</returns>
        [HttpPost("AnonymousFile")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAnonymousFile(IFormFile file, ImageSaveLocationEnum imageSaveLocation)
        {
            var model = new SaveImageStreamCommand();
            model.FileStream = file.OpenReadStream();
            model.ImageLocation = imageSaveLocation;
            //if (imageSaveLocation == ImageSaveLocationEnum.Profile)
            //    model.CurrentUserId = User.GetUserId();

            var imgGuid = await _commandBus.Send(model);
            return Ok(imgGuid);
        }
        /// <summary>
        /// Save image
        /// </summary>
        /// <remarks>
        /// Profile = 0 -> "images/profile/" -> save image for user profile<br/>
        /// Uploads = 1 -> "images/uploads/" -> save any other image<br/>
        /// </remarks>
        /// <param name="file"></param>
        /// <param name="imageSaveLocation"></param>
        /// <returns>Image Guid (uuid)</returns>
        [HttpPost("File")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(IFormFile file, ImageSaveLocationEnum imageSaveLocation)
        {
            var model = new SaveImageStreamCommand();
            model.FileStream = file.OpenReadStream();
            model.ImageLocation = imageSaveLocation;
            //if (imageSaveLocation == ImageSaveLocationEnum.Profile)
            //    model.CurrentUserId = User.GetUserId();

            var imgGuid = await _commandBus.Send(model);
            return Ok(imgGuid);
        }
        /// <summary>
        /// Save image in base64 format
        /// </summary>
        /// <remarks>
        /// Profile = 0 -> "images/profile/" -> save image for user profile<br/>
        /// Uploads = 1 -> "images/uploads/" -> save any other image<br/>
        /// </remarks>
        /// <param name="fileBase64"></param>
        /// <param name="imageSaveLocation"></param>
        /// <returns>Image Guid (uuid)</returns>
        [HttpPost("Base64")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] string fileBase64, ImageSaveLocationEnum imageSaveLocation)
        {
            var model = new SaveImageBase64Command();
            model.FileBase64 = fileBase64;
            model.ImageLocation = imageSaveLocation;
            //if (imageSaveLocation == ImageSaveLocationEnum.Profile)
            //    model.CurrentUserId = User.GetUserId();

            var imgGuid = await _commandBus.Send(model);
            return Ok(imgGuid);
        }

        #region CKEditor FileUpload Helper
        /// <summary>
        /// Upload images from CKEditor (WYSIWYG Editor)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("EmbedContentImage")]
        public async Task<ActionResult> UploadImage(IFormFile file)
        {
            string error = string.Empty;
            string retImageUrl = string.Empty;
            if (file == null || file.Length == 0 || !file.ContentType.Contains("image"))
            {
                error = "La imagen no es válida";
            }
            else
            {
                var model = new SaveImageStreamCommand();
                model.FileStream = file.OpenReadStream();
                model.ImageLocation = ImageSaveLocationEnum.Uploads;
                model.SaveMaxSizes = ImageSizeEnum.Img900;

                var imageUrl = await _commandBus.Send(model);

                retImageUrl = await _queryBus.Send(new GetImagePathQuery(imageUrl, ImageSaveLocationEnum.Uploads, ImageSizeEnum.Img900));
            }

            //para el upload de ckeditor utilizar siempre url completa
            if (!retImageUrl.StartsWith("http"))
            {
                var uri = await _queryBus.Send(new GetImageAbsoluteUriQuery());
                retImageUrl = uri.ToString().TrimEnd('/') + retImageUrl;
            }

            return Ok(new { url = retImageUrl, error });
        }
        #endregion
    }
}