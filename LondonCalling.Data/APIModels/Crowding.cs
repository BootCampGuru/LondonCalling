using System;
using System.Collections.Generic;
using System.Text;

namespace LondonCalling.Data.APIModels
{
    public class Crowding
    {
        public string Name { get; set; }
}

public class Line
{
    public string LineName { get; set; }
public string id { get; set; }
public string name { get; set; }
public string uri { get; set; }
public string type { get; set; }
public Crowding crowding { get; set; }
}

public class LineGroup
{
    public string Name { get; set; }
public string stationAtcoCode { get; set; }
public List<string> lineIdentifier { get; set; }
public string naptanIdReference { get; set; }
}

public class LineModeGroup
{
    public string Name { get; set; }
public string modeName { get; set; }
public List<string> lineIdentifier { get; set; }
}

public class RootObject
{
    public string Name { get; set; }
public string naptanId { get; set; }
public string indicator { get; set; }
public string stopLetter { get; set; }
public List<string> modes { get; set; }
public string icsCode { get; set; }
public string stopType { get; set; }
public string stationNaptan { get; set; }
public List<Line> lines { get; set; }
public List<LineGroup> lineGroup { get; set; }
public List<LineModeGroup> lineModeGroups { get; set; }
public bool status { get; set; }
public string id { get; set; }
public string commonName { get; set; }
public string placeType { get; set; }
public List<object> additionalProperties { get; set; }
public List<object> children { get; set; }
public double lat { get; set; }
public double lon { get; set; }
public string hubNaptanCode { get; set; }
}
}
