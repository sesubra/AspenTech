using LinkedNodes.InputModels;
using LinkedNodes.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedNodes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program starting .... reading nodes from JSON file input.");

            if(System.IO.File.Exists("nodestructure.json") == false)
            {
                ConsoleHelper.PrintError("File not found: nodestructure.json");
                Environment.Exit(0);
            }

            //Serializer settings to handle any malformed json file input
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Error = (object sender, ErrorEventArgs errorArgs) =>
            {
                if(errorArgs.ErrorContext.Member != null) ConsoleHelper.PrintError($"Error occured when trying to parse JSON for member: {errorArgs.ErrorContext.Member}");
                ConsoleHelper.PrintError(errorArgs.ErrorContext.Error.Message);
                errorArgs.ErrorContext.Handled = true;
                Environment.Exit(0);
            };
            var nodesInput = JsonConvert.DeserializeObject<NodeStructureInput>(System.IO.File.ReadAllText("nodestructure.json"), serializerSettings);

            //Successful deserialization of JSON input file
            NodesList nodesList = new NodesList();
            if(nodesInput.Nodes.FindIndex(n => n.Id == nodesInput.Root) < 0)
            {
                ConsoleHelper.PrintError("Root node is not found in the list of noodes.");
                return;
            }

            foreach (var node in nodesInput.Nodes)
            {
                if (nodesList.ContainsKey(node.Id) == true)
                {
                    ConsoleHelper.PrintError($"Multiple entries on node {node.Id} found.");
                }
                else
                {
                    nodesList.Add(node.Id, new Node(node.Id, node.Id == nodesInput.Root));
                }
            }

            foreach(var edge in nodesInput.Edges)
            {
                if(nodesList.ContainsKey(edge.From) && nodesList.ContainsKey(edge.To))
                {
                    nodesList[edge.From].Out.Add(new Link(edge.To));
                    nodesList[edge.To].In.Add(new Link(edge.From));
                }
                else
                {
                    ConsoleHelper.PrintError($"Invalid edge: From {edge.From} To {edge.To}.");
                }
            }

            //Traverse the nodes after initialization to make sure all the nodes are connected to root
            var disconnectedNodes = nodesList.ValidatePathToAllChildrenFromRoot();
            if(disconnectedNodes.Length > 0)
            {
                ConsoleHelper.PrintError($"All nodes are not connected to root. Disconnected nodes: {String.Join(",", disconnectedNodes)}");
            }

            while (true)
            {
                ConsoleHelper.PrintInput("Select starting node id to remove an edge: ");
                var fromNodeId = Console.ReadLine();

                if (nodesList.ContainsKey(fromNodeId) == false)
                {
                    ConsoleHelper.PrintError("Selected node is not found. Terminating....");
                    break;
                }
                else
                {
                    var fromNode = nodesList[fromNodeId];
                    if(fromNode.Out.Count == 0)
                    {
                        ConsoleHelper.PrintInfo($"Selected node {fromNodeId} does not have any valid outgoing edge to remove. Select another starting node.");
                        continue;
                    }

                    ConsoleHelper.PrintMessage($"Outgoing edge nodes: [{String.Join(", ", nodesList[fromNodeId].Out.Select(l => l.NodeId).ToArray())}]");
                    ConsoleHelper.PrintInput("Select one of the above nodeIds to remove the edge.");
                    var toNodeId = Console.ReadLine();
                    if (String.IsNullOrEmpty(toNodeId) == true || nodesList.ContainsKey(toNodeId) == false || fromNode.Out.Where(l => l.NodeId == toNodeId).Count() == 0)
                    {
                        ConsoleHelper.PrintError($"Link not found from node {fromNodeId} to {toNodeId}");
                    }
                    else
                    {
                        var toNode = nodesList[toNodeId];
                        ConsoleHelper.PrintInfo($"Removing edge from {fromNodeId} to {toNodeId}");
                        fromNode.RemoveOutEdge(toNodeId);
                        toNode.RemoveInEdge(fromNodeId);
                        var newlyDisconnectedNodesList = nodesList.ValidateNodesAfterRemovedEdge(fromNodeId);
                        if(newlyDisconnectedNodesList.Count() > 0)
                        {
                            ConsoleHelper.PrintError($"Disconnected Array: [{String.Join(", ", newlyDisconnectedNodesList.ToArray())}]");
                        }
                    }
                }
            }
        }
    }
}
