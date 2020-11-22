using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Services.Images.Queries
{
    public class GetImageAbsoluteUriQuery : IQuery<Uri>
    {
    }
    public class GetImageAbsoluteUriHandler : IQueryHandler<GetImageAbsoluteUriQuery, Uri>
    {
        private readonly IImageService _imageService;

        public GetImageAbsoluteUriHandler(IImageService imageService)
        {
            _imageService = imageService;
        }
        public Task<Uri> Handle(GetImageAbsoluteUriQuery request, CancellationToken cancellationToken)
        {
            var uri = _imageService.GetAbsoluteUri();
            return Task.FromResult(uri);
        }
    }
}
