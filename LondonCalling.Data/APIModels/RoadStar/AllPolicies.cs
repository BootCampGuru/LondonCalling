using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels.RoadStar
{
    public class Metadata
    {
        public string version { get; set; }
        public int count { get; set; }
    }

    public class Inputs
    {
        public string limit { get; set; }
    }

    public class Category
    {
        public string code { get; set; }
        public string title { get; set; }
        public string category_type { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string state { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string enacted_date { get; set; }
        public string amended_date { get; set; }
        public string plaintext { get; set; }
        public bool is_recent { get; set; }
        public int? seq_num { get; set; }
        public string type { get; set; }
        public string agency { get; set; }
        public string significant_update_date { get; set; }
        public string recent_update_or_new { get; set; }
        public List<Category> categories { get; set; }
        public List<object> types { get; set; }
        public List<object> references { get; set; }
        public List<object> topics { get; set; }
    }

    public class PolicyRootObject
    {
        public Metadata metadata { get; set; }
        public Inputs inputs { get; set; }
        public List<Result> result { get; set; }
    }

    public class SinglePolicyRootObject
    {
        public Metadata metadata { get; set; }
        public Inputs inputs { get; set; }
        public Result result { get; set; }
    }
}
