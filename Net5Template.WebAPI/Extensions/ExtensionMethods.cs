using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Net5Template.WebAPI
{
    public static class ExtensionMethods
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
                return Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value);

            throw new InvalidOperationException();
        }
    }
}
