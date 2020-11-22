using Net5Template.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Services
{
    public interface IImageService
    {
        bool DeleteImages(string fileName, ImageSaveLocationEnum imageLocation);
        Uri GetAbsoluteUri();
        string GetImagePath(string fileName, ImageSaveLocationEnum imageLocation, ImageSizeEnum imgSize = ImageSizeEnum.ImgOriginal, bool returnEmptyIfNotFound = false, bool returnMainLogoIfEmpty = false);
        bool GetImagePathExists(string fileName, ImageSaveLocationEnum imageLocation, ImageSizeEnum imgSize = ImageSizeEnum.ImgOriginal);
        string GetUserProfileImagePath(string name, ImageSizeEnum imageSize = ImageSizeEnum.Img300);
        string ImageVersion(string filePath);
        Task<string> SaveImagesBase64(string imageBase64, ImageSaveLocationEnum imageLocation, string fileName = "", ImageSizeEnum saveMaxSizes = ImageSizeEnum.ImgOriginal);
        Task<string> SaveImagesStream(Stream fileStream, ImageSaveLocationEnum imageLocation, string fileName = "", ImageSizeEnum saveMaxSizes = ImageSizeEnum.ImgOriginal);
    }
}
