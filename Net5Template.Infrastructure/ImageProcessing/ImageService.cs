using Net5Template.Core;
using Net5Template.Core.Enums;
using Net5Template.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.ImageProcessing
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IWebHostEnvironment env, ILogger<ImageService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveImagesStream(Stream fileStream, ImageSaveLocationEnum imageLocation, string fileName = "", ImageSizeEnum saveMaxSizes = ImageSizeEnum.ImgOriginal)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid().ToString("D");

            if (fileStream != null)
            {
                using Image img = await Image.LoadAsync(fileStream);

                await SaveImage(img, imageLocation, fileName, saveMaxSizes);
            }
            return fileName;
        }
        /// <summary>
        /// Helper para guardar las imágenes con distintos tamaños, utilizando GetImagePath se puede pedir el tamaño específico para la página
        /// </summary>
        /// <param name="base64Image">La imagen en base64</param>
        /// <param name="imageLocation">Enumerador con la ubicación por defecto a guardar la imagen</param>
        /// <param name="fileName">Nombre opcional, se generará un guid en caso de dejarse en blanco</param>
        /// <param name="saveMaxSizes">Tamaño máximo a guardarse, se guardarán versiones más pequeñas a ese tamaño</param>
        /// <returns>El nombre del archivo creado en crudo (sin extensión), al utilizarse GetImagePath con el nombre de archivo como identificador se completa la info</returns>
        public async Task<string> SaveImagesBase64(string imageBase64, ImageSaveLocationEnum imageLocation, string fileName = "", ImageSizeEnum saveMaxSizes = ImageSizeEnum.ImgOriginal)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid().ToString("D");

            using Image img = await Base64ToImage(imageBase64);

            await SaveImage(img, imageLocation, fileName, saveMaxSizes);

            return fileName;
        }
        private async Task SaveImage(Image img, ImageSaveLocationEnum imageLocation, string fileName = "", ImageSizeEnum saveMaxSizes = ImageSizeEnum.ImgOriginal)
        {
            decimal proportion = decimal.Divide(img.Size().Height, img.Size().Width);//hecho así para mejorar la precisión

            //si la proporción es mayor a 2:1 no hacer conversión ya que se la imagen se distorsionará
            if (proportion > 2)
                proportion = 1;

            // Format is automatically detected though can be changed.
            //ISupportedImageFormat format = new JpegFormat { Quality = 90 };
            var imageSizes = Enum.GetValues(typeof(ImageSizeEnum)).Cast<ImageSizeEnum>();
            if (saveMaxSizes == ImageSizeEnum.WithoutMark || saveMaxSizes == ImageSizeEnum.ImgOriginal)
            {
                saveMaxSizes = ImageSizeEnum.ImgOriginal;
            }
            imageSizes = imageSizes.SkipWhile(a => a != saveMaxSizes);

            foreach (var i in imageSizes)
            {
                int enumSize = (int)i.GetDefaultValue();
                Size size = new Size(enumSize, (int)(enumSize * proportion));

                // Initialize the ImageFactory using the overload to preserve EXIF metadata.

                var returnImageUrl = imageLocation.GetDescription() + fileName + "_" + enumSize + ".jpg";
                var path = Path.Combine(_env.WebRootPath, returnImageUrl);

                if (!size.Equals(default))
                    img.Mutate(a => a.Resize(size));

                await img.SaveAsJpegAsync(path);
            }
        }
        private async Task<Image> Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(System.Text.RegularExpressions.Regex.Replace(base64String, @"^data:image\/[a-zA-Z]+;base64,", string.Empty));

            using MemoryStream ms = new MemoryStream(imageBytes);

            Image image = await Image.LoadAsync(ms);

            return image;
        }
        public string GetImagePath(string fileName, ImageSaveLocationEnum imageLocation, ImageSizeEnum imgSize = ImageSizeEnum.ImgOriginal, bool returnEmptyIfNotFound = false, bool returnMainLogoIfEmpty = false)
        {
            var filePath = imageLocation.GetDescription() + fileName + (imgSize == ImageSizeEnum.WithoutMark ? "" : "_" + imgSize.GetDefaultValue().ToString()) + ".jpg";
            if (File.Exists(Path.Combine(_env.WebRootPath, filePath)))
            {
                var fi = File.GetCreationTime(Path.Combine(_env.WebRootPath, filePath));
                return $"/{filePath}?v={fi.Ticks}";
            }
            else if (returnEmptyIfNotFound)
            {
                return string.Empty;
            }
            else if (returnMainLogoIfEmpty)
            {
                return "/images/common/logo_og.png";
            }
            else
            {
                switch (imageLocation)
                {
                    case ImageSaveLocationEnum.Uploads:
                        return "/images/common/default_upload.jpg";
                    case ImageSaveLocationEnum.Profile:
                        return "/images/common/default_profile.png";
                    default:
                        return "/images/common/default.jpg";
                }
            }
        }
        /// <summary>
        /// Método utilizado para borrar todas las imágenes de los tamaños previamente creados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="imageLocation"></param>
        /// <returns></returns>
        public bool DeleteImages(string fileName, ImageSaveLocationEnum imageLocation)
        {
            foreach (var size in Enum.GetValues(typeof(ImageSizeEnum)).Cast<ImageSizeEnum>())
            {
                string filePath = GetImagePath(fileName, imageLocation, size, true);

                if (string.IsNullOrEmpty(filePath))
                    continue;

                var fi = new FileInfo(filePath);
                if (fi.Exists)
                    fi.Delete();
            }
            return true;
        }

        public bool GetImagePathExists(string fileName, ImageSaveLocationEnum imageLocation, ImageSizeEnum imgSize = ImageSizeEnum.ImgOriginal)
        {
            var filePath = imageLocation.GetDescription() + fileName + (imgSize == ImageSizeEnum.WithoutMark ? "" : "_" + imgSize.GetDefaultValue().ToString()) + ".jpg";

            return File.Exists(Path.Combine(_env.WebRootPath, filePath));
        }
        public Uri GetAbsoluteUri()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;

            if (_env.IsDevelopment() && request.Host.Port.HasValue)
                uriBuilder.Port = request.Host.Port.Value;

            //uriBuilder.Path = request.Path.ToString();
            //uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }
        public string ImageVersion(string filePath)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(Path.Combine(_env.WebRootPath, filePath));

            // display version in dex format
            return string.Format("{0}?v={1:x}", GetAbsoluteUri().ToString().TrimEnd('/') + filePath, lastWriteTime.Ticks);
        }

        public string GetUserProfileImagePath(string name, ImageSizeEnum imageSize = ImageSizeEnum.Img300)
        {
            if (GetImagePathExists(name, ImageSaveLocationEnum.Profile, imageSize))
            {
                return ImageVersion(GetImagePath(name, ImageSaveLocationEnum.Profile, imageSize));
            }
            else if (GetImagePathExists(name, ImageSaveLocationEnum.Profile, ImageSizeEnum.WithoutMark))
            {
                return ImageVersion(GetImagePath(name, ImageSaveLocationEnum.Profile, ImageSizeEnum.WithoutMark));
            }
            else
            {
                return "/images/common/default.png";
            }
        }
    }
}