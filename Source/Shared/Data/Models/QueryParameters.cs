
using System;

namespace Refactorizando.Shared.Data.Models
{
    public class QueryParameters
    {
        public string SearchText { get; set; }
        public string Filter { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public const int MaxCount = 9999;
    }
}