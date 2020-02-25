using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DefaultNamespace;
using UnityEngine;

public class ConnectionCollider : MonoBehaviour {

    private Connection connection;
    public BoxCollider2D collider;

    public bool drawingConnection = false;
    
    public void Refresh() {
        if (!connection) {
            connection = transform.parent.GetComponent<Connection>();
        }
        
        RefreshLength();
        RefreshAngle();
    }

    private void RefreshLength() {
        float length = Vector2.Distance(connection.lineRenderer.GetPosition(0), connection.lineRenderer.GetPosition(1));
        collider.size = new Vector2(length, connection.colliderThickness);
    }

    private void RefreshAngle() {
        Vector3 newPos = Vector2.Lerp(connection.lineRenderer.GetPosition(0),
            connection.lineRenderer.GetPosition(1), 0.5f);

        newPos.z = 0f;

        transform.position = newPos; 

        Vector2 p1 = connection.lineRenderer.GetPosition(0);
        Vector2 p2 = connection.lineRenderer.GetPosition(1);
        
        float angle = Mathf.Atan2(p2.y-p1.y, p2.x-p1.x)*180 / Mathf.PI;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!drawingConnection) {
            return;
        }
        
        NodeObject otherNodeObject = other.transform.GetComponent<NodeObject>();

        // If connection already exists, ignore it
        if (otherNodeObject && GameManager.levelScene.connectionManager.GetConnection(connection.start, otherNodeObject.GetNode())) {
            return;
        }

        if (!otherNodeObject || otherNodeObject.GetNode() == connection.start) {
            return;
        }
        
        // If node is not base or table, ignore it
        NodeDefinition nodeDef = otherNodeObject.GetNodeDefinition();

        if (nodeDef.nodeFamily != NodeFamily.Base) {
            return;
        }

        drawingConnection = false;
        
        //Debug.Log(otherNodeObject.transform.position);
        
        GameManager.levelScene.connectionManager.CreateAndAddConnection(connection.start, otherNodeObject.GetNode());
        GameManager.levelScene.connectionManager.RemoveConnection(connection);
    }

    void OnMouseUp() {
        if (GameManager.currentLevel.IsEditMode()) {
            connection.ToggleDuplex();
        }
    }
    
    public Connection GetConnection() {
        return connection;
    }
}
