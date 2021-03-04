using System;

namespace Refactorizando.Shared.Data.Models.Dtos
{
    public class SystemUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }

    }
}