using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels.RoadStar
{
    public class ContactMetadata
    {
        public string version { get; set; }
        public int count { get; set; }
    }

    public class ContactInputs
    {
        public string jurisdiction { get; set; }
    }

    public class ContactResult
    {
        public string title { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string agency { get; set; }
        public string web_page { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public int seq_num { get; set; }
    }

    public class ContactRootObject
    {
        public ContactMetadata metadata { get; set; }
        public ContactInputs inputs { get; set; }
        public List<ContactResult> result { get; set; }
    }
}
