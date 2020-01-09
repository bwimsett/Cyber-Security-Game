using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class NodeInteractor : MonoBehaviour {

    private NodeObject _nodeObject;
    private Vector2 dragStartPos;

    private Connection currentColliderConnection;
    private Connection tempConnection1;
    private Connection tempConnection2;
    private Connection tempConnectionCreator;
    
    void Start() {
        _nodeObject = transform.parent.GetComponent<NodeObject>();
    }
    
    void OnMouseDown() {
        Debug.Log("Click");
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag() {
        DragNode();
    }

    // Moves the node by the amount since the last dragging movement.
    public void DragNode() {
        Vector2 currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragDifference = currentDragPos - dragStartPos;
        _nodeObject.transform.Translate(dragDifference);
        dragStartPos = currentDragPos;

        // Refresh connections
        Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(_nodeObject.GetNode()); 
        
        foreach (Connection c in connections) {
            try {
                c.RefreshPosition();
            }
            catch (Exception e) {
                Debug.Log("Attempted to update "+connections.Length+" connections.");
                Debug.Log(c);
                throw;
            }
            
        }
    }

    public NodeObject GetNode() {
        return _nodeObject;
    }

    // Creates a temporary connection on collision with a connection if this node is a connection node
    private void OnTriggerEnter2D(Collider2D other) {
        // Check this is a connection node
        NodeFamily family = _nodeObject.GetNodeDefinition().nodeFamily;
        if (family != NodeFamily.Connection) {
            return;
        }
        
        // Check if collided with a connection
        ConnectionCollider connectionCollider = other.GetComponent<ConnectionCollider>();
        if (connectionCollider == null) {
            return;
        }
        
        //Create temporary connections between ends of connection
        Connection startingConnection = connectionCollider.GetConnection();
        Node end1 = startingConnection.start;
        Node end2 = startingConnection.end;

        Node thisNode = _nodeObject.GetNode();
        
        // Make sure neither end is this node
        if (end1 == thisNode || end2 == thisNode) {
            return;
        }
        
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection1);
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection2);

        tempConnectionCreator = startingConnection;
        tempConnection1 = GameManager.levelScene.connectionManager.CreateAndAddConnection(thisNode, end1);
        tempConnection2 = GameManager.levelScene.connectionManager.CreateAndAddConnection(thisNode, end2);
    }

    private void OnTriggerExit2D(Collider2D other) {
        // Check this is a connection node
        NodeFamily family = _nodeObject.GetNodeDefinition().nodeFamily;
        if (family != NodeFamily.Connection) {
            return;
        }
        
        // Check if collided with a connection
        ConnectionCollider connectionCollider = other.GetComponent<ConnectionCollider>();
        if (connectionCollider == null) {
            return;
        }
        
        // Check if connection is the temporary connection creator
        Connection connection = connectionCollider.GetConnection();
        if (connection != tempConnectionCreator) {
            return;
        }
        
        //Remove connection
        tempConnectionCreator = null;
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection1);
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection2);

        tempConnection1 = null;
        tempConnection2 = null;

    }
}
