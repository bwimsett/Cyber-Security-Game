using DefaultNamespace;
using gui.controlsmenu;
using GameAnalyticsSDK.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu_Option : MonoBehaviour {

    public TextMeshProUGUI controlName;
    public TextMeshProUGUI controlCost;
    public Color lockedColor;
    public Image controlImageOutline;
    public Image controlImageBackground;
    public Image controlIcon;
    public Sprite lockedIcon;
    public ControlsMenu_InfoButton infoButton;
    
    private NodeDefinition node;
    private NodeObject _clonedNodeObject;

    // Sets the node type, and refreshes the option
    public void SetNode(NodeDefinition node) {
        this.node = node;
        Refresh();
    }

    // Refreshes all the information displayed in the option
    public void Refresh() {  
        controlName.text = node.nodeName;
        controlCost.text = node.nodeCost+" hrs";

        Sprite shape = GameManager.levelScene.nodeManager.GetNodeShapeSprite(node.nodeFamily);

        controlImageOutline.sprite = controlImageBackground.sprite = shape;
        
        controlImageOutline.color = controlIcon.color = node.nodeColor;
        infoButton.SetNode(node);

        controlIcon.sprite = node.nodeIcon;
        
        if (GameManager.currentSaveGame.GetTokens() < node.nodeUnlockTokens) {
            controlCost.text = node.nodeUnlockTokens + " Tokens";
            controlIcon.sprite = lockedIcon;
            controlIcon.color = controlImageOutline.color = lockedColor;
        }
    }

    void OnMouseDown() {
        if (GameManager.currentLevel.GetRemainingBudget() < node.nodeCost) {
            return;
        }
        
        int nodeId = GameManager.currentLevel.GetNewNodeID();
        _clonedNodeObject = GameManager.levelScene.nodeManager.CreateNode(node.nodeType, GameManager.levelScene.guiManager.GetMousePosition(), nodeId);
    }
    
    void OnMouseDrag() {
        if (_clonedNodeObject) {
            Vector3 mousePos = GameManager.levelScene.guiManager.GetMousePosition();
            mousePos.z = 0;
            
            _clonedNodeObject.nodeInteractor.DragNode();
            //_clonedNodeObject.transform.position = mousePos;
        }
    }

    void OnMouseUp() {
        if (_clonedNodeObject) {
            _clonedNodeObject.nodeInteractor.OnMouseUp();
            // Purchase
            bool success = GameManager.currentLevel.CanPurchaseForAmount(_clonedNodeObject.GetNodeDefinition().nodeCost);
            // Destroy node if not enough budget (this shouldn't be executed, but is a secondary check)
            if (!success) {
                Destroy(_clonedNodeObject.gameObject);
            }

            _clonedNodeObject = null;
            
            GameManager.currentLevel.RecalculateBudget();
        }
    }


    public NodeDefinition GetNode() {
        return node;
    }
}
