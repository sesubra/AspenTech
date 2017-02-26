using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes.InputModels
{
    public class NodeInput
    {
        [JsonProperty("id")]
        [JsonRequired]
        public string Id { get; set; }
    }

    public class EdgeInput
    {
        [JsonProperty("from")]
        [JsonRequired]
        public string From { get; set; }
        [JsonProperty("to")]
        [JsonRequired]
        public string To { get; set; }
    }

    public class NodeStructureInput
    {
        [JsonProperty("root")]
        [JsonRequired]
        public string Root { get; set; }
        [JsonProperty("nodes")]
        [JsonRequired]
        public List<NodeInput> Nodes { get; set; }
        [JsonProperty("edges")]
        [JsonRequired]
        public List<EdgeInput> Edges { get; set; }
    }
}
