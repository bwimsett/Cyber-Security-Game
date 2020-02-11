using backend.level_serialization;
using backend.threat_modelling;
using UnityEngine;

namespace DefaultNamespace{
    public class NodeManager : MonoBehaviour{
        public Sprite baseNodeShape;
        public Sprite logicalNodeShape;
        public Sprite connectionNodeShape;
        public Sprite tableNodeShape;

        public GameObject defaultNode;

        public NodeDefinition[] nodeDefinitions;

        public Control_Dropdown_Option_Set[] optionSets;
        
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
            AssignNodeBehaviour(newNodeObject);
            return newNodeObject;
        }

        public NodeObject CreateNode(NodeType nodeType, Vector2 position, int ID) {
            NodeObject result = CreateNode(nodeType, position);
            result.GetNode().SetNodeID(ID);
            return result;
        }
        
        public NodeObject CreateNode(NodeSave nodeSave) {
            NodeObject result = CreateNode(nodeSave.nodeType, nodeSave.position.Vector2());
            result.GetNode().SetNodeID(nodeSave.id);
            result.GetNode().GetBehaviour().SetFields(nodeSave.fields);
            return result;
        }

        private NodeDefinition GetNodeScriptable(NodeType nodeType) {
            foreach (NodeDefinition nodeSc in nodeDefinitions) {
                if (nodeSc.nodeType == nodeType) {
                    return nodeSc;
                }
            }

            return null;
        }

        public void CreateNodeFromSave(NodeSave nodeSave) {
            NodeObject newNode = CreateNode(nodeSave);
        }

        public Node GetNodeByID(int id) {
            foreach (Node n in GameManager.currentLevel.nodes) {
                if (n.GetNodeID() == id) {
                    return n;
                }
            }

            return null;
        }

        private void AssignNodeBehaviour(NodeObject nodeObject) {
            NodeType nodeType = nodeObject.GetNodeDefinition().nodeType;

            Node node = nodeObject.GetNode();
            NodeBehaviour behaviour;
            
            switch (nodeType) {
                case NodeType.Table:
                    behaviour = new NodeBehaviour_Table(node);
                    break;
                case NodeType.Encryption:
                    behaviour = new NodeBehaviour_ConnectionEncryption(node);
                    break;
                default:
                    behaviour = new NodeBehaviour(node);
                    break;
            }
            
            node.SetBehaviour(behaviour);
        }

        public Control_Dropdown_Option_Set GetControlOptionSet(ControlDropdownOptionSets setName) {
            foreach (Control_Dropdown_Option_Set set in optionSets) {
                if (set.setName == setName) {
                    return set;
                }
            }

            return null;
        }
        
    }
}