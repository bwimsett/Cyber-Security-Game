using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Node {

    public List<Node> connectedNodes;
    public NodeObject nodeObject;
    private int nodeID;

    public Node(NodeObject nodeObject, int nodeID) {
        this.nodeObject = nodeObject;
        this.nodeID = nodeID;
        connectedNodes = new List<Node>();
    }

    public void AddConnection(NodeObject nodeObject) {
        Node node = nodeObject.GetNode();
        
        if (!connectedNodes.Contains(node)) {
            connectedNodes.Add(node);
        }
    }

    public bool HasConnection(Node otherNode) {
        if (connectedNodes.Contains(otherNode)) {
            return true;
        }

        return false;
    }
    public Node[] GetConnectedNodes() {
        return connectedNodes.ToArray();
    }

    public int GetNodeID() {
        return nodeID;
    }

}
