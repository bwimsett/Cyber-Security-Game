﻿using System.Collections;
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
        
        // Set default behaviour
        behaviour = new NodeBehaviour(this);
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
        List<ThreatType> possibleEvolutions = new List<ThreatType>();
        Threat_EvolutionPair[] evolutionList = nodeObject.GetNodeDefinition().threatEvolutions;
        
        // Add any possible evolutions to the possibleEvolutions list
        foreach (Threat_EvolutionPair evolution in evolutionList) {
            if (evolution.threatType == threat.threatType) {
                possibleEvolutions.Add(evolution.evolution);
            }
        }
        
        //Pick an evolution at random
        if (possibleEvolutions.Count == 0) {
            throw new EvolutionNotDefinedException("Evolution not defined for "+threat.threatType+" at "+nodeObject.GetNodeDefinition().nodeName);
        }
        
        int chosenIndex = Random.Range(0, possibleEvolutions.Count);

        ThreatType chosenEvolution = possibleEvolutions[chosenIndex];
        
        // Create threat
        Threat newThreat = GameManager.levelScene.threatManager.CreateThreat(chosenEvolution, threat, this);
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
