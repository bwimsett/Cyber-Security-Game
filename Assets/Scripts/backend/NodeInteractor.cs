using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NodeInteractor : MonoBehaviour {

    private Node node;
    private Vector2 dragStartPos;
    
    void Start() {
        node = transform.parent.GetComponent<Node>();
    }
    
    void OnMouseDown() {
        Debug.Log("Click");
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag() {
        DragNode();
    }

    // Moves the node by the amount since the last dragging movement.
    void DragNode() {
        Vector2 currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragDifference = currentDragPos - dragStartPos;
        node.transform.Translate(dragDifference);
        dragStartPos = currentDragPos;

        // Refresh connections
        Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(node);
        foreach (Connection c in connections) {
            c.RefreshPosition();
        }
    }

    public Node GetNode() {
        return node;
    }
}
