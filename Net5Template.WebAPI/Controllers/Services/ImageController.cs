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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//si se necesitan subir imágenes sin haber iniciado sesión quitar esto
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
        /// Guardar imagen
        /// </summary>
        /// <remarks>
        /// Listing = 0 -> "images/listing/" -> para guardar imágenes de publicaciones<br/>
        /// Profile = 1 -> "images/profile/" -> para guardar la imagen principal del perfil de usuario<br/>
        /// Uploads = 2 -> "images/uploads/" -> para guardar imágenes del blog y seo de páginas estáticas<br/>
        /// ProfileContent = 3 -> "images/profilecontent/" -> para imágenes del cuerpo de perfil de usuario<br/>
        /// </remarks>
        /// <param name="file"></param>
        /// <param name="imageSaveLocation"></param>
        /// <returns>Guid de la imagen (uuid)</returns>
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
        /// Guardar imagen en formato base64
        /// </summary>
        /// <remarks>
        /// Listing = 0 -> "images/listing/" -> para guardar imágenes de publicaciones<br/>
        /// Profile = 1 -> "images/profile/" -> para guardar la imagen principal del perfil de usuario<br/>
        /// Uploads = 2 -> "images/uploads/" -> para guardar imágenes del blog y seo de páginas estáticas<br/>
        /// ProfileContent = 3 -> "images/profilecontent/" -> para imágenes del cuerpo de perfil de usuario<br/>
        /// </remarks>
        /// <param name="fileBase64"></param>
        /// <param name="imageSaveLocation"></param>
        /// <returns>Guid de la imagen (uuid)</returns>
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
        /// Subida de imágenes desde el CKEditor
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