using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes.Models
{
    ///BAsic Link model
    public class Link
    {
        //default constructor (2)
        public Link(string nodeId)
        {
            this.NodeId = nodeId;
            this.IsVisited = false;
        }
        public string NodeId { get; set; }
        public bool IsVisited { get; set; }
    }
}
