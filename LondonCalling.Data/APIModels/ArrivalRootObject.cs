using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels
{
    public class Timing
    {
        public string Type { get; set; }
    public string countdownServerAdjustment { get; set; }
    public string source { get; set; }
    public string insert { get; set; }
    public string read { get; set; }
    public string sent { get; set; }
    public string received { get; set; }
}

public class ArrivalRootObject
{
    public string Type{ get; set; }
public string id { get; set; }
public int operationType { get; set; }
public string vehicleId { get; set; }
public string naptanId { get; set; }
public string stationName { get; set; }
public string lineId { get; set; }
public string lineName { get; set; }
public string platformName { get; set; }
public string bearing { get; set; }
public string timestamp { get; set; }
public int timeToStation { get; set; }
public string currentLocation { get; set; }
public string towards { get; set; }
public string expectedArrival { get; set; }
public string timeToLive { get; set; }
public string modeName { get; set; }
public Timing timing { get; set; }
public string direction { get; set; }
public string destinationNaptanId { get; set; }
public string destinationName { get; set; }
}
}
