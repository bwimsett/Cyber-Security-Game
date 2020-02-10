using DefaultNamespace;
using UnityEngine;


public class NodeObject : MonoBehaviour {
    
    
    public NodeInteractor nodeInteractor;
    public SpriteRenderer icon;
    public SpriteRenderer outline;

    public SpriteMask outlineMask;
    public SpriteRenderer outlineRenderer;
    public SpriteRenderer centerRenderer;

    public float moveSpeed;
    
    private NodeDefinition nodeDefinition;
    private Node node;

    private Connection tempConnection;

    private Vector2 targetPos;

    void Update() {
        Move();
    }
    
    void Awake() {
        targetPos = Vector2.negativeInfinity;
        node = new Node(this);
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
        outlineMask.sprite = outlineRenderer.sprite = centerRenderer.sprite =
            GameManager.levelScene.nodeManager.GetNodeShapeSprite(nodeDefinition.nodeFamily);
    }

    public void MoveToPosition(Vector2 position) {
        targetPos = position;
    }

    public void Move() {
        if (targetPos.Equals(Vector2.negativeInfinity)) {
            return;
        }

        transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed*Time.deltaTime);
        nodeInteractor.RefreshConnections();

        if (Vector2.Distance(transform.position, targetPos) < 0.1f) {
            transform.position = targetPos;
            targetPos = Vector2.negativeInfinity;
        }
    }

    // Mouse functions for drawing new connections
    void OnMouseDown() {
        if (!GameManager.currentLevel.IsEditMode()) {
            return;
        }

        if (nodeDefinition.nodeFamily != NodeFamily.Base) {
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

    public void Destroy() {
        GameManager.currentLevel.nodes.Remove(node);
        Destroy(gameObject);
    }

}
 