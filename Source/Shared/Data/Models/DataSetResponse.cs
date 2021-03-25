using System;
using System.Collections.Generic;

namespace Refactorizando.Shared.Data.Models
{
    public class DataSetResponse<T>
    {
        public IEnumerable<T> Values { get; set; }
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public double TotalCount { get; set; }

        public DataSetResponse()
        {
            Values = new List<T>();
        }

        public DataSetResponse(QueryParameters queryParameters)
        {
            if (queryParameters is null)
            {
                throw new ArgumentNullException(nameof(queryParameters));
            }

            Count = queryParameters.Count;
            CurrentPage = queryParameters.Page;
        }

        public void SetValues(IEnumerable<T> values)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }
    }
}