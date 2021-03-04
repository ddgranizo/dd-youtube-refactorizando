using System;

namespace Refactorizando.Shared.Data.Models
{
    public class BlobItem
    {
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get { return CreatedOn.ToString();  } }
    }
}