using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Refactorizando.Shared.Data.Models;

namespace Refactorizando.Shared.Data.Models
{
    public class LikeRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string SystemUserId { get; set; }
        public SystemUser SystemUser { get; set; }
        public Guid RequestId { get; set; }
        public Request Request { get; set; }
    }
}