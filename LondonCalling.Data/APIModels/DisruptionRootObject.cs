using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels
{
    public class DisruptionRootObject
    {
        public string Type { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public string categoryDescription { get; set; }
    public string description { get; set; }
    public List<object> affectedRoutes { get; set; }
    public List<object> affectedStops { get; set; }
    public string closureText { get; set; }
}
}
