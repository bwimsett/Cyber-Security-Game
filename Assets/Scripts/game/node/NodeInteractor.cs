using DefaultNamespace;
using UnityEngine;

public class NodeInteractor : MonoBehaviour {

    private NodeObject _nodeObject;
    private Vector2 dragStartPos;

    private Connection currentColliderConnection;
    private Connection tempConnection1;
    private Connection tempConnection2;
    private Connection tempConnectionCreator;

    private bool hasDragged = false;
    
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
        Debug.Log("Has Dragged: "+hasDragged);
        
        if (!hasDragged) {
            OpenNodeSettings();
        }

        hasDragged = false;
        
        MoveConnectionNodeIntoPlace();
    }

    private void MoveConnectionNodeIntoPlace() {
        // Check node family
        if (_nodeObject.GetNodeDefinition().nodeFamily != NodeFamily.Connection) {
            //Debug.Log("Mouse up, but not a connection node.");
            return;
        }
        
        // Check for temporary connections
        if (tempConnectionCreator == null) {
            //Debug.Log("Mouse up, but no temporary connection creator");
            return;
        }
        
        //Debug.Log("Mouse up. Moving to mid point.");
        
        // Remove temporary connection creator
        Node end1 = tempConnectionCreator.start;
        Node end2 = tempConnectionCreator.end;
        tempConnection1 = null;
        tempConnection2 = null;
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnectionCreator);
        
        // Move node to middle of two 
        Vector2 end1pos = end1.nodeObject.transform.position;
        Vector2 end2pos = end2.nodeObject.transform.position;
        
        Vector2 midPoint = Vector2.Lerp(end1pos, end2pos, 0.5f);

        _nodeObject.MoveToPosition(midPoint);
        
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
        // Check this is a connection node
        NodeFamily family = _nodeObject.GetNodeDefinition().nodeFamily;
        if (family != NodeFamily.Connection) {
            return;
        }
        
        //Check node doesn't already have connections
        if (_nodeObject.GetNode().connectedNodes.Count > 0 && !tempConnectionCreator) {
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

        if (tempConnectionCreator) {
            tempConnectionCreator.SetVisible(true);
        }
        
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection1);
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection2);

        tempConnectionCreator = startingConnection;
        tempConnectionCreator.SetVisible(false);
        tempConnection1 = GameManager.levelScene.connectionManager.CreateAndAddConnection(end1, thisNode);
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
        tempConnectionCreator.SetVisible(true);
        tempConnectionCreator = null;
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection1);
        GameManager.levelScene.connectionManager.RemoveConnection(tempConnection2);

        tempConnection1 = null;
        tempConnection2 = null;

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
