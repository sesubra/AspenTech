using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes.Models
{
    public class NodesList : SortedList<string, Node>
    {
        public string RootNodeId { get; private set; }

        public string[] ValidatePathToAllChildrenFromRoot()
        {
            var rootNodes = this.Values.Where(n => n.IsRoot == true);
            if(rootNodes.Count() > 1)
            {
                throw new Exception($"More than one root nodes found. Number of root nodes: {rootNodes.Count()}");
            }

            var rootNode = rootNodes.FirstOrDefault();
            if(rootNode == null)
            {
                throw new Exception("No root node found.");
            }

            this.RootNodeId = rootNode.Id;
            foreach (var outNode in rootNode.In)
            {
                MarkLinkedToRoot(rootNode.Id, rootNode, outNode);
            }

            return this.Values.Where(n => n.IsConnected == false).Select(n => n.Id).ToArray();
        }

        private void MarkLinkedToRoot(string rootNodeId, Node parentNode, Link link)
        {
            this[link.NodeId].IsConnected = true;
            link.IsVisited = true;
            //ConsoleHelper.PrintMessage($"{link.NodeId} is marked connected.");
            foreach (var innerLink in this[link.NodeId].In.Where(l => l.NodeId != parentNode.Id //not connecting back to the parent
                                                                && l.IsVisited == false //do not visit the same link again
                                                                && l.NodeId != link.NodeId //not connecting to itself
                                                                && l.NodeId != rootNodeId)) //not connecting back to root
            {
                MarkLinkedToRoot(rootNodeId, this[innerLink.NodeId], innerLink);
            }
        }

        public string[] ValidateNode(string fromNodeId)
        {
            return ValidateNodesAfterRemovedEdge(fromNodeId);
        }

        public string[] ValidateNodesAfterRemovedEdge(string fromNodeId)
        {
            //first level link to ROOT is not avilable
            //this condition should be captured on the upper level itself
            //this should not be happening
            if(this[fromNodeId].Out.Where(l => l.NodeId == this.RootNodeId).Count() > 0)
            {
                return new string[] { };
            }

            //When an out going edge is removed, invalidate it's connection to root and re-eveluate with other Outgoing links
            this[fromNodeId].IsConnected = false;
            foreach (var link in this[fromNodeId].Out.Where(l => l.NodeId != fromNodeId)) //not connecting back to root
            {
                //ConsoleHelper.PrintMessage($"Traversing OUT edge {fromNodeId} to {link.NodeId}");
                var visitedNodes = new List<string>(new string[] { fromNodeId });
                if(this[link.NodeId].IsConnected == true)
                {
                    if (ValidateConnectionToRoot(visitedNodes, this[fromNodeId], this[link.NodeId]) == true)
                    {
                        this[fromNodeId].IsConnected = true;
                        break;
                    }
                }
                else
                {
                    //ConsoleHelper.PrintInfo($"Node {link.NodeId} is already disconnected from root. Skipping.");
                }
            }

            if (this[fromNodeId].IsConnected == false)
            {
                //Node from where the enge is removed is disconnected from root node. 
                //so cycle through all nodes in this node's Incoming edges and re-evaluate them to make sure those are still connected
                foreach (var link in this[fromNodeId].In)
                {
                    InValidateAllIncomingNodes(this[fromNodeId], this[link.NodeId]);
                }
                return this.Values.Where(n => n.IsConnected == false).Select(n => n.Id).ToArray();
            }

            return new string[] { };
        }

        private bool InValidateAllIncomingNodes(Node parentNode, Node node)
        {
            if (node.In.Count == 0) return true;
            if (node.Id == this.RootNodeId) return true;

            foreach (var link in node.In.Where(l => l.NodeId != node.Id && l.NodeId != parentNode.Id))
            {
                if (this[link.NodeId].IsConnected == true)
                {
                    //ConsoleHelper.PrintMessage($"Traversing IN edge {node.Id} to {link.NodeId}");
                    this[link.NodeId].IsConnected = false;
                    ValidateNodesAfterRemovedEdge(link.NodeId);
                }
                else
                {
                    //ConsoleHelper.PrintInfo($"Node {link.NodeId} is already disconnected from root. Skipping.");
                    return false;
                }
            }

            return false;
        }

        private bool ValidateConnectionToRoot(List<string> visitedNodes, Node parentNode, Node node)
        {
            //ConsoleHelper.PrintMessage($"Validating {parentNode.Id} to {node.Id}");
            if (node.Out.Count == 0) return false;
            if (node.Id == this.RootNodeId) return true;

            foreach (var innerLink in node.Out.Where(l => l.NodeId != node.Id //not connecting back to the parent
                                                                && visitedNodes.Contains(l.NodeId) == false
                                                                && l.NodeId != parentNode.Id)) //not connecting to itself
            {
                this[innerLink.NodeId].IsConnected = false;
                if (innerLink.NodeId == this.RootNodeId)        //direct connection to root node
                {
                    //ConsoleHelper.PrintSuccess($"Marking {innerLink.NodeId} is Connected");
                    this[innerLink.NodeId].IsConnected = true;
                    return true;
                }
                else
                {
                    visitedNodes.Add(node.Id);
                    if (ValidateConnectionToRoot(visitedNodes, node, this[innerLink.NodeId]))   //validate Outgoing edges recursively until it reaches the end of it
                    {
                        //ConsoleHelper.PrintSuccess($"Marking {innerLink.NodeId} is Connected");
                        this[innerLink.NodeId].IsConnected = true;
                        return true;
                    }
                }
            }
            //ConsoleHelper.PrintError($"Marking {node.Id} is Not Connected");
            node.IsConnected = false;
            return false;       //reached the end - Not connected to root
        }
    }
}
