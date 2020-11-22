using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Enums;
using Net5Template.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Services.Images.Queries
{
    public class GetImagePathQuery : IQuery<string>
    {
        public GetImagePathQuery(string fileName, ImageSaveLocationEnum imageLocation, ImageSizeEnum imgSize = ImageSizeEnum.ImgOriginal,
            bool returnEmptyIfNotFound = false, bool returnMainLogoIfEmpty = false)
        {
            FileName = fileName;
            ImageLocation = imageLocation;
            ImgSize = imgSize;
            ReturnEmptyIfNotFound = returnEmptyIfNotFound;
            ReturnMainLogoIfEmpty = returnMainLogoIfEmpty;
        }
        public string FileName { get; }
        public ImageSaveLocationEnum ImageLocation { get; }
        public ImageSizeEnum ImgSize { get; }
        public bool ReturnEmptyIfNotFound { get; }
        public bool ReturnMainLogoIfEmpty { get; }
    }
    public class GetImagePathHandler : IQueryHandler<GetImagePathQuery, string>
    {
        private readonly IImageService _imageService;

        public GetImagePathHandler(IImageService imageService)
        {
            _imageService = imageService;
        }
        public Task<string> Handle(GetImagePathQuery request, CancellationToken cancellationToken)
        {
            var filePath = _imageService.GetImagePath(request.FileName, request.ImageLocation, request.ImgSize, request.ReturnEmptyIfNotFound, request.ReturnMainLogoIfEmpty);
            return Task.FromResult(filePath);
        }
    }
}
