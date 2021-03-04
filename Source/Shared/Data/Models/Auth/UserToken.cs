using System;
using System.ComponentModel.DataAnnotations;

namespace Refactorizando.Shared.Data.Models.Auth
{
    public class UserToken
    {

        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        
    }
}