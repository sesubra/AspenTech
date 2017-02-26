using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes.Models
{
    public class Node
    {
        public Node(string id)
        {
            this.Id = id;
            this.IsConnected = false;
            this.IsRoot = false;
            this.In = new List<Link>();
            this.Out = new List<Link>();
        }
        public Node(string id, bool isRoot)
            : this(id)
        {
            this.IsConnected = (isRoot == true);
            this.IsRoot = isRoot;
        }

        public string Id { get; set; }
        public bool IsConnected { get; set; }
        public bool IsRoot { get; set; }
        public List<Link> In { get; set; }
        public List<Link> Out { get; set; }

        public Link RemoveOutEdge(string toNodeId)
        {
            var linkToRemove = this.Out.Where(l => l.NodeId == toNodeId).FirstOrDefault();
            this.Out.Remove(linkToRemove);
            return linkToRemove;
        }

        public Link RemoveInEdge(string fromNodeId)
        {
            var linkToRemove = this.In.Where(l => l.NodeId == fromNodeId).FirstOrDefault();
            this.In.Remove(linkToRemove);
            return linkToRemove;
        }

        public void ResetOutLinkVisitedFlag()
        {
            this.Out.ForEach(l => l.IsVisited = false);
        }

        public void ResetInLinkVisitedFlag()
        {
            this.In.ForEach(l => l.IsVisited = false);
        }
    }
}
