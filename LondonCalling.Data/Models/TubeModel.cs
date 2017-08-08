using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.Models
{
    public class TubeModel
    {
        public string Id { get; set; }
        public string CurrentLocation { get; set; }
        public string Destination { get; set; }
        public string ArrivalTime { get; set; }

        public string Line { get; set; }
    }
}
