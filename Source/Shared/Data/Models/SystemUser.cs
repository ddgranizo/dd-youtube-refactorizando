using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Refactorizando.Shared.Data.Models
{
    public class SystemUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsEnabled { get; set; }
        public string ProfileUrl { get; set; }
        public virtual List<Request> Requests { get; set; }
        public virtual List<LikeRequest> LikeRequests {get; set; }

    }
}