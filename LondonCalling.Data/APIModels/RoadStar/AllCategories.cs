using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels.RoadStar
{
    public class CategoryMetadata
    {
        public string version { get; set; }
        public int count { get; set; }
    }

    public class CategoryResult
    {
        public string category_type { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public int sort_order { get; set; }
        public string help_text { get; set; }
    }

    public class CategoryRootObject
    {
        public CategoryMetadata metadata { get; set; }
        public List<CategoryResult> result { get; set; }
    }
}
