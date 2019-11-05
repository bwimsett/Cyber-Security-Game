using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NodeInteractor : MonoBehaviour {

    private NodeObject _nodeObject;
    private Vector2 dragStartPos;
    
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
    void DragNode() {
        Vector2 currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragDifference = currentDragPos - dragStartPos;
        _nodeObject.transform.Translate(dragDifference);
        dragStartPos = currentDragPos;

        // Refresh connections
        Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(_nodeObject.GetNode());
        foreach (Connection c in connections) {
            c.RefreshPosition();
        }
    }

    public NodeObject GetNode() {
        return _nodeObject;
    }
}
