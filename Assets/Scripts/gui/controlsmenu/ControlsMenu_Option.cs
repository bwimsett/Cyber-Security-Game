using DefaultNamespace;
using gui.controlsmenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu_Option : MonoBehaviour {

    public TextMeshProUGUI controlName;
    public TextMeshProUGUI controlCost;
    public Image controlImageOutline;
    public Image controlImageBackground;
    public Image controlIcon;
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
        controlIcon.sprite = node.nodeIcon;

        controlImageOutline.color = controlIcon.color = node.nodeColor;
        infoButton.SetNode(node);
    }

    void OnMouseDown() {
        _clonedNodeObject = GameManager.levelScene.nodeManager.CreateNode(node.nodeType, GameManager.levelScene.guiManager.GetMousePosition());
    }
    
    void OnMouseDrag() {
        if (_clonedNodeObject) {
            Vector3 mousePos = GameManager.levelScene.guiManager.GetMousePosition();
            mousePos.z = 0;
            
            _clonedNodeObject.transform.position = mousePos;
        }
    }
}
