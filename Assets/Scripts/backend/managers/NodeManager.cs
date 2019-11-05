using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace{
    public class NodeManager : MonoBehaviour{
        public Sprite baseNodeShape;
        public Sprite logicalNodeShape;
        public Sprite connectionNodeShape;
        public Sprite tableNodeShape;

        public GameObject defaultNode;

        public NodeDefinition[] nodeDefinitions;
        
        public Sprite GetNodeShapeSprite(NodeFamily family) {            
            switch (family) {
                case NodeFamily.Base:
                    return baseNodeShape;
                case NodeFamily.Table:
                    return tableNodeShape;
                case NodeFamily.Logical:
                    return logicalNodeShape;
                case NodeFamily.Connection:
                    return connectionNodeShape;
            }
            
            return null;
        }

        public NodeObject CreateNode(NodeType nodeType, Vector2 position) {
            NodeDefinition nodeDefinition = GetNodeScriptable(nodeType);
            
            if (!nodeDefinition) {
                Debug.Log("NodeDefinition "+nodeType+" not attached to NodeManager");
                return null;
            }
                      
            NodeObject newNodeObject = Instantiate(defaultNode, GameManager.levelScene.transform).GetComponent<NodeObject>();
            newNodeObject.transform.position = new Vector3(position.x, position.y, 0);
            newNodeObject.SetNodeDefinition(nodeDefinition);
            GameManager.currentLevel.nodes.Add(newNodeObject.GetNode());
            return newNodeObject;
        }

        private NodeDefinition GetNodeScriptable(NodeType nodeType) {
            foreach (NodeDefinition nodeSc in nodeDefinitions) {
                if (nodeSc.nodeType == nodeType) {
                    return nodeSc;
                }
            }

            return null;
        }
        
        
    }
}