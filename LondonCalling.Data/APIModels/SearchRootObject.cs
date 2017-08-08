using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Data.APIModels
{
    public class Match
    {
        public string Type { get; set; }
    public string icsId { get; set; }
    public List<string> modes { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string topMostParentId { get; set; }
}

public class SearchRootObject
{
    public string Type { get; set; }
public string query { get; set; }
public int total { get; set; }
public List<Match> matches { get; set; }
}
}
