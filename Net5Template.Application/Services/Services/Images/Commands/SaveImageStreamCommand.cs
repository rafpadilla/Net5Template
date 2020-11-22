using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Net5Template.Core.Bus;
using Net5Template.Core.Enums;
using Net5Template.Core.Services;

namespace Net5Template.Application.Services.Services.Images.Commands
{
    public class SaveImageStreamCommand : ICommand<string>
    {
        public Stream FileStream { get; set; }
        public ImageSaveLocationEnum ImageLocation { get; set; }
        public Guid? FileName { get; set; }
        public ImageSizeEnum SaveMaxSizes { get; set; }
    }
    public class SaveImageStreamCommandHandler : ICommandHandler<SaveImageStreamCommand, string>
    {
        private readonly IImageService _imageService;

        public SaveImageStreamCommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }
        public async Task<string> Handle(SaveImageStreamCommand request, CancellationToken cancellationToken)
        {
            var fileName = request.FileName ?? Guid.NewGuid();

            return await _imageService.SaveImagesStream(request.FileStream, request.ImageLocation, fileName.ToString("D"), request.SaveMaxSizes);
        }
    }
}