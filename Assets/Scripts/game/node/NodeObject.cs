using backend;
using DefaultNamespace;
using DefaultNamespace.node;
using GameAnalyticsSDK.Setup;
using TMPro;
using UnityEngine;


public class NodeObject : MonoBehaviour {
    
    public NodeInteractor nodeInteractor;
    public SpriteRenderer icon;
    public SpriteRenderer outline;
    
    [Range(0,1)]
    public float threatColorMix;
    public TextMeshProUGUI nodeNameText;
    public Animator nodeNameTextAnimator;
    public Animator nodeAnimator;
    public NodeHealthBar healthBar;
    public NodeAttackAnimationMonitor nodeAttackMonitor;
    
    public SpriteMask outlineMask;
    public SpriteRenderer outlineRenderer;
    public SpriteRenderer centerRenderer;

    public float moveSpeed;
    
    private NodeDefinition nodeDefinition;
    private Node node;
    private Color color;

    private Connection tempConnection;

    private Vector2 targetPos;

    void Update() {
        SetTempColor(Color.Lerp(nodeDefinition.nodeColor, GameManager.levelScene.threatManager.threatColor, threatColorMix));
        Move();
    }
    
    void Awake() {
        targetPos = Vector2.negativeInfinity;
        node = new Node(this);
    }
    
    void Start() {
        GameManager.levelScene.connectionManager.CreateConnectionsForNode(node);
        RefreshEditMode();
    }
    
    public void SetNodeDefinition(NodeDefinition nodeDefinition) {
        this.nodeDefinition = nodeDefinition;
        color = nodeDefinition.nodeColor;
        Refresh();
    }

    public void RefreshEditMode() {
        if (GameManager.currentLevel.IsEditMode() && nodeDefinition.nodeFamily == NodeFamily.Zone) {
            centerRenderer.enabled = true;
            outlineRenderer.enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            nodeInteractor.GetComponent<CircleCollider2D>().enabled = true;
        }
        else if(nodeDefinition.nodeFamily == NodeFamily.Zone){
            centerRenderer.enabled = false;
            outlineRenderer.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            nodeInteractor.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    
    public void SetTempColor(Color color) {
        this.color = color;
        Refresh();
    }

    public void SimulateAttack() {
        nodeAnimator.SetBool("attacked", true);
    }
    
    public void ResetThreatSimulation() {
        nodeAnimator.SetBool("attacked", false);
        color = nodeDefinition.nodeColor;
        nodeAttackMonitor.Reset();
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
        outline.color = icon.color = nodeNameText.color = color;
        nodeInteractor.RefreshConnections();
        nodeNameText.text = nodeDefinition.nodeName;
        healthBar.SetHealth(50,100);
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
            nodeInteractor.RemoveTemporaryConnections();
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
        tempConnection.RefreshChevron();
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

    public Color GetCurrentColor() {
        return color;
    }

}
 