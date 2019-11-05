using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gui.controlsmenu {
    public class ControlsMenu_InfoButton : MonoBehaviour {

        private NodeDefinition node;
        public TextMeshProUGUI text;
        public Image outline;
        
        void OnMouseEnter() {
            if (Input.GetMouseButton(0)) {
                return;
            }
            
            GameManager.levelScene.guiManager.controlDescriptionWindow.SetNode(node);
            GameManager.levelScene.guiManager.controlDescriptionWindow.SetVisible(true);
        }

        void OnMouseExit() {
            GameManager.levelScene.guiManager.controlDescriptionWindow.SetVisible(false);
        }

        public void SetNode(NodeDefinition node) {
            this.node = node;
            text.color = node.nodeColor;
            outline.color = node.nodeColor;
        }
        
    }
}