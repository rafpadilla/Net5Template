using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Enums
{
    [Flags]
    public enum LanguageEnum
    {
        [Description("es-ES")]
        es_ES = 1,
        [Description("en-US")]
        en_US = 2
    }
}
