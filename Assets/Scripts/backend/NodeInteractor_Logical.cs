using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NodeInteractor_Logical : MonoBehaviour {
    public NodeObject nodeObject;
    public NodeInteractor nodeInteractor;
    
    private NodeInteractor connectedNode;
    private Connection currentConnection;

    void Start() {
        // Object disables itself if the attached node is not a logical node
        if (nodeObject.GetNodeDefinition().nodeFamily != NodeFamily.Logical) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        NodeInteractor otherNode = other.gameObject.GetComponent<NodeInteractor>();

        if (!otherNode || otherNode == nodeInteractor) {
            return;
        }

        NodeDefinition nodeDef = otherNode.GetNodeObject().GetNodeDefinition();

        if (nodeDef.nodeFamily != NodeFamily.Base) {
            return;
        }

        if (connectedNode) {
            return;
        }

        // Create connection
        connectedNode = otherNode;
        currentConnection = GameManager.levelScene.connectionManager.CreateAndAddConnection(nodeObject.GetNode(),
            otherNode.GetNodeObject().GetNode());
        
        // Make connection dashed
        LineRenderer line = currentConnection.lineRenderer;
        //line.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
    }

    private void OnTriggerExit2D(Collider2D other) {
        NodeInteractor otherNode = other.gameObject.GetComponent<NodeInteractor>();

        if (otherNode != connectedNode) {
            return;
        }
        
        GameManager.levelScene.connectionManager.RemoveConnection(currentConnection);
        currentConnection = null;
        connectedNode = null;
    }
    
    
}
