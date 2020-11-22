using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers
{
    public static class ApiVersions
    {
        public const string V1 = "1";
    }

    public static class ApiRouteTemplate
    {
        public const string ROUTE_ENTITY = "api/v{version:apiVersion}/[controller]";
        public const string USERS_ROUTE_ENTITY = "api/v{version:apiVersion}/Users/[controller]";
        public const string ADMIN_ROUTE_ENTITY = "api/v{version:apiVersion}/Admin/[controller]";
        public const string IDENTITY_ROUTE_ENTITY = "api/v{version:apiVersion}/Identity/[controller]";
        //public const string ACTION = "api/v{version:apiVersion}/[controller]/[action]";
    }
}
