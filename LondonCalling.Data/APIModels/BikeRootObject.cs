using System;
using System.Collections.Generic;
using System.Text;

namespace LondonCalling.Data.APIModels
{
    public class AdditionalProperty
    {
        public string Name { get; set; }
    public string category { get; set; }
    public string key { get; set; }
    public string sourceSystemKey { get; set; }
    public string value { get; set; }
    public string modified { get; set; }
}

public class BikeRootObject
{
    public string Type { get; set; }
public string id { get; set; }
public string url { get; set; }
public string commonName { get; set; }
public string placeType { get; set; }
public List<AdditionalProperty> additionalProperties { get; set; }
public List<object> children { get; set; }
public List<object> childrenUrls { get; set; }
public double lat { get; set; }
public double lon { get; set; }
}

    public class TestRootObject
    {
        public string type { get; set; }
    public string id { get; set; }
    public string url { get; set; }
    public string commonName { get; set; }
    public string placeType { get; set; }
    public List<object> additionalProperties { get; set; }
    public List<object> children { get; set; }
    public List<object> childrenUrls { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
}



}
