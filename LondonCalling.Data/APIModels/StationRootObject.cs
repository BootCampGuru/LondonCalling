using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels.Search
{
    public class Crowding
    {
        public string Type { get; set; }
}

public class Line
{
    public string Type { get; set; }
public string id { get; set; }
public string name { get; set; }
public string uri { get; set; }
public string type { get; set; }
public Crowding crowding { get; set; }
}

public class LineGroup
{
    public string Type { get; set; }
public string stationAtcoCode { get; set; }
public List<string> lineIdentifier { get; set; }
}

public class LineModeGroup
{
    public string Type { get; set; }
public string modeName { get; set; }
public List<string> lineIdentifier { get; set; }
}

public class AdditionalProperty
{
    public string Type { get; set; }
public string category { get; set; }
public string key { get; set; }
public string sourceSystemKey { get; set; }
public string value { get; set; }
}

public class Child
{
    public string Type { get; set; }
public string naptanId { get; set; }
public List<object> modes { get; set; }
public string icsCode { get; set; }
public string stationNaptan { get; set; }
public string hubNaptanCode { get; set; }
public List<object> lines { get; set; }
public List<object> lineGroup { get; set; }
public List<object> lineModeGroups { get; set; }
public bool status { get; set; }
public string id { get; set; }
public string commonName { get; set; }
public string placeType { get; set; }
public List<object> additionalProperties { get; set; }
public List<object> children { get; set; }
public double lat { get; set; }
public double lon { get; set; }
}

public class StopPoint
{
    public string Type { get; set; }
public string naptanId { get; set; }
public List<string> modes { get; set; }
public string icsCode { get; set; }
public string stopType { get; set; }
public string stationNaptan { get; set; }
public string hubNaptanCode { get; set; }
public List<Line> lines { get; set; }
public List<LineGroup> lineGroup { get; set; }
public List<LineModeGroup> lineModeGroups { get; set; }
public bool status { get; set; }
public string id { get; set; }
public string commonName { get; set; }
public double distance { get; set; }
public string placeType { get; set; }
public List<AdditionalProperty> additionalProperties { get; set; }
public List<Child> children { get; set; }
public double lat { get; set; }
public double lon { get; set; }
}

public class StationRootObject
{
        public StationRootObject()
        {
            stopPoints = new List<StopPoint>();
        }

        public string Type { get; set; }
public List<double> centrePoint { get; set; }
public List<StopPoint> stopPoints { get; set; }
public int pageSize { get; set; }
public int total { get; set; }
public int page { get; set; }
}
   
}
