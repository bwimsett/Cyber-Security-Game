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
    private Vector2 dragStartPos;
    public List<Node> connectedNodes;
    public Color nodeColor;
    public Sprite nodeIcon;
    public TextAsset description;

    void Start() {
        GameManager.levelScene.connectionManager.CreateConnectionsForNode(this);
    }
    
    private void OnMouseDown() {
        Debug.Log("Node clicked",this);
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag() {
        DragNode();
    }

    // Moves the node by the amount since the last dragging movement.
    private void DragNode() {
        Vector2 currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragDifference = currentDragPos-dragStartPos;
        transform.Translate(dragDifference);
        dragStartPos = currentDragPos;
        
        // Refresh connections
        Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(this);
        foreach (Connection c in connections) {
            c.RefreshPosition();
        }
    }
}
 