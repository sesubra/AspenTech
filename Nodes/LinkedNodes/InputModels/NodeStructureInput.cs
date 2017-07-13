using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes.InputModels
{
    ///Node Input
    public class NodeInput
    {
        [JsonProperty("id")]
        [JsonRequired]
        public string Id { get; set; }
    }

    //THIS IS AN EDGE INPUT NODE
    //EdgeInput hotfix 2
    //second part hotfix
    public class EdgeInput
    {
        //REquired property
        [JsonProperty("from")]
        [JsonRequired]
        public string From { get; set; }
        //required TO property
        [JsonProperty("to")]
        [JsonRequired]
        public string To { get; set; }
    }
    //sample one here as well
    //Node structure input
    //this is also hotfix 4
    public class NodeStructureInput
    {
        //Root property
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
