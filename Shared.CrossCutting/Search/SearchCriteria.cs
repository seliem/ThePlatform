using System;
using System.Collections.Generic;

namespace Shared.CrossCutting.Search
{
    public class SearchCriteria
    {
        public int? PageSize { set; get; } = 10;
        public int? PageNumber { set; get; } = 1;
    }
}
