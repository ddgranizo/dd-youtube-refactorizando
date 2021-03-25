using System;
using static Refactorizando.Shared.Data.Models.Request;

namespace Refactorizando.Shared.Data.Models.Dtos
{
    public class RequestDto
    {
        
        public Guid Id { get; set; }
        public string RepositoryUri { get; set; }
        public string Description { get; set; }
        public string SystemUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public RequestStates State { get; set; }
        public RequestStateReasons StateReason { get; set; }
        public string Comments { get; set; }
        public string VideoUrl { get; set; }

        public int LikesCount { get; set; }
        public bool HasCurrentUserLike { get; set; }

        public SystemUserDto SystemUser { get; set; }
        public bool HasVideo()  => !string.IsNullOrEmpty(VideoUrl);

        public string StateString()
            => State switch {
                RequestStates.Accepted => "Accepted",
                RequestStates.Completed => "Completed",
                RequestStates.Pending => "Pending",
                RequestStates.Rejected => "Rejected",
                _ => throw new NotImplementedException()
            };

        public string StateReasonString()
            => StateReason switch {
                RequestStateReasons.Accepted => "Accepted",
                RequestStateReasons.Pending => "Pending",
                RequestStateReasons.Postponed => "Postponed",
                RequestStateReasons.Repeated => "Repeated",
                RequestStateReasons.TooBigProject => "Too big project",
                RequestStateReasons.TooComplicate => "Too complicate",
                RequestStateReasons.TooShort => "Too short",
                _ => throw new NotImplementedException()
            };
        

    }
}