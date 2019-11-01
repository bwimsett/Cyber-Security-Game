using System.Collections;
using System.Collections.Generic;
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
        
        Node otherNode = other.transform.GetComponent<Node>();

        // If connection already exists, ignore it
        if (otherNode && GameManager.levelScene.connectionManager.GetConnection(connection.start, otherNode)) {
            return;
        }

        if (!otherNode || otherNode == connection.start) {
            return;
        }

        drawingConnection = false;
        
        connection.start.connectedNodes.Add(otherNode);
        otherNode.connectedNodes.Add(connection.start);
        
        Debug.Log(otherNode.transform.position);
        
        GameManager.levelScene.connectionManager.CreateAndAddConnection(connection.start, otherNode);
        GameManager.levelScene.connectionManager.RemoveConnection(connection);
    }
}
