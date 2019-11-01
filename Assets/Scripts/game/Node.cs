using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using UnityEngine;

public class Node : MonoBehaviour {

    public String nodeName;
    public int nodeCost;
    public NodeType nodeType;
    public List<Node> connectedNodes;
    public Color nodeColor;
    public Sprite nodeIcon;
    public TextAsset description;

    public NodeInteractor nodeInteractor;

    private Connection tempConnection;
    
    void Start() {
        GameManager.levelScene.connectionManager.CreateConnectionsForNode(this);
    }

    // Mouse functions for drawing new connections
    void OnMouseDown() {
        if (!GameManager.currentLevel.IsEditMode()) {
            return;
        }
        
        tempConnection = GameManager.levelScene.connectionManager.CreateAndAddConnection(this, null);
        tempConnection.start = this;
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

        if (tempConnection.end) {
            tempConnection = null;
            return;
        }
        
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection);
        tempConnection = null;
    }

    
    

}
 