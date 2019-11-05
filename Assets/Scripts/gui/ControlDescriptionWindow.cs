using System.Net.Mime;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gui {
    public class ControlDescriptionWindow : Window {

        public TextMeshProUGUI nodeDescription;
        public Image nodeOutline;
        public Image nodeInterior;
        public Image nodeIcon;
        
        public Animator animator;
        private NodeDefinition currentNode;
        
        public void SetVisible(bool value) {
            animator.SetBool("Visible", value);
        }

        public void SetNode(NodeDefinition node) {
            currentNode = node;
            Refresh();
        }

        public void Refresh() {
            SetTitle(currentNode.nodeName);
            nodeDescription.text = currentNode.description.text;
            nodeOutline.sprite = GameManager.levelScene.nodeManager.GetNodeShapeSprite(currentNode.nodeFamily);
            nodeInterior.sprite = nodeOutline.sprite;
            nodeIcon.sprite = currentNode.nodeIcon;
        }
             
    }
}