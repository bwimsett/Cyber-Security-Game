using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace{
    public class NodeManager : MonoBehaviour{
        public Sprite baseNodeShape;
        public Sprite logicalNodeShape;
        public Sprite connectionNodeShape;
        public Sprite tableNodeShape;

        private List<Node> nodes;

        void Awake() {
            nodes = new List<Node>();
        }
        
        public Sprite GetNodeShapeSprite(Node node) {
            NodeType nodeType = node.nodeType;
            
            switch (nodeType) {
                case NodeType.Base:
                    return baseNodeShape;
                case NodeType.Table:
                    return tableNodeShape;
                case NodeType.Logical:
                    return logicalNodeShape;
                case NodeType.Connection:
                    return connectionNodeShape;
            }
            
            return null;
        }

        public Node CreateNode(Node node, Vector2 position) {
            Node newNode = Instantiate(node.gameObject, GameManager.levelScene.transform).GetComponent<Node>();
            newNode.transform.position = new Vector3(position.x, position.y, 0);
            nodes.Add(newNode);
            return newNode;
        }
    }
}