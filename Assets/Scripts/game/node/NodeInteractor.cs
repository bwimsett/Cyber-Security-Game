using System.Collections.Generic;
using DefaultNamespace;
using GameAnalyticsSDK.Setup;
using UnityEngine;

public class NodeInteractor : MonoBehaviour {

    private NodeObject _nodeObject;
    private Vector2 dragStartPos;

    private Connection currentColliderConnection;
    private Connection tempConnection1;
    private Connection tempConnection2;

    private bool collidingWithTrash;

    private bool hasDragged = false;

    public float connectionBreakDistance;
    
    void Start() {
        _nodeObject = transform.parent.GetComponent<NodeObject>();
    }
    
    void OnMouseDown() {
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    void OnMouseDrag() {
        DragNode();
    }

    public void OnMouseUp() {  
        // Delete node if dropping on trash
        if (collidingWithTrash) {
            GameManager.levelScene.nodeManager.DeleteNode(_nodeObject.GetNode());
            GameManager.levelScene.guiManager.HideTrash();
            GameManager.currentLevel.RecalculateBudget();
            return;
        }
        
        if (_nodeObject.GetNode().IsControl() || GameManager.currentLevel.IsEditMode()) {
            GameManager.levelScene.guiManager.HideTrash();
        }

        Debug.Log("Has Dragged: "+hasDragged);
        
        if (!hasDragged) {
            OpenNodeSettings();
            return;
        }

        hasDragged = false;

        if (_nodeObject.GetNode().GetConnection() == null) {
            return;
        }
        
        _nodeObject.GetNode().GetConnection().AddConnectionNode(_nodeObject.GetNode());
    }

    public void OnMouseOver() {
        _nodeObject.nodeNameTextAnimator.SetBool("Visible", true);
    }

    public void OnMouseExit() {
        _nodeObject.nodeNameTextAnimator.SetBool("Visible", false);
    }

    public void RemoveTemporaryConnections() {
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection1);
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection2);
        _nodeObject.GetNode().GetConnection().Enable();
    }
    
    public void MoveConnectionNodeIntoPlace(Vector2 position) {
        // Check node family
        if (_nodeObject.GetNodeDefinition().nodeFamily != NodeFamily.Connection) {
            //Debug.Log("Mouse up, but not a connection node.");
            return;
        }

        Connection connectionParent = _nodeObject.GetNode().GetConnection();
        
        // Check for temporary connections
        if (connectionParent == null) {
            //Debug.Log("Mouse up, but no temporary connection creator");
            return;
        }
        
        //Debug.Log("Mouse up. Moving to mid point.");
        
        // Remove temporary connection creator
        Node end1 = connectionParent.start;
        Node end2 = connectionParent.end;
        //connectionParent.Disable();
        
        // Move node to middle of two 
        Vector2 end1pos = end1.nodeObject.transform.position;
        Vector2 end2pos = end2.nodeObject.transform.position;
        
        Vector2 midPoint = Vector2.Lerp(end1pos, end2pos, 0.5f);

        _nodeObject.MoveToPosition(position);
        
        RefreshConnections();
    }
    
    // Moves the node by the amount since the last dragging movement.
    public void DragNode() {
       bool isEditMode = GameManager.currentLevel.IsEditMode();
        bool isMovable = _nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Logical ||
                         _nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Connection;

        if (!isEditMode && !isMovable) {
            return;
        }
        

        Vector2 currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragDifference = currentDragPos - dragStartPos;

        dragStartPos = currentDragPos;

        if (dragDifference.magnitude > 0) {
            hasDragged = true;    
            Debug.Log(dragDifference.magnitude);
            // Show trash
            if (_nodeObject.GetNode().IsControl() || GameManager.currentLevel.IsEditMode()) {
                GameManager.levelScene.guiManager.ShowTrash();
            }
        }
        
        // Check node has actually been dragged
        if (!hasDragged) {
            return;
        }  

        Vector2 gridPos = GameManager.levelScene.grid.GetGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        _nodeObject.transform.position = gridPos;
        
        
        RefreshConnections();
    }

    public NodeObject GetNodeObject() {
        return _nodeObject;
    }

    // Creates a temporary connection on collision with a connection if this node is a connection node
    private void OnTriggerEnter2D(Collider2D other) {
        Connection connectionParent = _nodeObject.GetNode().GetConnection();
        
        // Check if this is the trash
        if (other.tag == "Trash") {
            GameManager.levelScene.guiManager.ShakeTrash();
            collidingWithTrash = true;
        }
        
        
        // Check this is a connection node
        NodeFamily family = _nodeObject.GetNodeDefinition().nodeFamily;
        if (family != NodeFamily.Connection) {
            return;
        }
        
        //Check node doesn't already have connections
        if (_nodeObject.GetNode().connectedNodes.Count > 0 && !connectionParent) {
            return;
        }
        
        // Check if collided with a connection
        ConnectionCollider connectionCollider = other.GetComponent<ConnectionCollider>();
        if (connectionCollider == null) {
            return;
        }
        
        // Check colliding connection isn't disabled
        if (connectionCollider.GetConnection().isDisabled()) {
            return;
        }

        // If the node already rests on a connection
        if (connectionParent) {
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

        if (connectionParent) {
            connectionParent.Enable();
        }
        
        _nodeObject.GetNode().SetConnection(startingConnection);
        tempConnection1 = GameManager.levelScene.connectionManager.CreateAndAddConnection(end1, thisNode);
        tempConnection2 = GameManager.levelScene.connectionManager.CreateAndAddConnection(thisNode, end2);
        tempConnection1.HideChevron();
        tempConnection2.HideChevron();
        
        startingConnection.Disable();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Trash") {
            GameManager.levelScene.guiManager.HideTrash();
            collidingWithTrash = false;
        }
        
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
        Connection connectionParent = _nodeObject.GetNode().GetConnection();
        if (connection != connectionParent) {
            return;
        }
        
        //Remove connection
        if (tempConnection1 || tempConnection2) {
            RemoveTemporaryConnections();
        }
        
        connectionParent.RemoveConnectionNode(_nodeObject.GetNode());
        _nodeObject.GetNode().ClearConnection();
    }
    
    public void RefreshConnections() {
        if (!_nodeObject) {
            return;
        }
        
        // Refresh connections
        Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(_nodeObject.GetNode()); 
        
        foreach (Connection c in connections) {
            c.Refresh();
        }
    }

    private void OpenNodeSettings() {
        GameManager.levelScene.guiManager.OpenControlSettingsWindow(_nodeObject.GetNode());
    }
}
