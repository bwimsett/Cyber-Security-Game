using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class NodeObject : MonoBehaviour {
    
    
    public NodeInteractor nodeInteractor;
    public SpriteRenderer icon;
    public SpriteRenderer outline;
    
    private NodeDefinition nodeDefinition;
    private Node node;

    private Connection tempConnection;

    void Awake() {
        node = new Node(this, GameManager.currentLevel.GetNewNodeID());
    }
    
    void Start() {
        GameManager.levelScene.connectionManager.CreateConnectionsForNode(node);
    }
    
    public void SetNodeDefinition(NodeDefinition nodeDefinition) {
        this.nodeDefinition = nodeDefinition;
        Refresh();
    }

    public NodeDefinition GetNodeDefinition() {
        return nodeDefinition;
    }

    public Node GetNode() {
        return node;
    }

    public void Refresh() {
        icon.sprite = nodeDefinition.nodeIcon;
        outline.color = icon.color = nodeDefinition.nodeColor;
    }

    // Mouse functions for drawing new connections
    void OnMouseDown() {
        if (!GameManager.currentLevel.IsEditMode()) {
            return;
        }
        
        tempConnection = GameManager.levelScene.connectionManager.CreateAndAddConnection(node, null);
        tempConnection.start = node;
        tempConnection.connectionCollider.drawingConnection = true;
        tempConnection.lineRenderer.SetPosition(1, GameManager.levelScene.guiManager.GetMousePosition());
        tempConnection.connectionCollider.Refresh();
    }

    void OnMouseDrag() {
        if (!tempConnection) {
            return;
        }
        
        tempConnection.lineRenderer.SetPosition(1, GameManager.levelScene.guiManager.GetMousePosition());
        tempConnection.connectionCollider.Refresh();
    }

    void OnMouseUp() {
        if (!tempConnection) {
            return;
        }

        if (tempConnection.end != null) {
            tempConnection = null;
            return;
        }
        
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection);
        tempConnection = null;
    }

}
 