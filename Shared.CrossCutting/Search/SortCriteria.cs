using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Search
{
    public class SortCriteria
    {
        public string SortColumn { set; get; }
        public bool? IsDescending { set; get; }
    }
}
