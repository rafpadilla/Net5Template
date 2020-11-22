using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Net5Template.Core.Bus;
using Net5Template.Core.Services;
using Net5Template.Core.Enums;

namespace Net5Template.Application.Services.Services.Images.Commands
{
    public class SaveImageBase64Command : ICommand<string>
    {
        public string FileBase64 { get; set; }
        public ImageSaveLocationEnum ImageLocation { get; set; }
        public Guid? FileName { get; set; }
        public ImageSizeEnum SaveMaxSizes { get; set; }

    }
    public class SaveImageBase64CommandHandler : ICommandHandler<SaveImageBase64Command, string>
    {
        private readonly IImageService _imageService;

        public SaveImageBase64CommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }
        public async Task<string> Handle(SaveImageBase64Command request, CancellationToken cancellationToken)
        {
            var fileName = request.FileName ?? Guid.NewGuid();

            return await _imageService.SaveImagesBase64(request.FileBase64, request.ImageLocation, fileName.ToString("D"), request.SaveMaxSizes);
        }
    }
}