using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Refactorizando.Shared.Data.Models
{

    public class Request
    {
        public enum RequestStates
        {
            Pending = 1,
            Accepted = 2,
            Rejected = 3,
            Completed = 4    
        }


        public enum RequestStateReasons
        {
            Accepted = 1,
            Pending = 2,
            TooBigProject = 3,
            TooComplicate = 4,
            TooShort = 5,
            Repeated = 6,
            Postponed = 7
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string RepositoryUri { get; set; }
        public string Description { get; set; }
        public string SystemUserId { get; set; }
        public SystemUser SystemUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public RequestStates State { get; set; }
        public RequestStateReasons StateReason { get; set; }
        public string Comments { get; set; }
        public string VideoUrl { get; set; }

        public List<LikeRequest> LikeRequests {get; set; }

    }
}