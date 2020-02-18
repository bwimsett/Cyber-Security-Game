using System.Collections.Generic;
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

        public int maxNodes = 256;
        
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
            if (GameManager.currentLevel.nodes.Count >= maxNodes) {
                Debug.Log("Maximum nodes reached.");
                return null;
            }
            
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
            if (result) {
                result.GetNode().SetNodeID(ID);
            }

            return result;
        }
        
        public NodeObject CreateNode(NodeSave nodeSave) {
            NodeObject result = CreateNode(nodeSave.nodeType, nodeSave.position.Vector2());
            result.GetNode().SetNodeID(nodeSave.id);
            result.GetNode().GetBehaviour().SetFields(nodeSave);
            result.GetNode().GetBehaviour().GetSelectedStartingThreats().SetOptionSet(result.GetNodeDefinition().GetStartingThreatsOptionSet());
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
                case NodeType.Sanitisation:
                    behaviour = new NodeBehaviour_Sanitisation(node);
                    break;
                case NodeType.CAPTCHA:
                    behaviour = new NodeBehaviour_Captcha(node);
                    break;
                case NodeType.Antivirus:
                    behaviour = new NodeBehaviour_Antivirus(node);
                    break;
                default:
                    behaviour = new NodeBehaviour(node);
                    break;
            }
            
            node.SetBehaviour(behaviour);

            if (node.GetBehaviour().GetSelectedStartingThreats() == null) {
                node.GetBehaviour().InitialiseStartingThreatSet();
            }
        }

        public Control_Dropdown_Option_Set GetControlOptionSet(ControlDropdownOptionSets setName) {
            foreach (Control_Dropdown_Option_Set set in optionSets) {
                if (set.setName == setName) {
                    return set;
                }
            }

            return null;
        }

        public Vector4[] GetNodeScreenPositionsForShader() {
            
            
            List<Node> nodes = GameManager.currentLevel.nodes;
            Vector4[] vectorArray = new Vector4[maxNodes];
            
            for (int i = 0; i < nodes.Count; i++) {
                // Get node position
                Vector3 nodePos = Camera.main.WorldToScreenPoint(nodes[i].nodeObject.transform.position);
                nodePos.y = Camera.main.pixelHeight - nodePos.y;
                // Normalise to clip space (-1,1)
                //nodePos = new Vector3(nodePos.x/Camera.main.pixelWidth*2-1, nodePos.y/Camera.main.pixelHeight*2-1);
                
                //Debug.Log(nodePos.x+" "+nodePos.y);
                vectorArray[i] = new Vector4(nodePos.x, nodePos.y, 0, 0);
            }

            //Debug.Log("Vector positions output: "+vectorArray.Length);

            return vectorArray;
        }
        
    }
}