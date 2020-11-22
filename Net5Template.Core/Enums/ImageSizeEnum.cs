using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Enums
{
    public enum ImageSizeEnum
    {
        WithoutMark = 0,
        [DefaultValue(0)]
        ImgOriginal = 1,
        [DefaultValue(1920)]
        Img1920 = 2,
        [DefaultValue(1200)]
        Img1200 = 3,
        [DefaultValue(900)]
        Img900 = 4,
        [DefaultValue(600)]
        Img600 = 5,
        [DefaultValue(300)]
        Img300 = 6
    }
}
