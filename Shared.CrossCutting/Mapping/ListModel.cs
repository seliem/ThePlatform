using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shared.CrossCutting.Mapping
{
    public class ListModel<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public ListModel(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}