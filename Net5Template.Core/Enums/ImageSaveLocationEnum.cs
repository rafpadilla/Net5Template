using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Enums
{
    public enum ImageSaveLocationEnum
    {
        [Description("images/profile/")]
        Profile = 0,
        [Description("images/uploads/")]
        Uploads = 1
    }
}
