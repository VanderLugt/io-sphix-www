using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace AspNetCoreWebApplication.Models
{
    public class ClaimAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext _context { get { return _httpContextAccessor.HttpContext; } }
        public ClaimAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            //string od = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
          //  UserId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public long UserId
        {
            get
            {
                long userId = 0;
                // Figure out the user's identity
                if (_context != null)
                {
                    if (_context.User != null)
                    {
                        var identity = _context.User.Identity;

                        if (identity != null && identity.IsAuthenticated)
                        {
                            userId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                        }
                    }
                }

                return userId;
            }


        }
        public string UserName
        {
            get
            {
                var userName = "SystemGenerated";
                // Figure out the user's identity
                if (_context != null)
                {
                    if (_context.User != null)
                    {
                        var identity = _context.User.Identity;

                        if (identity != null && identity.IsAuthenticated)
                        {
                            userName = identity.Name;
                        }
                    }
                }

                return userName;
            }


        }
        public string Roles
        {
            get
            {
                var Roles = "SystemGenerated";
                // Figure out the user's identity
                if (_context != null)
                {
                    if (_context.User != null)
                    {
                        var identity = _context.User.Identity;

                        if (identity != null && identity.IsAuthenticated)
                        {
                            Roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                        }
                    }
                }

                return Roles;
            }


        }
    }
}
