using System.Collections;
using System.Collections.Generic;
using backend;
using DefaultNamespace;
using UnityEngine;

public class Node {

    public List<Node> connectedNodes;
    public NodeObject nodeObject;
    private int nodeID;
    private NodeBehaviour behaviour;

    public Node(NodeObject nodeObject) {
        this.nodeObject = nodeObject;
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

    public void SetNodeID(int id) {
        nodeID = id;
    }

    public ThreatStatus Attack(Threat threat) {
        return behaviour.Attack(threat);
    }

    public void EvolveThreat(Threat threat) {
        behaviour.EvolveThreat(threat);
    }
    
    public ThreatStatus GetThreatEffect(Threat threat) {
        Threat_EffectPair[] responses = nodeObject.GetNodeDefinition().threatResponses;   
        
        foreach (Threat_EffectPair t in responses) {
            if (t.threatType == threat.threatType) {
                return t.threatStatus;
            }
        }

        return ThreatStatus.Propagate;
    }

    public bool IsControl() {
        return nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Logical ||
               nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Connection;
    }
    
    public override string ToString() {
        return nodeObject.GetNodeDefinition().nodeName;
    }

    public void SetBehaviour(NodeBehaviour behaviour) {
        this.behaviour = behaviour;
    }

    public NodeBehaviour GetBehaviour() {
        return behaviour;
    }
}
